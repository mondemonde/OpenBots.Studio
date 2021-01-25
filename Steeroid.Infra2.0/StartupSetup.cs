//using CleanArchitecture1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeroid.Infra2._0.DAL;

namespace Steeroid.Infra2._0
{
    public static class StartupSetup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MyDbContext>();// (options => options.UseSqlite(connectionString)); // will be created in web project root

        public static IHost MyHost { get; set; }


    }
}
