using RestSharp;
using System.Windows;

namespace CheckClinicUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbClinic.DataContext = new ClinicVM(StaticData.ClinicId.Clinic62).Model;
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            ResponseModel responseModel = (ResponseModel)e.AddedItems[0];
            lbSpec.DataContext = new SpecialityVM(StaticData.ClinicId.Clinic62, responseModel.Id).Model;
        }
    }
}
