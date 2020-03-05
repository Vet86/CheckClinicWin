using CheckClinic.Interfaces;
using CheckClinicUI.Base;
using System.Collections.Generic;

namespace CheckClinic.UI
{
    public class ObserverDataVM : ViewModelBase
    {
        private IReadOnlyList<ITicket> _tickets;

        public IObserveData ObserveData { get; set; }
        public IReadOnlyList<ITicket> Tickets
        {
            get
            {
                return _tickets;
            }
            set
            {
                _tickets = value;
                FirePropertyChange(nameof(Tickets));
            }
        }
    }
}
