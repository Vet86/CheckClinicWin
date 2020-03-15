using Autofac;
using CheckClinic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace CheckClinic.Bot
{
    class TelegramProcessor
    {
        enum Level
        {
            Start,
            SelectDistric,
            SelectClinic,
            SelectSpec,
            SelectDoctor,
            SelectTicket
        }

        class DataInfo
        {
            public IDistrict District { get; set; }
            public IClinic Clinic { get; set; }
            public ISpeciality Speciality { get; set; }
            public IDoctor Doctor { get; set; }
        }

        class ChatInfo
        {
            public Level ChatLevel { get; set; }
            public DataInfo Info { get; set; } = new DataInfo();

            public ChatInfo(Level chatLevel)
            {
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
        }

        #region private fields 
        TelegramBotClient _botClient = new TelegramBotClient("");

        private readonly IDetector _detector = ContainerHolder.Container.Resolve<IDetector>();
        private readonly Repository _repository = new Repository();

        //private string _districts = null;
        private List<KeyValuePair<IDistrict, KeyboardButton>> _districtToKey = new List<KeyValuePair<IDistrict, KeyboardButton>>();
        private Dictionary<string, List<KeyValuePair<IClinic, KeyboardButton>>> _clinicToKey = new Dictionary<string, List<KeyValuePair<IClinic, KeyboardButton>>>();
        private Dictionary<long, ChatInfo> _chatsInfo = new Dictionary<long, ChatInfo>();
        #endregion

        public TelegramProcessor()
        {
            _botClient.OnMessage += onMessage;
            _botClient.OnCallbackQuery += onCallbackQuery;
            _botClient.StartReceiving();
        }

        ~TelegramProcessor()
        {
            _botClient.StopReceiving();
        }

        private void onCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onMessage(object sender, MessageEventArgs messageEventArgs)
        {
            var chatId = messageEventArgs.Message.Chat.Id;

            if (!_chatsInfo.ContainsKey(chatId) || messageEventArgs.Message.Text == @"/start")
            {
                _chatsInfo[chatId] = new ChatInfo(Level.Start);
            }

            ChatInfo chatInfo = _chatsInfo[chatId];
            int i = 0;
            if (chatInfo.ChatLevel != Level.Start)
            {
                if (int.TryParse(messageEventArgs.Message.Text, out i))
                {
                    if (i < 0)
                    {
                        _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                        return;
                    }

                    if (i == 0)
                    {
                        if (chatInfo.ChatLevel >= Level.SelectDoctor && messageEventArgs.Message.Text == "00")
                        {
                            chatInfo.ChatLevel = (Level)Math.Max(0, (int)chatInfo.ChatLevel - 1);
                        }
                        else if (chatInfo.ChatLevel != Level.SelectDistric)
                        {
                            chatInfo.ChatLevel = (Level)Math.Max(0, (int)chatInfo.ChatLevel - 2);
                        }
                        else
                        {
                            _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                            return;
                        }
                    }
                }
                else
                {
                    _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                    return;
                }
            }

            switch (chatInfo.ChatLevel)
            {
                case Level.Start:
                    {
                        _botClient.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        _botClient.SendTextMessageAsync(chatId, getDistrictsMessage());
                        chatInfo.ChatLevel = Level.SelectDistric;
                        chatInfo.Clear();
                        return;
                    }
                case Level.SelectDistric:
                    {
                        var districts = _repository.GetDistricts();
                        if (i > districts.Count)
                        {
                            _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                            return;
                        }

                        IDistrict district;
                        if (i == 0)
                        {
                            district = (IDistrict)chatInfo.Info.District;
                        }
                        else
                        {
                            district = districts[i - 1];
                        }

                        _botClient.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        _botClient.SendTextMessageAsync(chatId, getClinicsMessage(district));
                        chatInfo.ChatLevel = Level.SelectClinic;
                        chatInfo.SetDistrict(district);
                        return;
                    }
                case Level.SelectClinic:
                    {
                        var district = chatInfo.Info.District;
                        if (district == null)
                            return;

                        var clinics = _repository.GetClinics(district);
                        if (i > clinics.Count)
                        {
                            _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                            return;
                        }

                        IClinic clinic;
                        if (i == 0)
                        {
                            clinic = chatInfo.Info.Clinic;
                        }                        
                        else
                        {
                            clinic = clinics[i - 1];
                        }

                        _botClient.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        _botClient.SendTextMessageAsync(chatId, getSpecialitiesMessage(clinic));
                        chatInfo.ChatLevel = Level.SelectSpec;
                        chatInfo.SetClinic(clinic);
                        return;
                    }
                case Level.SelectSpec:
                    {
                        IClinic clinic = chatInfo.Info.Clinic;
                        if (clinic == null)
                            return;

                        var specialities = _repository.GetSpecialities(clinic);
                        if (i > specialities.Count)
                        {
                            _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                            return;
                        }

                        ISpeciality speciality;
                        if (i == 0)
                        {
                            speciality = chatInfo.Info.Speciality;
                        }
                        else
                        {
                            speciality = specialities[i - 1];
                        }

                        _botClient.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        _botClient.SendTextMessageAsync(chatId, getDoctorsMessage(clinic, speciality));
                        chatInfo.ChatLevel = Level.SelectDoctor;
                        chatInfo.SetSpeciality(speciality);
                        return;
                    }
                case Level.SelectDoctor:
                    {
                        IClinic clinic = chatInfo.Info.Clinic;
                        if (clinic == null)
                            return;

                        ISpeciality speciality = chatInfo.Info.Speciality;
                        if (speciality == null)
                            return;

                        var doctors = _repository.GetDoctors(clinic, speciality);
                        if (i > doctors.Count)
                        {
                            _botClient.SendTextMessageAsync(chatId, "Неверная команда");
                            return;
                        }

                        IDoctor doctor;
                        if (i == 0)
                        {
                            doctor = chatInfo.Info.Doctor;
                        }
                        else
                        {
                            doctor = doctors[i - 1];
                        }

                        _botClient.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        _botClient.SendTextMessageAsync(chatId, getTicketsMessage(clinic, doctor));
                        chatInfo.ChatLevel = Level.SelectTicket;
                        chatInfo.SetDoctor(doctor);
                        return;
                    }
            }

            //var district = _districtToKey.FirstOrDefault(x => x.Value.Text == messageEventArgs.Message.Text);
            //if (district != default( KeyValuePair<IDistrict, KeyboardButton>())
            {

            }
        }

        private void doStep()
        {

        }

        private string getDistrictsMessage()
        {
            var stringBuilder = new StringBuilder();
            var districts = _repository.GetDistricts();
            for (int i = 0; i < districts.Count; ++i)
            {
                stringBuilder.AppendLine($"{i + 1}. {districts[i].Name}");
            }

            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("Наберите номер района");
            return stringBuilder.ToString();
        }

        private string getClinicsMessage(IDistrict district)
        {
            var stringBuilder = new StringBuilder();
            var clinics = _repository.GetClinics(district);
            for (int i = 0; i < clinics.Count; ++i)
            {
                stringBuilder.AppendLine($"{i + 1}. {clinics[i].FullName}");
            }

            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("0. Вернуться назад");
            stringBuilder.AppendLine("Наберите номер клиники");
            return stringBuilder.ToString();
        }

        private string getSpecialitiesMessage(IClinic clinic)
        {
            var stringBuilder = new StringBuilder();
            var specialities = _repository.GetSpecialities(clinic);
            for (int i = 0; i < specialities.Count; ++i)
            {
                stringBuilder.AppendLine($"{i + 1}. {specialities[i].DoctorName}");
            }

            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("0. Вернуться назад");
            stringBuilder.AppendLine("Наберите номер специализации");
            return stringBuilder.ToString();
        }

        private string getDoctorsMessage(IClinic clinic, ISpeciality speciality)
        {
            var stringBuilder = new StringBuilder();
            var doctors = _repository.GetDoctors(clinic, speciality);
            for (int i = 0; i < doctors.Count; ++i)
            {
                stringBuilder.AppendLine($"{i + 1}. {doctors[i].DoctorName} номерков {doctors[i].FreeTickets}");
            }

            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("0. Вернуться назад");
            stringBuilder.AppendLine("00. Обновить");
            stringBuilder.AppendLine("Наберите номер доктора");
            return stringBuilder.ToString();
        }

        private string getTicketsMessage(IClinic clinic, IDoctor doctor)
        {
            var stringBuilder = new StringBuilder();
            var tickets = _repository.GetTickets(clinic, doctor);
            for (int i = 0; i < tickets.Count; ++i)
            {
                stringBuilder.AppendLine($"{tickets[i].Time}");
            }

            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("0. Вернуться назад");
            stringBuilder.AppendLine("00. Обновить");
            stringBuilder.AppendLine("Наберите + чтобы начать следить");
            return stringBuilder.ToString();
        }

    }
}
