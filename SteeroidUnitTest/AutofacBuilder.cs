using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Steeroid.Infra2._0;

namespace SteeroidUnitTest
{
    public static class MyAutoFac
    {
        static IContainer _container;
        public static IContainer Container
        {
            get
            {
                if(_container==null)
                {
                    _setup();
                }
                return _container;
            }
        }

        static void _setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new SteerInfrastructureModule(true));

            _container = builder.Build();
        }
    }
}
