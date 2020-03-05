using System.Collections.ObjectModel;
using System.Net.Mail;

namespace CheckClinic.UI
{
    public class Receiver
    {
        public string Name { get; set; }
    }

    class SettingsVM
    {
        public SettingsVM()
        {
            Receivers.CollectionChanged += onReceiversCollectionChanged;
        }

        private void onReceiversCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach(Receiver item in e.NewItems)
            {

            }
        }

        public ObservableCollection<Receiver> Receivers { get; set; } = new ObservableCollection<Receiver>();
    }
}
