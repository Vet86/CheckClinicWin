using CheckClinic.DataParser;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Complex.Tests
{
    [TestClass]
    public class DoctorCollectionTests
    {
        [DataTestMethod]
        /*[DataRow("1",22)]
        [DataRow("18",20)]*/
        public void RequestDoctorsTest()
        {
            var doctorCollectionJson = new DoctorCollectionDataResolver().RequestProcess("591", "2");
            var doctorCollectionModel = new DoctorCollectionParser().Parse(doctorCollectionJson);
            Assert.AreEqual(1, doctorCollectionModel.Doctors.Count);
        }
    }
}
