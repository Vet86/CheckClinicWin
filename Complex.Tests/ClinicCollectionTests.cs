using CheckClinic.DataParser;
using CheckClinicDataResolver;
using NUnit.Framework;

namespace Complex.Tests
{
    public class ClinicCollectionTests
    {
        [TestCase("1",22)]
        [TestCase("2",16)]
        [TestCase("3",38)]
        [TestCase("4",38)]
        [TestCase("5",28)]
        [TestCase("6",19)]
        [TestCase("7",25)]
        [TestCase("8",29)]
        [TestCase("9",5)]
        [TestCase("10",13)]
        [TestCase("11",29)]
        [TestCase("12",30)]
        [TestCase("13",15)]
        [TestCase("14",7)]
        [TestCase("15",51)]
        [TestCase("16",18)]
        [TestCase("17",29)]
        [TestCase("18",20)]
        public void RequestClinicsTest(string id, int expectCount)
        {
            var clinicCollectionJson = new ClinicCollectionDataResolver().RequestProcess(id);
            var clinicCollectionModel = new ClinicCollectionParser().ParseClinics(clinicCollectionJson);
            Assert.AreEqual(expectCount, clinicCollectionModel.Count);
        }
    }
}
