using CheckClinicDataResolver;
using NUnit.Framework;

namespace DataResolver.Tests
{
    public class SpecialityCollectionDataResolverTests
    {
        [Test]
        public void RequestProcessTest()
        {
            var specialityCollectionDataResolver = new SpecialityCollectionDataResolver();
            var html = specialityCollectionDataResolver.RequestProcess("591");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
