using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemaWareHouse.Persistency;

namespace RemaWareHouse
{
    public class Program
    {

        private static IHost _host;
        public static void Main(string[] args)
        {
            _host = CreateHostBuilder(args).Build();

            SetupDbContext();
            
            _host.Run();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
        private static void SetupDbContext()
        {
            IServiceProvider serviceProvider = GetServiceProvider();

            try
            {
                SetupWarehouseContext(serviceProvider.GetRequiredService<WarehouseContext>());
            }
            catch (Exception exception)
            {
                LogException(exception, serviceProvider.GetRequiredService<ILogger<Program>>());
            }
        }

        private static void SetupWarehouseContext(WarehouseContext warehouseDbContext)
        {
            DatabaseFacade facade = warehouseDbContext.Database;
            
            facade.Migrate();
            facade.EnsureCreated();
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceScope scope = _host.Services.CreateScope();

            return scope.ServiceProvider;
        }
        
        private static void LogException(Exception exception, ILogger logger)
        {
            logger.LogError(exception, "An error occurred creating the DB: ");
        }
        
    }
}