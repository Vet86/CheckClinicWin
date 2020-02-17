using Autofac;
using CheckClinic.DataParser;
using CheckClinicDataResolver;
using System.Windows;

namespace CheckClinicUINew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var settings = ContainerRegister.Container.Resolve<IDistrictCollectionRequestSettings>();
            var districtCollectionJsonWriter = new DistrictCollectionDataResolver();
            var html = districtCollectionJsonWriter.RequestProcess(settings);
            DataParser dataParser = new DataParser();
            var data = dataParser.ParseDistricts(html);
        }
    }
}
