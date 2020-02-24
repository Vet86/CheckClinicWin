using CheckClinic.DataParser;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Complex.Tests
{
    [TestClass]
    public class SpecialityCollectionTests
    {
        [DataTestMethod]
        /*[DataRow("1",22)]
        [DataRow("18",20)]*/
        public void RequestSpecialityTest()
        {
            var specialityCollectionJson = new SpecialityCollectionDataResolver().RequestProcess("591");
            var specialityCollectionModel = new SpecialityCollectionParser().ParseSpecialities(specialityCollectionJson);
            Assert.AreEqual(9, specialityCollectionModel.Count);
        }
    }
}
