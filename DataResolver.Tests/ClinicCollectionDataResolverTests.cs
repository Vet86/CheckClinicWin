using CheckClinicDataResolver;
using NUnit.Framework;

namespace DataResolver.Tests
{
    public class ClinicCollectionDataResolverTests
    {
        [Test]
        public void RequestProcessTest()
        {
            var clinicCollectionDataResolver = new ClinicCollectionDataResolver();
            var html = clinicCollectionDataResolver.RequestProcess("1");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
