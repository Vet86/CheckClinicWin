using Autofac;
using CheckClinic.Model;
using System;
using System.Collections.Generic;

namespace CheckClinic.Detector
{
    public class Detector : IDetector
    {
        private IDataRequest _dataRequest;
        private Dictionary<IObserveData, TicketCollection> _data = new Dictionary<IObserveData, TicketCollection>(new ObserveDataComparer()); 
        public Detector()
        {
            _dataRequest = ContainerHolder.Container.Resolve<IDataRequest>();
            _dataRequest.NewDataReceived += onNewDataReceived;
            _dataRequest.SetInterval(TimeSpan.FromSeconds(10));
        }

        public void Add(IObserveData observeData)
        {
            _dataRequest.Add(observeData);
            _data.Add(observeData, _dataRequest.Receive(observeData));
        }

        public void Add(IObserveData observeData, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public void Remove(IObserveData observeData)
        {
            throw new NotImplementedException();
        }

        private void onNewDataReceived(IObserveData observeData, TicketCollection ticket)
        {
            var cache = _data[observeData];
            if (ticket.Tickets.Count != cache.Tickets.Count)
            {
                if (ticket.Tickets.Count > cache.Tickets.Count)
                {
                    // Узнать, какой именно билет появился
                    alarmNewTicket();
                }
                _data[observeData] = ticket;
            }
            Console.WriteLine($"tickets: {ticket.Tickets.Count}");
        }

        private void alarmNewTicket()
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}
