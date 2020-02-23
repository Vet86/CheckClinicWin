using Autofac;
using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckClinic.Detector
{
    public class Detector : IDetector
    {
        private IDataRequest _dataRequest;
        private IMailNotifier _mailNotifier;
        private Dictionary<IObserveData, IReadOnlyList<ITicket>> _data = new Dictionary<IObserveData, IReadOnlyList<ITicket>>(new ObserveDataComparer()); 
        public Detector()
        {
            _dataRequest = ContainerHolder.Container.Resolve<IDataRequest>();
            _mailNotifier = ContainerHolder.Container.Resolve<IMailNotifier>();
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
            if (_data.ContainsKey(observeData))
                _dataRequest.Remove(observeData);
        }

        private void onNewDataReceived(IObserveData observeData, IReadOnlyList<ITicket> newTickets)
        {
            var cacheTickets = _data[observeData];
            if (newTickets.Count != cacheTickets.Count)
            {
                if (newTickets.Count > cacheTickets.Count)
                {
                    // Узнать, какой именно билет появился
                    alarmNewTicket(observeData, findNewTickets(cacheTickets, newTickets));
                }
                _data[observeData] = newTickets;
            }
            Console.WriteLine($"tickets: {newTickets.Count}");
        }

        private void alarmNewTicket(IObserveData observeData, IEnumerable<ITicket> newTickets)
        {
            foreach (var ticket in newTickets)
            {
                Console.WriteLine($"{ticket.Id}");
            }
        }

        private IEnumerable<ITicket> findNewTickets(IReadOnlyList<ITicket> oldCollection, IReadOnlyList<ITicket> newCollection)
        {
            return Enumerable.Except(newCollection, oldCollection, new TicketComparer());
        }
    }
}
