using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookStore.Infrastructure.Shared.Persistence;

public class NassAppContextFactory : IDesignTimeDbContextFactory<BookStoreAppContext>
{
    public BookStoreAppContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookStoreAppContext>();

        // Default to "Development" if no environment argument is passed
        var environment = args.Length > 0 ? args[0] : "Development";

        // Build the configuration using the correct appsettings file based on the environment
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();


        // Get the connection string
        var bookStoreDbConnection = configuration.GetConnectionString("BookStoreDbConnection");



        Console.WriteLine(environment);
        Console.WriteLine($"BookStoreDbConnection: {bookStoreDbConnection}");

        // Configure the DbContext with the connection string
        optionsBuilder.UseSqlServer(bookStoreDbConnection);

        return new BookStoreAppContext(optionsBuilder.Options);
    }
}

