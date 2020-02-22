using Autofac;

namespace CheckClinic.Detector
{
    public static class ContainerHolder
    {
        public static IContainer Container { get; }
        static ContainerHolder()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DetectorModule>();
            Container = containerBuilder.Build();
        }
    }
}
