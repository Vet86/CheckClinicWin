using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CheckClinicUI.StaticData;

namespace CheckClinicUI
{
    class SpecialityVM : NotifyPropertyChangedBase
    {
        private SpecialityModel _selectModel;
        private readonly string _jsonFile;
        private ClinicId _clinicId;
        private int? _specialitiId;

        public HashSet<int> NotifySpecialities { get; private set; } = new HashSet<int>();
        public List<SpecialityModel> NotifyModels { get; private set; } = new List<SpecialityModel>();
        public Action<ResponseDoctorModel> SubscribeChangeHandler { get; set; }
        public Action<ResponseDoctorModel> TicketChangeHandler { get; set; }

        public SpecialityVM(string jsonFile)
        {
            _jsonFile = jsonFile;
        }

        public SpecialityModel SelectModel
        {
            get { return _selectModel; }
            set
            {
                _selectModel = value;
                FirePropertyChange(nameof(SelectModel));
            }
        }

        public List<SpecialityModel> Models { get; set; } = new List<SpecialityModel>();

        public void Init(string jsonFile, ClinicId clinicId, int specialityId)
        {
            _clinicId = clinicId;
            _specialitiId = specialityId;

            var model = Models.FirstOrDefault(x => x.Id == specialityId);
            SelectModel = model;
            regenerateNotifiers();
        }

        internal void Recalc()
        {
            if (_specialitiId == null)
                return;

            var model = GetData(_specialitiId.Value);
            if (model != null && SelectModel == null)
            {
                Models.Add(model);
                SelectModel = model;
                regenerateNotifiers();
                foreach (var m in model.ResponseModels)
                {
                    m.PropertyChanged += onResponseDoctorPropertyChanged;
                }
            }
            foreach (var notifyModel in NotifyModels)
            {
                var res = notifyModel.UpdateTickets(GetData(notifyModel.Id));
            }
        }

        private void onResponseDoctorPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var responseModel = sender as ResponseDoctorModel;
            if (responseModel == null)
                return;

            if (e.PropertyName == nameof(responseModel.Subscribe))
            {
                SubscribeChangeHandler?.Invoke(responseModel);
                regenerateNotifiers();
            }

            if (e.PropertyName == nameof(responseModel.FreeTickets))
                TicketChangeHandler?.Invoke(responseModel);
        }

        private void regenerateNotifiers()
        {
            NotifySpecialities.Clear();
            NotifyModels.Clear();
            foreach (var specModel in Models)
            {
                if (specModel.ResponseModels.Any(x => x.Subscribe))
                {
                    NotifySpecialities.Add(specModel.Id);
                    NotifyModels.Add(specModel);
                }
            }
            if (_specialitiId != null)
                NotifySpecialities.Add(_specialitiId.Value);
        }

        private SpecialityModel GetData(int specialitiId)
        {
            var file = string.Format(_jsonFile, (int)_clinicId, specialitiId);
            if (!File.Exists(file))
                return null;

            var content = File.ReadAllText(file);
            var specialityModel = JsonConvert.DeserializeObject<SpecialityModel>(content);
            specialityModel?.SetId(specialitiId);
            return specialityModel;
        }
    }
}
