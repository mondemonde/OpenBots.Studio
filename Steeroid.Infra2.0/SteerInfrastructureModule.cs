using Autofac;
using Steeroid.Business.License;
using Steeroid.Infra2._0.DAL;
//using Steeroid.Infra2._0.Services;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System.Collections.Generic;
using System.Reflection;
using Module = Autofac.Module;
using Steeroid.Infra2._0.Areas.License;
using Steeroid.Models.Enums;
using Steeroid.Business.Areas.Communication;
using Steeroid.Infra2._0.Areas.Communication;
using Steeroid.Models.Helpers;
using Steeroid.Infra2._0.Helpers;
using DevNoteHub.API.Service;
using Steeroid.Business.Areas.Remoting;
using Steeroid.Business.Machine;
using Steeroid.Business.Areas.Recordings;
using Steeroid.Infra2._0.Areas.Recordings;
using Microsoft.Extensions.DependencyInjection;
using Steeroid.Infra2._0.Areas.Machine;
using MediatR;
using Steeroid.Business.Recorodings.Query;
using Steeroid.Infra2._0.Areas.Scripting;
using Steeroid.Models.Interfaces.Scripting;
using Steeroid.Infra2._0.Areas.Bot;
using Steeroid.Business.Areas.Bot;
using Steeroid.Business.Areas.Input;

namespace Steeroid.Infra2._0
{
    public class SteerInfrastructureModule : Module
    {
        private bool _isDevelopment = false;
        private List<Assembly> _assemblies = new List<Assembly>();

        public SteerInfrastructureModule(bool isDevelopment, Assembly callingAssembly = null)
        {
            _isDevelopment = isDevelopment;
            var coreAssembly = Assembly.GetAssembly(typeof(MachineServer));

            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));

            _assemblies.Add(coreAssembly);

            _assemblies.Add(infrastructureAssembly);
            if (callingAssembly != null)
            {
                _assemblies.Add(callingAssembly);
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }
            RegisterCommonDependencies(builder);


        }

        private void RegisterCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventDispatcher>().As<IDomainEventDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(_assemblies.ToArray())
                 .AsClosedTypesOf(typeof(IHandle<>));

            //STEP_.AUTOFAC #1 REgister here...

            // Mediator itself
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            //// request & notification handlers
            //builder.Register<ServiceFactory>(context =>
            //{
            //    var c = context.Resolve<IComponentContext>();
            //    return t => c.Resolve(t);
            //});

            //// finally register our custom code (individually, or via assembly scanning)
            //// - requests & handlers as transient, i.e. InstancePerDependency()
            //// - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            //// - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(SearchRecordCmd).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            //          



            var db = new MyDbContext(); //StartupSetup.MyHost.Services.GetRequiredService<MyDbContext>();

            builder.RegisterType<EfRepository>().As<IRepository>()
                  .WithParameter(new TypedParameter(typeof(MyDbContext), db))
                .InstancePerLifetimeScope();

            builder.RegisterType<LicenseManager>().As<ILicenseManager>()
                        .InstancePerLifetimeScope();

          

            var logger = new SLogger();
            Agent.SetLogger(logger);

            builder.RegisterInstance(logger).As<INote>().SingleInstance();

            //builder.RegisterType<Ef6Repository>().As<IRepository>()
            // .WithParameter(new TypedParameter(typeof(MyDBContext), new MyDBContext()))
            // .InstancePerLifetimeScope();
            var mac = new MyMachineManager();
            builder.RegisterType<MyMachineManager>().As<IMachineManager>().InstancePerLifetimeScope();

            var sync = new SynchManager(mac);
            var scriptManager = new ScriptManager();

            var recordMngr = new RecordManager(new EfRepository(db), sync, scriptManager);

            builder.RegisterType<RecordManager>().As<IRecordManager>()
            .WithParameter(new TypedParameter(typeof(IRepository),new EfRepository(db)))
            .WithParameter(new TypedParameter(typeof(ISynchManager), sync))
            .WithParameter(new TypedParameter(typeof(IScriptManager), scriptManager))

            .InstancePerLifetimeScope();

            builder.RegisterType<SynchManager>().As<ISynchManager>()
            .WithParameter(new TypedParameter(typeof(IMachineManager), mac))
            .InstancePerLifetimeScope();

            var thisMac = mac.GetMachineRecord();

            builder.RegisterType<MyQueueClient>().As<IBizQueueClient>()
           .WithParameter(new TypedParameter(typeof(MachineServer), thisMac))
           .InstancePerLifetimeScope();

            var bot = new MyOpenBot();

           builder.RegisterType<BizInputManager>()//.As<BizInputManager>()
          .WithParameter(new TypedParameter(typeof(IOPenBot),bot))
          .InstancePerLifetimeScope();

            builder.RegisterType<MyOpenBot>().As<IOPenBot>().InstancePerLifetimeScope();

            //builder.RegisterType<EmailSender>().As<IEmailSender>()
            //    .InstancePerLifetimeScope();
        }

        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add development only services
        }

        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add production only services
        }

    }
}
