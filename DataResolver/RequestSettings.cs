using CheckClinic.Interfaces;

namespace CheckClinic.DataResolver
{
    public class RequestSettings : IRequestSettings
    {
        public string Site => "https://beta.gorzdrav.spb.ru/signup/free/?";
    }
}
