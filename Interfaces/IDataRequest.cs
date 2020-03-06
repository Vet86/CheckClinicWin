using System;
using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IDataRequest
    {
        void SetInterval(TimeSpan timeSpan);
        TimeSpan GetInterval();
        void Add(IObserveData observeData);
        void Remove(IObserveData observeData);
        void Start();
        void Stop();
        Action<IObserveData, IReadOnlyList<ITicket>> NewDataReceived { get; set; }
        IReadOnlyList<ITicket> Receive(IObserveData observeData);
    }
}
