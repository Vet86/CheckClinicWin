using Autofac;

namespace CheckClinic.Bot
{
    public static class ContainerHolder
    {
        public static IContainer Container { get; }
        static ContainerHolder()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CheckClinicBotModule>();
            Container = containerBuilder.Build();
        }
    }
}
