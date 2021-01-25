using Autofac;
using Steeroid.Business.Areas.Remoting;
using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Infra2._0.Services
{
   public static class GlobalService
    {
        public static IContainer Factory { get; set; }

        ///public static IContainer Container { get; set; }
        public static ISynchManager GetSynchManager()
        {
            return Factory.Resolve<ISynchManager>();
        }

        public static IRepository GetRepository()
        {
            return Factory.Resolve<IRepository>();
        }
    }
}
