using Autofac;
using CheckClinic.DataResolver;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataResolver.Tests
{
    [TestClass]
    public class ClinicCollectionDataResolverTests
    {
        [TestMethod]
        public void RequestProcessTest()
        {
            var clinicCollectionDataResolver = new ClinicCollectionDataResolver();
            var html = clinicCollectionDataResolver.RequestProcess("1");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
