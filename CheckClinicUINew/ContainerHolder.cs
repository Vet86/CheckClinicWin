using Autofac;

namespace CheckClinic.UI
{
    public static class ContainerHolder
    {
        public static IContainer Container { get; }
        static ContainerHolder()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CheckClinicUIModule>();
            Container = containerBuilder.Build();
        }
    }
}
