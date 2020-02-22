using System;

namespace CheckClinic.Model
{
    public interface IDataRequest
    {
        void SetInterval(TimeSpan timeSpan);
        void Add(IObserveData observeData);
        void Remove(IObserveData observeData);
        void Start();
        void Stop();
        Action<IObserveData, TicketCollection> NewDataReceived { get; set; }
    }
}
