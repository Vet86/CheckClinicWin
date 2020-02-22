using Autofac;

namespace CheckClinic.Console
{
    public static class ContainerHolder
    {
        public static IContainer Container { get; }
        static ContainerHolder()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CheckClinicConsoleModule>();
            Container = containerBuilder.Build();
        }
    }
}
