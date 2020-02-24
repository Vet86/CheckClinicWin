using Autofac;
using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CheckClinic.Detector
{
    public class Detector : IDetector
    {
        private IDataRequest _dataRequest;
        private IMailNotifier _mailNotifier;
        private Dictionary<IObserveData, IReadOnlyList<ITicket>> _data = new Dictionary<IObserveData, IReadOnlyList<ITicket>>(new ObserveDataComparer());
        private Dictionary<IObserveData, string> _observeDataToDoctor = new Dictionary<IObserveData, string>(new ObserveDataComparer());
        
        public Detector()
        {
            _dataRequest = ContainerHolder.Container.Resolve<IDataRequest>();
            _mailNotifier = ContainerHolder.Container.Resolve<IMailNotifier>();
            _dataRequest.NewDataReceived += onNewDataReceived;
            _dataRequest.SetInterval(TimeSpan.FromMinutes(5));
        }

        public void Add(IObserveData observeData, string doctorName = null)
        {
            if (_data.ContainsKey(observeData))
                return;

            _dataRequest.Add(observeData);
            var tickets = _dataRequest.Receive(observeData);
            _data.Add(observeData, tickets);
            string doctorText = doctorName ?? observeData.DoctorId;
            System.Console.WriteLine($"{doctorText} has {tickets.Count()} tickets");
            _observeDataToDoctor.Add(observeData, doctorName);
        }

        public void Add(IObserveData observeData, DateTime dateTime, string doctorName = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(IObserveData observeData)
        {
            if (_data.ContainsKey(observeData))
            {
                _data.Remove(observeData);
                _dataRequest.Remove(observeData);
                _observeDataToDoctor.Remove(observeData);
            }
        }

        private void onNewDataReceived(IObserveData observeData, IReadOnlyList<ITicket> newTickets)
        {
            try
            {
                File.AppendAllText($"log{DateTime.Now.ToShortDateString()}.txt", $"{DateTime.Now.ToString()}: {_observeDataToDoctor[observeData]} has {newTickets.Count} tickets\n");
                var cacheTickets = _data[observeData];
                var newAddedTickets = findNewTickets(cacheTickets, newTickets);
                if (cacheTickets.Count != newTickets.Count)
                {
                    System.Console.WriteLine($"{_observeDataToDoctor[observeData]} has {newTickets.Count} tickets");
                }
                if (newAddedTickets.Any())
                {
                    alarmNewTicket(observeData, newAddedTickets);
                }
                _data[observeData] = newTickets;
            }
            catch(Exception ex)
            {
                File.AppendAllText($"log{DateTime.Now.ToShortDateString()}.txt", $"{DateTime.Now.ToString()}: {ex.Message}\n");
            }
        }

        private void alarmNewTicket(IObserveData observeData, IEnumerable<ITicket> newTickets)
        {
            foreach(var ticket in newTickets)
            {
                System.Console.WriteLine($"{_observeDataToDoctor[observeData]} add new {ticket.Id} ticket");
            }
            var mailTextPreparer = new MailTextPreparer(observeData, newTickets.ToList(), _observeDataToDoctor[observeData]);
            _mailNotifier.Send(mailTextPreparer.Title, mailTextPreparer.Content);
        }

        private IEnumerable<ITicket> findNewTickets(IReadOnlyList<ITicket> oldCollection, IReadOnlyList<ITicket> newCollection)
        {
            return Enumerable.Except(newCollection, oldCollection, new TicketComparer());
        }

        public void AddMailReceiver(string mailReceiver)
        {
            _mailNotifier.AddReceiver(mailReceiver);
        }
    }
}