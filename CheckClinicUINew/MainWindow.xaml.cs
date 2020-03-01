using CheckClinic.Interfaces;
using CheckClinic.UI;
using System.Windows;
using System.Windows.Controls;

namespace CheckClinicUINew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }

        private void onNotifyButton(object sender, RoutedEventArgs e)
        {
            var doctor = (IDoctor)((Button)sender).DataContext;
            ((MainVM)DataContext).AddOservable(doctor);
        }
    }
}
