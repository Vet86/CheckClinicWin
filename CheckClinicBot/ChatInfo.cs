using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;

namespace CheckClinic.Bot
{
    class ChatInfo : IDetectListener
    {
        public long ChatId { get; }
        public Level ChatLevel { get; set; }
        public DataInfo Info { get; set; } = new DataInfo();
        public IDetector Detector { get; set; }

        public Action<ChatInfo, IObserveData, IEnumerable<ITicket>> TicketsEventHandler;

        public ChatInfo(long chatId, Level chatLevel)
        {
            ChatId = chatId;
            ChatLevel = chatLevel;
        }

        public void Clear()
        {
            Info.District = null;
            Info.Clinic = null;
            Info.Speciality = null;
            Info.Doctor = null;
        }

        public void SetDistrict(IDistrict district)
        {
            Info.District = district;
            Info.Clinic = null;
            Info.Speciality = null;
            Info.Doctor = null;
        }

        public void SetClinic(IClinic clinic)
        {
            Info.Clinic = clinic;
            Info.Speciality = null;
            Info.Doctor = null;
        }

        public void SetSpeciality(ISpeciality speciality)
        {
            Info.Speciality = speciality;
            Info.Doctor = null;
        }

        public void SetDoctor(IDoctor doctor)
        {
            Info.Doctor = doctor;
        }

        public void NewTicketsAdded(IObserveData observeData, IEnumerable<ITicket> newTickets)
        {
            TicketsEventHandler?.Invoke(this, observeData, newTickets);
        }
    }
}
