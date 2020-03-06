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
        void RemoveAll();
        IReadOnlyList<IObserveData> GetObserves();

        void AddMailReceiver(MailAddress mailReceiver);
        void ClearReceivers();
        IReadOnlyList<MailAddress> GetMailReceivers();

        void AddListener(IDetectListener detectListener);

        void SetUpdateInterval(TimeSpan interval);
        TimeSpan GetUpdateInterval();

        string ExportSettings();
        void ImportSettings(string settings);

        string ExportData();
        void ImportData(string data);
    }
}
