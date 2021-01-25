using Autofac;
using MediatR;
using Steeroid.Business.Areas.Communication;
using Steeroid.Business.Areas.Remoting;
using Steeroid.Models.Interfaces;
using Steeroid.Models.Interfaces.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Steeroid.Business.Services
{
    public static class BizService
    {

        public static IContainer Factory { get; set; }

        public static ILifetimeScope AutofacContainer { get; set; }

        public static T Resolve<T>()
        {
            if (Factory != null)
                return Factory.Resolve<T>();
            else
                return AutofacContainer.Resolve<T>();
        }


        public static IBizQueueClient MyAzure { get; set; }

        public static Dictionary<string, string> TasktVars { get; set; }

        public static IScriptDirector ScriptDirector { get; set; }

        /// <summary>
        /// For FrameWork 4.5 up only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="IsHandlerOnly"></param>
        public static void BuildAutofacMediaR<T>(ref ContainerBuilder builder,bool IsHandlerOnly=false)
        {
            if (IsHandlerOnly == false)
            {
                // Mediator itself
                builder
                    .RegisterType<Mediator>()
                    .As<IMediator>()
                    .InstancePerLifetimeScope();

                // request & notification handlers
                builder.Register<ServiceFactory>(context =>
                {
                    var c = context.Resolve<IComponentContext>();
                    return t => c.Resolve(t);
                });
            }

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(T).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
                                                                                                            //builder.RegisterType<MyHandler>().AsImplementedInterfaces().InstancePerDependency();          // or individually
        }

        public static IMediator Mediator
        {
            get
            {
                return Resolve<IMediator>();
            }
        }
    }
}
