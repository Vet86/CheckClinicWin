using System;
using System.ComponentModel;
using Newtonsoft.Json;
using RestSharp;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class ClinicVM : INotifyPropertyChanged
    {
        RestClient _client = new RestClient("https://www.gorzdrav.spb.ru/api/check_clinic/");
        RestRequest _request;

        private ClinicModel _model;
        public ClinicModel Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                    return;

                _model = value;
                firePropertyChange(nameof(Model));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ClinicVM(ClinicId clinicId)
        {
            _request = new RestRequest(Method.POST);
            _request.AddHeader("Referrer", "https://www.gorzdrav.spb.ru/signup/free/?");
            _request.AddHeader("Host", "www.gorzdrav.spb.ru");
            _request.AddHeader("X-Requested-With", "XMLHttpRequest");
            _request.AddParameter("clinic_form-clinic_id", ((int)clinicId).ToString(), ParameterType.GetOrPost);
        }

        internal void Recalc(bool full = false)
        {
            if (_request == null)
                return;

            IRestResponse response = _client.Execute(_request);
            string json = response.Content;
            var newModel = JsonConvert.DeserializeObject<ClinicModel>(json);
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
