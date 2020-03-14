using CheckClinicDataResolver;
using NUnit.Framework;

namespace DataResolver.Tests
{
    public class TicketCollectionDataResolverTests
    {
        [Test]
        public void RequestProcessTest()
        {
            var ticketCollectionDataResolver = new TicketCollectionDataResolver();
            var html = ticketCollectionDataResolver.RequestProcess("255", "д62.51");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
