using CheckClinic.Interfaces;
using RestSharp;

namespace CheckClinicDataResolver
{
    public class DistrictCollectionDataResolver : IDistrictCollectionDataResolver
    {
        private readonly IRequestSettings _requestSettings;

        public DistrictCollectionDataResolver(IRequestSettings requestSettings)
        {
            _requestSettings = requestSettings;
        }

        public string RequestProcess()
        {
            RestClient client = new RestClient(_requestSettings.Site);
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
