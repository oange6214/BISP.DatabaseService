using BISP.Infra.Dapper;
using BISP.Infra.EfCore;
using BISP.Infra.Entity.Data;
using BISP.Infra.Entity.Entities;
using BISP.Service.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BISP.Client.WPF.HostBuilders;

public static class AddDatabaseHostBuilderExtensions
{
    public static IHostBuilder AddDatabase(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostContext, services) =>
        {
            string defaultDb = hostContext.Configuration.GetSection("ConnectionDB")["Default"];
            string connectionString = hostContext.Configuration.GetConnectionString(defaultDb);

            // -- Dapper
            //services.AddSingleton<IRepository<Recipe>>(s
            //    => new DapperRepository<Recipe>(connectionString, defaultDb));

            // -- EfCore
            var options = new DbContextOptionsBuilder<BispContext>()
            //.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 17)))
            .UseNpgsql(connectionString)
            .Options;

            services.AddSingleton<IRepository<Recipe>>(s
                => new EfRepository<Recipe>(new BispContext(options)));            
        });

        return hostBuilder;
    }
}
