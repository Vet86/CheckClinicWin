using System;

namespace CheckClinic.Interfaces
{
    public interface IDetector
    {
        void Add(IObserveData observeData, string doctorName = null);
        void Add(IObserveData observeData, DateTime dateTime, string doctorName = null);
        void Remove(IObserveData observeData);
        void AddMailReceiver(string mailReceiver);
    }
}
