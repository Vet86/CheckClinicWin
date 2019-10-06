using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class ClinicVM : NotifyPropertyChangedBase
    {
        private ClinicModel _model;
        private readonly string _jsonFile;
        private ClinicId _clinicId;

        public ClinicModel Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                    return;

                _model = value;
                FirePropertyChange(nameof(Model));
            }
        }

        public ClinicVM(string jsonFile, ClinicId clinicId)
        {
            _jsonFile = jsonFile;
            _clinicId = clinicId;
        }

        internal void Recalc(bool full = false)
        {
            var file = string.Format(_jsonFile, (int)_clinicId);
            if (!File.Exists(file))
                return;

            var content = File.ReadAllText(file);
            var newModel = JsonConvert.DeserializeObject<ClinicModel>(content);
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
    }
}
