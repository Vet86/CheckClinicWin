using Autofac;
using CheckClinic.Detector.Model;
using CheckClinic.Interfaces;
using CheckClinic.Model;
using Newtonsoft.Json;
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
            _dataRequest.SetInterval(TimeSpan.FromMinutes(1));
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

        public void RemoveAll()
        {
            foreach(var item in _data.Keys.ToArray())
            {
                _data.Remove(item);
                _dataRequest.Remove(item);
            }
        }


        public bool Exists(IObserveData observeData)
        {
            return _data.ContainsKey(observeData);
        }

        private void onNewDataReceived(IObserveData observeData, IReadOnlyList<ITicket> newTickets)
        {
            try
            {
                File.AppendAllText(getLogFileName(), $"{DateTime.Now.ToString()}: {observeData.DoctorName} has {newTickets.Count} tickets\n");
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
                File.AppendAllText(getLogFileName(), $"{DateTime.Now.ToString()}: {ex.Message}\n");
            }
        }

        private string getLogFileName()
        {
            string dateFormat = DateTime.Now.ToShortDateString().Replace("/", "_").Replace("\\", "_");
            return $"log_{dateFormat}.txt";
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

        public void SetUpdateInterval(TimeSpan interval)
        {
            _dataRequest.SetInterval(interval);
        }

        public TimeSpan GetUpdateInterval()
        {
            return _dataRequest.GetInterval();
        }

        public string ExportSettings()
        {
            var settingsModel = new SettingsModel();
            foreach(var receiver in _mailNotifier.GetReceivers())
            {
                settingsModel.ReceiverModels.Add(new ReceiverModel() { Email = receiver.Address });
            }
            settingsModel.Interval = _dataRequest.GetInterval();
            settingsModel.Version = new Version(0, 0, 0, 1);

            return serialize(settingsModel);
        }

        public void ImportSettings(string settings)
        {
            if (string.IsNullOrWhiteSpace(settings))
                return;

            var settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settings);
            _mailNotifier.ClearAllReceivers();
            if (settingsModel != null)
            {
                foreach (var receiver in settingsModel.ReceiverModels)
                {
                    _mailNotifier.AddReceiver(new MailAddress(receiver.Email));
                }
                _dataRequest.SetInterval(settingsModel.Interval);
            }
        }

        public string ExportData()
        {
            var observerCollectionModel = new ObserverCollectionModel();
            foreach (var observeData in GetObserves())
            {
                observerCollectionModel.Observers.Add(new ObserveData(observeData));
            }

            return serialize(observerCollectionModel);
        }

        public void ImportData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            var observerCollectionModel = JsonConvert.DeserializeObject<ObserverCollectionModel>(data);
            RemoveAll();
            if (observerCollectionModel != null)
            {
                foreach (var observe in observerCollectionModel.Observers)
                {
                    Add(observe);
                }
            }
        }

        private string serialize(object data)
        {
            JsonSerializer serializer = new JsonSerializer();
            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, data);
            return textWriter.ToString();
        }
    }
}