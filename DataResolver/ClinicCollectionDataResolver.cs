using CheckClinic.Interfaces;
using RestSharp;

namespace CheckClinicDataResolver
{
    public class ClinicCollectionDataResolver : IClinicCollectionDataResolver
    {
        public string RequestProcess(string districtId)
        {
            RestClient client = new RestClient("https://beta.gorzdrav.spb.ru/api/clinic_list/");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://beta.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "beta.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("district_form-district_id", districtId, ParameterType.GetOrPost);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
