using Autofac;
using CheckClinic.Interfaces;
using CheckClinic.Model;

namespace CheckClinic.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var detector = ContainerHolder.Container.Resolve<IDetector>();
            //dataRequest.Add(new ObserveData("255", "д62.51"));
            detector.Add(new ObserveData("255", "д62.62"));
            System.Console.ReadKey();
        }
    }
}
