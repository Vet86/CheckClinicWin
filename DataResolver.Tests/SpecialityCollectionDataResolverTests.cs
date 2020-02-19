using Autofac;
using CheckClinic.DataResolver;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataResolver.Tests
{
    [TestClass]
    public class SpecialityCollectionDataResolverTests
    {
        [TestMethod]
        public void RequestProcessTest()
        {
            var specialityCollectionDataResolver = new SpecialityCollectionDataResolver();
            var html = specialityCollectionDataResolver.RequestProcess("591");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
