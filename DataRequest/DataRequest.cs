using System;
using System.Collections.Generic;
using System.Threading;
using CheckClinic.Interfaces;

namespace CheckClinic.DataRequest
{
    public class DataRequest : IDataRequest
    {
        private List<IObserveData> _observes = new List<IObserveData>();
        private Timer _timer;
        private ITicketCollectionDataResolver _ticketCollectionDataResolver;
        private ITicketCollectionParser _ticketCollectionParser;
        private TimeSpan _interval;

        public DataRequest(ITicketCollectionDataResolver ticketCollectionDataResolver, ITicketCollectionParser ticketCollectionParser)
        {
            _ticketCollectionDataResolver = ticketCollectionDataResolver;
            _ticketCollectionParser = ticketCollectionParser;
        }

        public Action<IObserveData, IReadOnlyList<ITicket>> NewDataReceived { get; set; }

        public void Add(IObserveData observeData)
        {
            _observes.Add(observeData);
        }

        public IReadOnlyList<ITicket> Receive(IObserveData observeData)
        {
            return receive(observeData);
        }

        public void Remove(IObserveData observeData)
        {
            _observes.Remove(observeData);
        }

        public void SetInterval(TimeSpan timeSpan)
        {
            if (_timer == null)
            {
                _timer = new Timer(onTimerTick, null, 0, (int)timeSpan.TotalMilliseconds);
            }
            else
            {
                System.Diagnostics.Trace.Assert(_timer.Change(0, (int)timeSpan.TotalMilliseconds));
            }
            _interval = timeSpan;
        }

        public TimeSpan GetInterval()
        {
            return _interval;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void onTimerTick(object state)
        {
            foreach (var obs in _observes)
            {
                try
                {
                    NewDataReceived(obs, receive(obs));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private IReadOnlyList<ITicket> receive(IObserveData observeData)
        {
            var ticketCollectionJson = _ticketCollectionDataResolver.RequestProcess(observeData.ClinicId, observeData.DoctorId);
            return _ticketCollectionParser.Parse(ticketCollectionJson);
        }
    }
}