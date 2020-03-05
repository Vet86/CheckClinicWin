using CheckClinic.Interfaces;
using System.Collections.Generic;
using PropertyChanged;

namespace CheckClinic.UI
{
    [AddINotifyPropertyChangedInterface]
    public class ObserverDataVM
    {

        public IObserveData ObserveData { get; set; }
        public IReadOnlyList<ITicket> Tickets { get; set; }
    }
}
