using Autofac;
using CheckClinic.Complex.Tests;
using CheckClinic.Interfaces;
using CheckClinicDataResolver;
using NUnit.Framework;

namespace DataResolver.Tests
{
    public class DistrictCollectionDataResolverTests
    {
        [TestCase("<li data-id=\"1\" onclick=\"\">Адмиралтейский</li>")]
        [TestCase("<li data-id=\"2\" onclick=\"\">Василеостровский</li>")]
        [TestCase("<li data-id=\"3\" onclick=\"\">Выборгский</li>")]
        [TestCase("<li data-id=\"4\" onclick=\"\">Калининский</li>")]
        [TestCase("<li data-id=\"5\" onclick=\"\">Кировский</li>")]
        [TestCase("<li data-id=\"6\" onclick=\"\">Колпинский</li>")]
        [TestCase("<li data-id=\"7\" onclick=\"\">Красногвардейский</li>")]
        [TestCase("<li data-id=\"8\" onclick=\"\">Красносельский</li>")]
        [TestCase("<li data-id=\"9\" onclick=\"\">Кронштадтский</li>")]
        [TestCase("<li data-id=\"10\" onclick=\"\">Курортный</li>")]
        [TestCase("<li data-id=\"11\" onclick=\"\">Московский</li>")]
        [TestCase("<li data-id=\"12\" onclick=\"\">Невский</li>")]
        [TestCase("<li data-id=\"13\" onclick=\"\">Петроградский</li>")]
        [TestCase("<li data-id=\"14\" onclick=\"\">Петродворцовый</li>")]
        [TestCase("<li data-id=\"15\" onclick=\"\">Приморский</li>")]
        [TestCase("<li data-id=\"16\" onclick=\"\">Пушкинский</li>")]
        [TestCase("<li data-id=\"17\" onclick=\"\">Фрунзенский</li>")]
        [TestCase("<li data-id=\"18\" onclick=\"\">Центральный</li>")]
        public void RequestProcessTest(string row)
        {
            var settings = ContainerHolder.Container.Resolve<IRequestSettings>();
            var districtCollectionJsonWriter = new DistrictCollectionDataResolver(settings);
            var html = districtCollectionJsonWriter.RequestProcess();
            Assert.IsTrue(html.Contains(row));
        }
    }
}
