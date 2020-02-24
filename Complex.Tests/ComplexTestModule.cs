using Autofac;
using CheckClinic.DataParser;
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
            containerBuilder.RegisterModule<ComplexTestModule>();
            Container = containerBuilder.Build();
        }
    }
    public class ComplexTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TicketCollectionDataResolver>().As<ITicketCollectionDataResolver>();
            builder.RegisterType<TicketCollectionParser>().As<ITicketCollectionParser>();
            builder.RegisterType<RequestSettings>().As<IRequestSettings>();
            base.Load(builder);
        }
    }
}