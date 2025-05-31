using Asp.Versioning.ApiExplorer;
using BookStore.Application;
using BookStore.Infrastructure;
using BookStore.Server;
using BookStore.Server.Extensions;
using BookStore.Server.OpenApi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();





builder.AddServer();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddHealthChecks();


builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

WebApplication? app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.UseDefaultFiles(); // This will look for default files such as index.html
app.UseStaticFiles();  // This serves static files from wwwroot




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (ApiVersionDescription description in app.DescribeApiVersions())
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });



    // automate using update-database command 
    app.ApplyMigrations();

   // app.SeedContractor();
}



app.UseCors("AllowAllOrigins"); // Apply CORS after custom OPTIONS middleware
app.UseCustomMiddleware();

// it is used to check the validation of jwt 
//app.UseJwtDiagnosticMiddleware();

app.UseAuthentication();
app.UseAuthorization();



// Configure the endpoints
app.MapControllers()
    .WithOpenApi()
    .WithTags("BookStore");

// Document health checks with authorization
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = _ => true
});/*.RequireAuthorization("AdminOnly")*/;  // Apply the "AdminOnly" policy




app.MapFallbackToFile("/index.html");

app.Run();



namespace BookStore.Server
{
    public partial class Program;
}
