using Autofac;
using CheckClinic.Complex.Tests;
using CheckClinic.DataParser;
using CheckClinic.Interfaces;
using CheckClinicDataResolver;
using NUnit.Framework;

namespace Complex.Tests
{
    public class DistrictCollectionTests
    {
        [Test]
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
