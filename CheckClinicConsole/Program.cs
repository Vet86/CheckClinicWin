using Autofac;
using CheckClinic.Model;

namespace CheckClinic.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CheckClinicConsoleModule>();
            var container = containerBuilder.Build();
            var dataRequest = container.Resolve<IDataRequest>();
            //dataRequest.Add()
        }
    }
}
