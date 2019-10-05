using System.ComponentModel;

namespace CheckClinicUI
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateAll()
        {
            FirePropertyChange(string.Empty);
        }
    }
}
