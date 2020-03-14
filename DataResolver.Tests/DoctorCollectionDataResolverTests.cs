using CheckClinicDataResolver;
using NUnit.Framework;

namespace DataResolver.Tests
{
    public class DoctorCollectionDataResolverTests
    {
        [Test]
        public void RequestProcessTest()
        {
            var doctorCollectionDataResolver = new DoctorCollectionDataResolver();
            var html = doctorCollectionDataResolver.RequestProcess("591", "2");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
