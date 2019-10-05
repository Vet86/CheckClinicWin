using RestSharp;
using System.Windows;

namespace CheckClinicUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM _mainVM = new MainVM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainVM;
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            ResponseModel responseModel = (ResponseModel)e.AddedItems[0];
            _mainVM.Speciality.Init(StaticData.ClinicId.Clinic62, responseModel.Id);
        }
    }
}
