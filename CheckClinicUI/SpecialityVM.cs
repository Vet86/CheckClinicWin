using Newtonsoft.Json;
using RestSharp;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class SpecialityVM
    {
        public SpecialityModel Model { get; private set; }
        public SpecialityVM(ClinicId clinicId, int specialitiId)
        {
            var client = new RestClient("https://www.gorzdrav.spb.ru/api/doctor_list/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            request.AddHeader("Host", "www.gorzdrav.spb.ru");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddParameter("speciality_form-speciality_id", (specialitiId).ToString(), ParameterType.GetOrPost);
            request.AddParameter("speciality_form-clinic_id", ((int)clinicId).ToString(), ParameterType.GetOrPost);
            IRestResponse response = client.Execute(request);
            string json = response.Content;
            Model = JsonConvert.DeserializeObject<SpecialityModel>(json);
        }
    }
}
