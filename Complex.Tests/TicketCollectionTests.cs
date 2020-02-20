using CheckClinic.DataParser;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Complex.Tests
{
    [TestClass]
    public class TicketCollectionTests
    {
        [DataTestMethod]
        /*[DataRow("1",22)]
        [DataRow("18",20)]*/
        public void RequestTicketsTest()
        {
            var ticketCollectionJson = new TicketCollectionDataResolver().RequestProcess("255", "д62.51");
            var ticketCollectionModel = new TicketCollectionParser().Parse(ticketCollectionJson);
            //Assert.AreEqual(1, ticketCollectionModel.Doctors.Count);
        }
    }
}
