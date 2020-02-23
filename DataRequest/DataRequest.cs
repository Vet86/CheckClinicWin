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

        public DataRequest(ITicketCollectionDataResolver ticketCollectionDataResolver, ITicketCollectionParser ticketCollectionParser)
        {
            _ticketCollectionDataResolver = ticketCollectionDataResolver;
            _ticketCollectionParser = ticketCollectionParser;
            _timer = new Timer(onTimerTick, null, 0, 10000);
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
            System.Diagnostics.Debug.Assert(_timer.Change(0, (int)timeSpan.TotalMilliseconds));
        }

        public void Start()
        {
            //_timer.
            //ontTimerTick(null, null);
        }

        public void Stop()
        {
            //_timer.Stop();
        }

        private void onTimerTick(object state)
        {
            foreach (var obs in _observes)
            {
                NewDataReceived(obs, receive(obs));
            }
        }

        private IReadOnlyList<ITicket> receive(IObserveData observeData)
        {
            var ticketCollectionJson = _ticketCollectionDataResolver.RequestProcess(observeData.ClinicId, observeData.DoctorId);
            return _ticketCollectionParser.Parse(ticketCollectionJson);
        }
    }
}
