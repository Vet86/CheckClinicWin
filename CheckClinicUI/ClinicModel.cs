using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CheckClinicUI
{
    public class ClinicModel : NotifyPropertyChangedBase
    {
        private Dictionary<int, ResponseModel> _idToResponseModel = new Dictionary<int, ResponseModel>();
        [JsonProperty("changes")]
        public string Changes { get; set; }

        [JsonProperty("response")]
        public IList<ResponseModel> ResponseModels { get; set; }

        public void Init()
        {
            _idToResponseModel.Clear();
            foreach (var response in ResponseModels)
            {
                _idToResponseModel.Add(response.Id, response);
            }
        }

        internal bool UpdateTickets(ClinicModel newModel)
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

    public class ResponseModel : NotifyPropertyChangedBase
    {
        private int _freeTickets;

        [JsonProperty("IdSpesiality")]
        public int Id { get; set; }

        [JsonProperty("NameSpesiality")]
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
    }
}
