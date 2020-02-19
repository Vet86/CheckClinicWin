using Autofac;
using CheckClinic.DataParser;
using CheckClinic.DataResolver;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Complex.Tests
{
    [TestClass]
    public class DistrictCollectionTests
    {
        [TestMethod]
        public void CountTest()
        {
            var settings = ContainerRegister.Container.Resolve<IRequestSettings>();
            var districtCollectionDataResolver = new DistrictCollectionDataResolver();
            var html = districtCollectionDataResolver.RequestProcess(settings);
            DistrictCollectionParser dataParser = new DistrictCollectionParser();
            var data = dataParser.ParseDistricts(html);
            Assert.AreEqual(18, data.Count);
        }
    }
}
