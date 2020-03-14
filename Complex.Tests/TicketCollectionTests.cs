using CheckClinic.DataParser;
using CheckClinicDataResolver;

namespace Complex.Tests
{
    public class TicketCollectionTests
    {
        /*[TestCase("1",22)]
        [TestCase("18",20)]*/
        public void RequestTicketsTest()
        {
            var ticketCollectionJson = new TicketCollectionDataResolver().RequestProcess("255", "д62.51");
            var ticketCollectionModel = new TicketCollectionParser().Parse(ticketCollectionJson);
            //Assert.AreEqual(1, ticketCollectionModel.Doctors.Count);
        }
    }
}
