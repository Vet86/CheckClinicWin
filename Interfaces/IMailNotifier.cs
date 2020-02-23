using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IMailNotifier
    {
        void AddReceiver(string receiver);
        void AddReceivers(IEnumerable<string> receivers);
        void ClearAllReceivers();
        void Send(string title, string content);
    }
}
