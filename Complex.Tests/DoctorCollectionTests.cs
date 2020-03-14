using CheckClinic.DataParser;
using CheckClinicDataResolver;
using NUnit.Framework;

namespace Complex.Tests
{
    public class DoctorCollectionTests
    {
        /*[TestCase("1",22)]
        [TestCase("18",20)]*/
        public void RequestDoctorsTest()
        {
            var doctorCollectionJson = new DoctorCollectionDataResolver().RequestProcess("591", "2");
            var doctorCollectionModel = new DoctorCollectionParser().ParseDoctors(doctorCollectionJson);
            Assert.AreEqual(1, doctorCollectionModel.Count);
        }
    }
}
