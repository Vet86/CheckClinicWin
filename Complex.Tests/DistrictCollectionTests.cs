using Autofac;
using CheckClinic.Complex.Tests;
using CheckClinic.DataParser;
using CheckClinic.Interfaces;
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
            var settings = ContainerHolder.Container.Resolve<IRequestSettings>();
            var districtCollectionDataResolver = new DistrictCollectionDataResolver(settings);
            var html = districtCollectionDataResolver.RequestProcess();
            DistrictCollectionParser dataParser = new DistrictCollectionParser();
            var data = dataParser.ParseDistricts(html);
            Assert.AreEqual(18, data.Count);
        }
    }
}
