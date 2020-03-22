using Autofac;
using CheckClinic.Detector;
using CheckClinic.Interfaces;
using CheckClinic.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace CheckClinic.Bot
{
    internal class TelegramProcessor
    {
        #region private fields 
        private TelegramBotClient _botClient;
        private readonly Repository _repository = new Repository();
        private Dictionary<long, ChatInfo> _chatsInfo = new Dictionary<long, ChatInfo>();
        #endregion

        public TelegramProcessor(string token)
        {
            _botClient = new TelegramBotClient(token);
            _botClient.OnMessage += onMessage;
            _botClient.StartReceiving();
        }

        ~TelegramProcessor()
        {
            _botClient.StopReceiving();
        }

        private void onMessage(object sender, MessageEventArgs messageEventArgs)
        {
            var chatId = messageEventArgs.Message.Chat.Id;

            if (!_chatsInfo.ContainsKey(chatId) || messageEventArgs.Message.Text == @"/start")
            {
                _chatsInfo[chatId] = new ChatInfo(chatId, Level.Start);
            }

            ChatInfo chatInfo = _chatsInfo[chatId];
            var commandInfo = new CommandInfo(messageEventArgs.Message.Text);

            doStep(chatInfo, commandInfo);
        }

        private void doStep(ChatInfo chatInfo, CommandInfo commandInfo)
        {
            switch (chatInfo.ChatLevel)
            {
                case Level.Start:
                    {
                        _botClient.SendChatActionAsync(chatInfo.ChatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        _botClient.SendTextMessageAsync(chatInfo.ChatId, getDistrictsMessage());
                        chatInfo.ChatLevel = Level.SelectDistric;
                        chatInfo.Clear();
                        return;
                    }
                case Level.SelectDistric:
                    {
                        doSelectDistrict(chatInfo, commandInfo);
                        return;
                    }
                case Level.SelectClinic:
                    {
                        doSelectClinic(chatInfo, commandInfo);
                        return;
                    }
                case Level.SelectSpec:
                    {
                        doSelectSpec(chatInfo, commandInfo);
                        return;
                    }
                case Level.SelectDoctor:
                    {
                        doSelectDoctor(chatInfo, commandInfo);
                        return;
                    }
                case Level.SelectTicket:
                    {
                        doSelectTicket(chatInfo, commandInfo);
                        return;
                    }
            }
        }

        private void doSelectDistrict(ChatInfo chatInfo, CommandInfo commandInfo)
        {
            IDistrict district;
            switch (commandInfo.Command)
            {
                case Commands.RefreshByCommand:
                    district = chatInfo.Info.District;
                    break;
                case Commands.SelectItem:
                    {
                        var districts = _repository.GetDistricts();
                        if (commandInfo.Index > districts.Count || commandInfo.Index <= 0)
                        {
                            sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                            return;
                        }
                        district = districts[commandInfo.Index - 1];
                        break;
                    }
                default:
                    sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                    return;
            }

            _botClient.SendChatActionAsync(chatInfo.ChatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
            _botClient.SendTextMessageAsync(chatInfo.ChatId, getClinicsMessage(district));
            chatInfo.ChatLevel = Level.SelectClinic;
            chatInfo.SetDistrict(district);
        }

        private void doSelectClinic(ChatInfo chatInfo, CommandInfo commandInfo)
        {
            var district = chatInfo.Info.District;
            if (district == null)
                return;

            IClinic clinic;
            switch (commandInfo.Command)
            {
                case Commands.Back:
                    commandInfo.Command = Commands.RefreshByCommand;
                    chatInfo.ChatLevel = Level.Start;
                    doStep(chatInfo, commandInfo);
                    return;
                case Commands.RefreshByCommand:
                    clinic = chatInfo.Info.Clinic;
                    break;
                case Commands.SelectItem:
                    {
                        var clinics = _repository.GetClinics(district);
                        if (commandInfo.Index > clinics.Count || commandInfo.Index <= 0)
                        {
                            sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                            return;
                        }
                        clinic = clinics[commandInfo.Index - 1];
                        break;
                    }
                default:
                    sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                    return;
            }

            _botClient.SendChatActionAsync(chatInfo.ChatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
            _botClient.SendTextMessageAsync(chatInfo.ChatId, getSpecialitiesMessage(clinic));
            chatInfo.ChatLevel = Level.SelectSpec;
            chatInfo.SetClinic(clinic);
        }

        private void doSelectSpec(ChatInfo chatInfo, CommandInfo commandInfo)
        {
            IClinic clinic = chatInfo.Info.Clinic;
            if (clinic == null)
                return;

            ISpeciality speciality;
            switch (commandInfo.Command)
            {
                case Commands.Back:
                    commandInfo.Command = Commands.RefreshByCommand;
                    chatInfo.ChatLevel = Level.SelectDistric;
                    doStep(chatInfo, commandInfo);
                    return;
                case Commands.RefreshByCommand:
                    speciality = chatInfo.Info.Speciality;
                    break;
                case Commands.SelectItem:
                    {
                        var specialities = _repository.GetSpecialities(clinic);
                        if (commandInfo.Index > specialities.Count || commandInfo.Index <= 0)
                        {
                            sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                            return;
                        }
                        speciality = specialities[commandInfo.Index - 1];
                        break;
                    }
                default:
                    sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                    return;
            }

            _botClient.SendChatActionAsync(chatInfo.ChatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
            _botClient.SendTextMessageAsync(chatInfo.ChatId, getDoctorsMessage(clinic, speciality));
            chatInfo.ChatLevel = Level.SelectDoctor;
            chatInfo.SetSpeciality(speciality);
        }

        private void doSelectDoctor(ChatInfo chatInfo, CommandInfo commandInfo)
        {
            IClinic clinic = chatInfo.Info.Clinic;
            if (clinic == null)
                return;

            ISpeciality speciality = chatInfo.Info.Speciality;
            if (speciality == null)
                return;

            IDoctor doctor;
            switch (commandInfo.Command)
            {
                case Commands.Back:
                    commandInfo.Command = Commands.RefreshByCommand;
                    chatInfo.ChatLevel = Level.SelectClinic;
                    doStep(chatInfo, commandInfo);
                    return;
                case Commands.Refresh:
                    commandInfo.Command = Commands.RefreshByCommand;
                    chatInfo.ChatLevel = Level.SelectSpec;
                    doStep(chatInfo, commandInfo);
                    return;
                case Commands.RefreshByCommand:
                    doctor = chatInfo.Info.Doctor;
                    break;
                case Commands.SelectItem:
                    {
                        var doctors = _repository.GetDoctors(clinic, speciality);
                        if (commandInfo.Index > doctors.Count || commandInfo.Index <= 0)
                        {
                            sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                            return;
                        }
                        doctor = doctors[commandInfo.Index - 1];
                        break;
                    }
                default:
                    sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                    return;
            }

            _botClient.SendChatActionAsync(chatInfo.ChatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
            _botClient.SendTextMessageAsync(chatInfo.ChatId, getTicketsMessage(clinic, doctor));
            chatInfo.ChatLevel = Level.SelectTicket;
            chatInfo.SetDoctor(doctor);
        }

        private void doSelectTicket(ChatInfo chatInfo, CommandInfo commandInfo)
        {
            switch (commandInfo.Command)
            {
                case Commands.Back:
                    commandInfo.Command = Commands.RefreshByCommand;
                    chatInfo.ChatLevel = Level.SelectSpec;
                    doStep(chatInfo, commandInfo);
                    return;
                case Commands.Refresh:
                    commandInfo.Command = Commands.RefreshByCommand;
                    chatInfo.ChatLevel = Level.SelectDoctor;
                    doStep(chatInfo, commandInfo);
                    return;
                case Commands.Subscribe:
                    {
                        if (chatInfo.Detector == null)
                        {
                            chatInfo.Detector = ContainerHolder.Container.Resolve<IDetector>();
                            chatInfo.TicketsEventHandler += onTicketsAdded;
                            chatInfo.Detector.AddListener(chatInfo);
                        }

                        var observe = new ObserveData(chatInfo.Info.Clinic.Id, chatInfo.Info.Doctor.Id, chatInfo.Info.Doctor.DoctorName);
                        if (chatInfo.Detector.Exists(observe))
                        {
                            _botClient.SendTextMessageAsync(chatInfo.ChatId, "Вы уже подписаны на этого врача");
                            return;
                        }
                        chatInfo.Detector.Add(observe);
                        _botClient.SendTextMessageAsync(chatInfo.ChatId, $"Мы начали следить за номерками {chatInfo.Info.Doctor.DoctorName}");
                        break;
                    }
                default:
                    sendErrorCommand(chatInfo.ChatId, commandInfo.OriginalCommand);
                    return;
            }
        }

        private void onTicketsAdded(ChatInfo chatInfo, IObserveData observeData, IEnumerable<ITicket> tickets)
        {
            try
            {
                var mailTextPreparer = new MailTextPreparer(observeData, tickets.ToList(), observeData.DoctorName);
                _botClient.SendTextMessageAsync(chatInfo.ChatId, mailTextPreparer.FullMessage);
            }
            catch
            {

            }
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

        private void sendErrorCommand(long chatId, string command)
        {
            _botClient.SendTextMessageAsync(chatId, "Неверная команда");
        }
    }
}
