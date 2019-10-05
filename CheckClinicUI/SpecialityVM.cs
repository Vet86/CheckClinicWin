using Newtonsoft.Json;
using RestSharp;
using System;
using System.ComponentModel;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class SpecialityVM : INotifyPropertyChanged
    {
        RestClient _client = new RestClient("https://www.gorzdrav.spb.ru/api/doctor_list/");
        RestRequest _request;

        private SpecialityModel _model;
        public SpecialityModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                firePropertyChange(nameof(Model));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Init(ClinicId clinicId, int specialitiId)
        {
            Model = null;
            _request = new RestRequest(Method.POST);
            _request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            _request.AddHeader("Host", "www.gorzdrav.spb.ru");
            _request.AddHeader("X-Requested-With", "XMLHttpRequest");
            _request.AddParameter("speciality_form-speciality_id", (specialitiId).ToString(), ParameterType.GetOrPost);
            _request.AddParameter("speciality_form-clinic_id", ((int)clinicId).ToString(), ParameterType.GetOrPost);
            Recalc();
        }

        internal void Recalc()
        {
            if (_request == null)
                return;

            IRestResponse response = _client.Execute(_request);
            string json = response.Content;
            var newModel = JsonConvert.DeserializeObject<SpecialityModel>(json);
            if (Model == null || Model.ResponseModels.Count != newModel.ResponseModels.Count)
            {
                Model = newModel;
                Model.Init();
            }
            else
            {
                var res = Model.UpdateTickets(newModel);
                if (!res)
                {
                    Model = newModel;
                    Model.Init();
                }
            }
        }

        private void firePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
