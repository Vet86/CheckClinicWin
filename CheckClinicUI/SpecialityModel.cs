using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CheckClinicUI
{
    public class SpecialityModel
    {
        private Dictionary<string, ResponseDoctorModel> _idToResponseModel = new Dictionary<string, ResponseDoctorModel>();

        private IList<ResponseDoctorModel> _responseModels;

        public void SetId(int id)
        {
            Id = id;
        }

        [JsonProperty("response")]
        public IList<ResponseDoctorModel> ResponseModels
        {
            get
            {
                return _responseModels;
            }
            set
            {
                _responseModels = value;
                _idToResponseModel.Clear();
                foreach (var response in ResponseModels)
                {
                    _idToResponseModel.Add(response.Id, response);
                }
            }
        }

        public int Id { get; set; }

        internal bool UpdateTickets(SpecialityModel newModel)
        {
            bool res = true;
            try
            {
                foreach (var response in newModel.ResponseModels)
                {
                    _idToResponseModel[response.Id].FreeTickets = response.FreeTickets;
                }
            }
            catch
            {
                res = false;
            }
            return res;
        }
    }

    public class ResponseDoctorModel : NotifyPropertyChangedBase
    {
        private int _freeTickets;

        private bool _subscribe;

        [JsonProperty("IdDoc")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string DoctorName { get; set; }

        [JsonProperty("CountFreeTicket")]
        public int FreeTickets
        {
            get
            {
                return _freeTickets;
            }
            set
            {
                if (_freeTickets == value)
                    return;

                _freeTickets = value;
                FirePropertyChange(nameof(FreeTickets));
            }
        }

        public bool Subscribe
        {
            get => _subscribe;
            set
            {
                if (_subscribe == value)
                    return;

                _subscribe = value;
                FirePropertyChange(nameof(Subscribe));
            }
        }
    }
}
