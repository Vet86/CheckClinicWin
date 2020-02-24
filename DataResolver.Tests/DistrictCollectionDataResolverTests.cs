using Autofac;
using CheckClinic.Complex.Tests;
using CheckClinic.Interfaces;
using CheckClinicDataResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataResolver.Tests
{
    [TestClass]
    public class DistrictCollectionDataResolverTests
    {
        [DataTestMethod]
        [DataRow("<li data-id=\"1\" onclick=\"\">Адмиралтейский</li>")]
        [DataRow("<li data-id=\"2\" onclick=\"\">Василеостровский</li>")]
        [DataRow("<li data-id=\"3\" onclick=\"\">Выборгский</li>")]
        [DataRow("<li data-id=\"4\" onclick=\"\">Калининский</li>")]
        [DataRow("<li data-id=\"5\" onclick=\"\">Кировский</li>")]
        [DataRow("<li data-id=\"6\" onclick=\"\">Колпинский</li>")]
        [DataRow("<li data-id=\"7\" onclick=\"\">Красногвардейский</li>")]
        [DataRow("<li data-id=\"8\" onclick=\"\">Красносельский</li>")]
        [DataRow("<li data-id=\"9\" onclick=\"\">Кронштадтский</li>")]
        [DataRow("<li data-id=\"10\" onclick=\"\">Курортный</li>")]
        [DataRow("<li data-id=\"11\" onclick=\"\">Московский</li>")]
        [DataRow("<li data-id=\"12\" onclick=\"\">Невский</li>")]
        [DataRow("<li data-id=\"13\" onclick=\"\">Петроградский</li>")]
        [DataRow("<li data-id=\"14\" onclick=\"\">Петродворцовый</li>")]
        [DataRow("<li data-id=\"15\" onclick=\"\">Приморский</li>")]
        [DataRow("<li data-id=\"16\" onclick=\"\">Пушкинский</li>")]
        [DataRow("<li data-id=\"17\" onclick=\"\">Фрунзенский</li>")]
        [DataRow("<li data-id=\"18\" onclick=\"\">Центральный</li>")]
        public void RequestProcessTest(string row)
        {
            var settings = ContainerHolder.Container.Resolve<IRequestSettings>();
            var districtCollectionJsonWriter = new DistrictCollectionDataResolver(settings);
            var html = districtCollectionJsonWriter.RequestProcess();
            Assert.IsTrue(html.Contains(row));
        }
    }
}
