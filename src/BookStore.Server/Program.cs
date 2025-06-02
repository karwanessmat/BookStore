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


app.UseDefaultFiles(); 
app.UseStaticFiles(); 




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



    app.ApplyMigrations();

}



app.UseCors("AllowAllOrigins"); 
app.UseCustomMiddleware();


app.UseAuthentication();
app.UseAuthorization();



app.MapControllers()
    .WithOpenApi()
    .WithTags("BookStore");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    Predicate = _ => true
});




app.MapFallbackToFile("/index.html");

app.Run();



namespace BookStore.Server
{
    public partial class Program;
}
