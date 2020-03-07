using CheckClinic.DataParser;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Complex.Tests
{
    [TestClass]
    public class ClinicCollectionTests
    {
        [DataTestMethod]
        [DataRow("1",22)]
        [DataRow("2",16)]
        [DataRow("3",38)]
        [DataRow("4",38)]
        [DataRow("5",28)]
        [DataRow("6",19)]
        [DataRow("7",25)]
        [DataRow("8",29)]
        [DataRow("9",5)]
        [DataRow("10",12)]
        [DataRow("11",29)]
        [DataRow("12",29)]
        [DataRow("13",15)]
        [DataRow("14",7)]
        [DataRow("15",51)]
        [DataRow("16",18)]
        [DataRow("17",29)]
        [DataRow("18",20)]
        public void RequestClinicsTest(string id, int expectCount)
        {
            var clinicCollectionJson = new ClinicCollectionDataResolver().RequestProcess(id);
            var clinicCollectionModel = new ClinicCollectionParser().ParseClinics(clinicCollectionJson);
            Assert.AreEqual(expectCount, clinicCollectionModel.Count);
        }
    }
}
