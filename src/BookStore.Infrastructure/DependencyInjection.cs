using Asp.Versioning;
using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Interfaces.Persistence.User;
using BookStore.Application.Abstractions.Interfaces.Services;
using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.Infrastructure.Abstractions.Authentication;
using BookStore.Infrastructure.Abstractions.Authorization;
using BookStore.Infrastructure.Abstractions.Interceptors;
using BookStore.Infrastructure.Abstractions.Services;
using BookStore.Infrastructure.ApplicationUser.Persistence;
using BookStore.Infrastructure.Outbox;
using BookStore.Infrastructure.Shared.Persistence;
using BookStore.Infrastructure.Shared.Persistence.Repositories.Base;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Quartz;

namespace BookStore.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        // Configure CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });





        AddLifeTimeServices(services, configuration);
        
        var dbConnection = configuration.GetConnectionString("BookStoreDbConnection");
        AddPersistence(services, dbConnection);
        AddHealthChecks(services, dbConnection);

        AddAuthorization(services);
        AddAuthentication(services, configuration);

        AddBackgroundJobs(services, configuration);
        AddSwaggerGen(services);
        AddApiVersioning(services);
        return services;

    }

    private static void AddLifeTimeServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<QueuedHostedService>();
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ITokenAccessor, TokenAccessor>();
        services.AddScoped<IJwtTokenReader, JwtTokenReader>();
        services.AddScoped<ITokenRevocationRepository, EfTokenRevocationRepository>();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
    }
    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();

    }
 
    private static void AddPersistence(IServiceCollection services, string? dbConnection)
    {
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<BookStoreAppContext>(
            (sp, options) =>
            {
                UpdateAuditableEntitiesInterceptor? auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
                if (auditableInterceptor != null)
                {
                    options     
                        .UseSqlServer(dbConnection,o=>o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
                        .AddInterceptors(auditableInterceptor)
                        .EnableSensitiveDataLogging();
                }
                else
                {
                    options.UseSqlServer(dbConnection);
                }
            }
        );


        services.AddIdentity<Domain.ApplicationUsers.ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<BookStoreAppContext>()
            .AddDefaultTokenProviders();


        services.AddScoped<IUserRepository, ApplicationUserRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
       
        //sp = Service Provider
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BookStoreAppContext>());
    }
 
    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
        services.Configure<RevokedTokenOptions>(configuration.GetSection("RevokedTokens"));

        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
        services.ConfigureOptions<PurgeRevokedTokensJobSetup>();
    }
    

    private static void AddHealthChecks(IServiceCollection services,string connectionString)
    {
        // Add health checks and optionally configure a simple check
        services.AddHealthChecks()
            .AddCheck("basic_health_check", () => HealthCheckResult.Healthy("The API is OK!"))
            .AddSqlServer(connectionString: connectionString, name: "database_check");


    }
    private static void AddSwaggerGen(IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var baseDirectory = AppContext.BaseDirectory;
            var serverXmlPath = Path.Combine(baseDirectory, "BookStore.Server.xml");
            opt.IncludeXmlComments(serverXmlPath);


       

        });
    }
    public static void AddApiVersioning(IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;


            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;

            });
    }



}
