using Autofac;
using CheckClinic.DataResolver;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataResolver.Tests
{
    [TestClass]
    public class DoctorCollectionDataResolverTests
    {
        [TestMethod]
        public void RequestProcessTest()
        {
            var doctorCollectionDataResolver = new DoctorCollectionDataResolver();
            var html = doctorCollectionDataResolver.RequestProcess("591", "2");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
