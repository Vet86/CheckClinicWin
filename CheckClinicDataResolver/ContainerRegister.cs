﻿using Autofac;

namespace CheckClinicDataResolver
{
    public static class ContainerRegister
    {
        public static IContainer Container { get; private set; }

        static ContainerRegister()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RequestSettings>().As<IDistrictCollectionRequestSettings>();
            Container = builder.Build();
        }
    }
}