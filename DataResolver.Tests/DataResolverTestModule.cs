using Autofac;
using CheckClinic.DataResolver;
using CheckClinic.Interfaces;
using CheckClinicDataResolver;

namespace CheckClinic.Complex.Tests
{
    public static class ContainerHolder
    {
        public static IContainer Container { get; }
        static ContainerHolder()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DataResolverTestModule>();
            Container = containerBuilder.Build();
        }
    }
    public class DataResolverTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TicketCollectionDataResolver>().As<ITicketCollectionDataResolver>();
            builder.RegisterType<RequestSettings>().As<IRequestSettings>();
            base.Load(builder);
        }
    }
}