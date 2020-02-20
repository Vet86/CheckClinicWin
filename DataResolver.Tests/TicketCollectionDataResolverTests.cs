using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataResolver.Tests
{
    [TestClass]
    public class TicketCollectionDataResolverTests
    {
        [TestMethod]
        public void RequestProcessTest()
        {
            var ticketCollectionDataResolver = new TicketCollectionDataResolver();
            var html = ticketCollectionDataResolver.RequestProcess("255", "д62.51");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
        }
    }
}
