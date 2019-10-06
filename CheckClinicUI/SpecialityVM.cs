using Newtonsoft.Json;
using System.IO;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class SpecialityVM : NotifyPropertyChangedBase
    {
        private SpecialityModel _model;
        private readonly string _jsonFile;
        private ClinicId _clinicId;
        private int _specialitiId;

        public SpecialityVM(string jsonFile)
        {
            _jsonFile = jsonFile;
        }

        public SpecialityModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                FirePropertyChange(nameof(Model));
            }
        }

        public void Init(string jsonFile,ClinicId clinicId, int specialitiId)
        {
            Model = null;
            _clinicId = clinicId;
            _specialitiId = specialitiId;
            Recalc();
        }

        internal void Recalc()
        {
            var file = string.Format(_jsonFile, (int)_clinicId, _specialitiId);
            if (!File.Exists(file))
                return;

            var content = File.ReadAllText(file);
            var newModel = JsonConvert.DeserializeObject<SpecialityModel>(content);
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
