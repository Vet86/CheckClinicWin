using System;
using System.Collections.Generic;
using System.Windows.Threading;
using CheckClinic.Model;

namespace CheckClinic.DataRequest
{
    public class DataRequest : IDataRequest
    {
        private List<IObserveData> _observes = new List<IObserveData>();
        private DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(1) };
        private ITicketCollectionDataResolver _ticketCollectionDataResolver;
        private ITicketCollectionParser _ticketCollectionParser;

        public DataRequest(ITicketCollectionDataResolver ticketCollectionDataResolver, ITicketCollectionParser ticketCollectionParser)
        {
            _ticketCollectionDataResolver = ticketCollectionDataResolver;
            _ticketCollectionParser = ticketCollectionParser;
            _timer.Tick += ontTimerTick;
        }

        public Action NewDataReceive { get; private set; }

        public void Add(IObserveData observeData)
        {
            _observes.Add(observeData);
        }

        public void Remove(IObserveData observeData)
        {
            _observes.Remove(observeData);
        }

        public void SetInterval(TimeSpan timeSpan)
        {
            _timer.Interval = timeSpan;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void ontTimerTick(object sender, EventArgs e)
        {
            foreach (var obs in _observes)
            {
                var ticketCollectionJson = _ticketCollectionDataResolver.RequestProcess("255", "д62.51");
                var ticketCollectionModel = _ticketCollectionParser.Parse(ticketCollectionJson);
            }
        }
    }
}
