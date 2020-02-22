using System;
using Autofac;
using CheckClinic.Model;

namespace CheckClinic.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new Processor();
            System.Console.ReadKey();
        }
    }

    class Processor
    {
        public Processor()
        {
            var dataRequest = ContainerHolder.Container.Resolve<IDataRequest>();
            dataRequest.NewDataReceived += onNewDataReceived;
            dataRequest.SetInterval(TimeSpan.FromSeconds(10));
            //dataRequest.Add(new ObserveData("255", "д62.51"));
            dataRequest.Add(new ObserveData("255", "д62.62"));
            dataRequest.Start();
            //dataRequest.Add()
        }

        private void onNewDataReceived(IObserveData observeData, TicketCollection ticket)
        {
            System.Console.WriteLine($"tickets: {ticket.Tickets.Count}");
        }
    }
}
