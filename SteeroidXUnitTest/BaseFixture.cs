using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using MediatR;
using Steeroid.Models.Helpers;

namespace SteeroidXUnitTest
{
    public abstract class BaseFixture : IDisposable
    {
        public static IContainer MyFactory;

        public BaseFixture()
        {
            MyFactory = SteeroidXUnitTest.MyAutoFac.Container;//.Resolve<IMediator>();

        }

        public void Dispose()
        {
            //throw new NotImplementedException();         
            Agent.DisableTest();
        }
    }
}
