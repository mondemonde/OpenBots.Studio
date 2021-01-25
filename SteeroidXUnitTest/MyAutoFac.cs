using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Steeroid.Business.Recorodings.Query;
using Steeroid.Business.Services;
using Steeroid.Infra2._0;

namespace SteeroidXUnitTest
{
    public static class MyAutoFac
    {
        static IContainer _container;
        public static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _setup();
                }
                return _container;
            }
        }

        static void _setup()
        {
            var builder = new ContainerBuilder();

            builder.RegisterMediatR(typeof(SearchRecordCmd).Assembly);

            builder.RegisterModule(new SteerInfrastructureModule(true));

            _container = builder.Build();
            BizService.Factory = _container;
        }


    }

}
