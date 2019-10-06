﻿using System;
using System.Collections.Generic;

namespace CheckClinicUI
{
    class TicketIncreaseNotifier
    {
        private readonly Dictionary<string, int> _idToTickets = new Dictionary<string, int>();
        public Action<ResponseDoctorModel> TicketIncreaseHandler {get;set;}
        public TicketIncreaseNotifier(SpecialityVM specialityVM)
        {
            specialityVM.SubscribeChangeHandler += OnSubscribeChanged;
            specialityVM.TicketChangeHandler += OnTicketsChanged;
        }

        private void OnTicketsChanged(ResponseDoctorModel responseDoctorModel)
        {
            if (!_idToTickets.ContainsKey(responseDoctorModel.Id))
                return;

            if (responseDoctorModel.FreeTickets > _idToTickets[responseDoctorModel.Id])
            {
                TicketIncreaseHandler?.Invoke(responseDoctorModel);
            }
            else
            {
                _idToTickets[responseDoctorModel.Id] = responseDoctorModel.FreeTickets;
            }
        }

        private void OnSubscribeChanged(ResponseDoctorModel responseDoctorModel)
        {
            if (responseDoctorModel.Subscribe)
            {
                _idToTickets[responseDoctorModel.Id] = responseDoctorModel.FreeTickets;
            }
            else
            {
                if (_idToTickets.ContainsKey(responseDoctorModel.Id))
                    _idToTickets.Remove(responseDoctorModel.Id);
            }
        }
    }
}