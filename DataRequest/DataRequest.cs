using System;
using System.Collections.Generic;
using System.Threading;
using CheckClinic.Model;

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

        public Action<IObserveData, TicketCollection> NewDataReceived { get; set; }

        public void Add(IObserveData observeData)
        {
            _observes.Add(observeData);
        }

        public TicketCollection Receive(IObserveData observeData)
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

        private TicketCollection receive(IObserveData observeData)
        {
            var ticketCollectionJson = _ticketCollectionDataResolver.RequestProcess(observeData.ClinicId, observeData.DoctorId);
            return _ticketCollectionParser.Parse(ticketCollectionJson);
        }
    }
}
