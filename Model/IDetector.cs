using System;

namespace CheckClinic.Model
{
    public interface IDetector
    {
        void Add(IObserveData observeData);
        void Add(IObserveData observeData, DateTime dateTime);
        void Remove(IObserveData observeData);
    }
}
