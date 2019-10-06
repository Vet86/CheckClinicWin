﻿using Newtonsoft.Json;
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
        }

        internal void Recalc()
        {
            if (_specialitiId == null)
                return;

            var model = GetData();
            if (model == null)
                return;

            if (SelectModel == null)
            {
                Models.Add(model);
                SelectModel = model;
                foreach(var m in model.ResponseModels)
                {
                    m.PropertyChanged += onResponseDoctorPropertyChanged;
                }
            }
            else
            {
                var res = SelectModel.UpdateTickets(GetData());
                if (!res)
                {
                }
            }
        }

        private void onResponseDoctorPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var responseModel = sender as ResponseDoctorModel;
            if (responseModel == null)
                return;

            if (e.PropertyName == nameof(responseModel.Subscribe))
                SubscribeChangeHandler?.Invoke(responseModel);

            if (e.PropertyName == nameof(responseModel.FreeTickets))
                TicketChangeHandler?.Invoke(responseModel);
        }

        private SpecialityModel GetData()
        {
            if (!_specialitiId.HasValue)
                return null;

            var file = string.Format(_jsonFile, (int)_clinicId, _specialitiId);
            if (!File.Exists(file))
                return null;

            var content = File.ReadAllText(file);
            var specialityModel = JsonConvert.DeserializeObject<SpecialityModel>(content);
            specialityModel?.SetId(_specialitiId.Value);
            return specialityModel;
        }
    }
}
