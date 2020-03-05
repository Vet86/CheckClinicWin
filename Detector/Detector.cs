﻿using Autofac;
using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace CheckClinic.Detector
{
    public class Detector : IDetector
    {
        private IDataRequest _dataRequest;
        private IMailNotifier _mailNotifier;
        private Dictionary<IObserveData, IReadOnlyList<ITicket>> _data = new Dictionary<IObserveData, IReadOnlyList<ITicket>>(new ObserveDataComparer());
        private List<IDetectListener> _listeners = new List<IDetectListener>();

        public Detector()
        {
            _dataRequest = ContainerHolder.Container.Resolve<IDataRequest>();
            _mailNotifier = ContainerHolder.Container.Resolve<IMailNotifier>();
            _dataRequest.NewDataReceived += onNewDataReceived;
            _dataRequest.SetInterval(TimeSpan.FromMinutes(5));
        }

        public IReadOnlyList<IObserveData> GetObserves()
        {
            return _data.Keys.ToList();
        }

        public void Add(IObserveData observeData)
        {
            if (_data.ContainsKey(observeData))
                return;

            _dataRequest.Add(observeData);
            var tickets = _dataRequest.Receive(observeData);
            _data.Add(observeData, tickets);
            string doctorText = observeData.DoctorName ?? observeData.DoctorId;
            System.Console.WriteLine($"{doctorText} has {tickets.Count()} tickets");
        }

        public void Add(IObserveData observeData, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public void Remove(IObserveData observeData)
        {
            if (_data.ContainsKey(observeData))
            {
                _data.Remove(observeData);
                _dataRequest.Remove(observeData);
            }
        }

        private void onNewDataReceived(IObserveData observeData, IReadOnlyList<ITicket> newTickets)
        {
            try
            {
                File.AppendAllText($"log{DateTime.Now.ToShortDateString()}.txt", $"{DateTime.Now.ToString()}: {observeData.DoctorName} has {newTickets.Count} tickets\n");
                var cacheTickets = _data[observeData];
                var newAddedTickets = findNewTickets(cacheTickets, newTickets);
                if (cacheTickets.Count != newTickets.Count)
                {
                    System.Console.WriteLine($"{observeData.DoctorName} has {newTickets.Count} tickets");
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
                System.Console.WriteLine($"{observeData.DoctorName} add new {ticket.Id} ticket");
            }
            if (_mailNotifier.GetReceivers().Any())
            {
                var mailTextPreparer = new MailTextPreparer(observeData, newTickets.ToList(), observeData.DoctorName);
                _mailNotifier.Send(mailTextPreparer.Title, mailTextPreparer.Content);
            }
            foreach(var listener in _listeners)
            {
                listener.NewTicketsAdded(observeData, newTickets);
            }
        }

        private IEnumerable<ITicket> findNewTickets(IReadOnlyList<ITicket> oldCollection, IReadOnlyList<ITicket> newCollection)
        {
            return Enumerable.Except(newCollection, oldCollection, new TicketComparer());
        }

        public void AddMailReceiver(MailAddress mailReceiver)
        {
            _mailNotifier.AddReceiver(mailReceiver);
        }

        public IReadOnlyList<MailAddress> GetMailReceivers()
        {
            return _mailNotifier.GetReceivers();
        }

        public void AddListener(IDetectListener detectListener)
        {
            _listeners.Add(detectListener);
        }

        public void ClearReceivers()
        {
            _mailNotifier.ClearAllReceivers();
        }
    }
}