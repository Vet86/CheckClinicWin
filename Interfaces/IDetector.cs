using System;

namespace CheckClinic.Interfaces
{
    public interface IDetector
    {
        void Add(IObserveData observeData);
        void Add(IObserveData observeData, DateTime dateTime);
        void Remove(IObserveData observeData);
        void AddMailReceiver(string mailReceiver);
    }
}
