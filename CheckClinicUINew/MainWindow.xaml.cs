using Autofac;
using CheckClinic.DataParser;
using CheckClinicDataResolver;
using System.Windows;
using CheckClinic.DataResolver;

namespace CheckClinicUINew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var settings = ContainerRegister.Container.Resolve<IRequestSettings>();
            //var districtCollectionDataResolver = new DistrictCollectionDataResolver();
            //var html = districtCollectionDataResolver.RequestProcess(settings);
            //DistrictCollectionParser dataParser = new DistrictCollectionParser();
            //var data = dataParser.ParseDistricts(html);

            var clinicCollectionJson = new ClinicCollectionDataResolver().RequestProcess("1");
            var clinicCollectionModel = new ClinicCollectionParser().ParseClinics(clinicCollectionJson);
        }
    }
}
