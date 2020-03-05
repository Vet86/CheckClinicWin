using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace CheckClinic.Interfaces
{
    public interface IDetector
    {
        void Add(IObserveData observeData);
        void Add(IObserveData observeData, DateTime dateTime);
        void Remove(IObserveData observeData);
        IReadOnlyList<IObserveData> GetObserves();

        void AddMailReceiver(MailAddress mailReceiver);
        void ClearReceivers();
        IReadOnlyList<MailAddress> GetMailReceivers();

        void AddListener(IDetectListener detectListener);
    }
}
