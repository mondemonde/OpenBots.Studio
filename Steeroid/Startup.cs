using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeroid._Infra;
using Steeroid.Business.License.Commands;
using Steeroid.Business.Recorodings.Query;
using Steeroid.Business.Services;
using Steeroid.Data;
using Steeroid.Infra2._0;
using Steeroid.Infra2._0.Areas.Machine;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Services.License;
using MediatR.Pipeline;
using DevNoteHub.API.Service;
using Steeroid.Infra2._0.DAL;
using Steeroid.Business.Areas.Recordings;
using Steeroid.Infra2._0.Areas.Recordings;
using Steeroid.Infra2._0.Areas.Communication;
using Steeroid.Models.BaseInterfaces;

namespace Steeroid
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(RegisterCmd).Assembly);      

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<MachineService>();

            string connectionString = Configuration.GetConnectionString("SqliteConnection");  //Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext(connectionString);

            //services.AddMediatR(typeof(Startup));
            //services.AddMediatR(typeof(RegisterCmd).Assembly);
            //services.AddMediatR(typeof(SearchRecordCmd).Assembly);
            //services.AddScoped(typeof(IUniversityRepository), typeof(UniversitySqlServerRepository));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            BizService.AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //STEP_.AZURE_BUS #1 Entry poinnt
            string banner = @"    
  ___   _                             _      _ 
 / __| | |_   ___   ___   _ _   ___  (_)  __| |
 \__ \ |  _| / -_) / -_) | '_| / _ \ | | / _` |
 |___/  \__| \___| \___| |_|   \___/ |_| \__,_|
                                               
  
";
            Console.WriteLine(banner);


            var asyncResult = AsyncHelper.RunSync<ServiceResponse<bool>>(async () =>
            {
                MyBusServiceManager azureFront =
                new MyBusServiceManager(new MyMachineManager());

                return await azureFront.Step1_InitAzure();
            });

            if (asyncResult.model == false)
                Agent.LogError("Running with no Bus Service connection.");

        }


        #region FROM CLEAN Architech------------------------

        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            Configuration = config;
            _env = env;
        }

        public ILifetimeScope AutofacContainer { get; private set; }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new SteerInfrastructureModule(_env.EnvironmentName == "Development"));

        }


        #endregion


    }
}
