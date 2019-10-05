using Newtonsoft.Json;
using RestSharp;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class ClinicVM
    {
        public ClinicModel Model { get; private set; }
        public ClinicVM(ClinicId clinicId)
        {
            var client = new RestClient("https://www.gorzdrav.spb.ru/api/check_clinic/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("clinic_form-clinic_id", ((int)clinicId).ToString(), ParameterType.GetOrPost);
            IRestResponse response = client.Execute(request);
            string json = response.Content;
            Model = JsonConvert.DeserializeObject<ClinicModel>(json);
        }
    }
}
