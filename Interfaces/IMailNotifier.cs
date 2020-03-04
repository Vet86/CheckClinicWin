using System.Collections.Generic;
using System.Net.Mail;

namespace CheckClinic.Interfaces
{
    public interface IMailNotifier
    {
        void AddReceiver(MailAddress receiver);
        void AddReceivers(IEnumerable<MailAddress> receivers);
        IReadOnlyList<MailAddress> GetReceivers();
        void ClearAllReceivers();
        void Send(string title, string content);
    }
}
