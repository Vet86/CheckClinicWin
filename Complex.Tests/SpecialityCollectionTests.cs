using CheckClinic.DataParser;
using CheckClinicDataResolver;
using NUnit.Framework;

namespace Complex.Tests
{
    public class SpecialityCollectionTests
    {
        /*[TestCase("1",22)]
        [TestCase("18",20)]*/
        public void RequestSpecialityTest()
        {
            var specialityCollectionJson = new SpecialityCollectionDataResolver().RequestProcess("591");
            var specialityCollectionModel = new SpecialityCollectionParser().ParseSpecialities(specialityCollectionJson);
            Assert.AreEqual(9, specialityCollectionModel.Count);
        }
    }
}
