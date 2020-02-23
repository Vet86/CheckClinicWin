using System.Collections.Generic;

namespace CheckClinic.Interfaces
{
    public interface IMailNotifier
    {
        void AddReceiver(string receiver);
        void AddReceivers(IEnumerable<string> receivers);
        void Send(IEnumerable<ITicket> tickets);
    }
}
