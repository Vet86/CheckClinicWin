using CheckClinicUI.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Windows;

namespace CheckClinic.UI
{
    public class Receiver
    {
        public string Name { get; set; }
    }

    class SettingsVM
    {
        public SettingsVM()
        {
            CopyToClipboardCommand = new RelayCommand(x => Clipboard.SetText(ConfigurationPath));
        }

        public ObservableCollection<Receiver> Receivers { get; set; } = new ObservableCollection<Receiver>();

        public bool Verify()
        {
            bool res = true;
            foreach (Receiver item in Receivers)
            {
                try
                {
                    new MailAddress(item.Name);
                }
                catch
                {
                    MessageBox.Show($"{item.Name} - некорректный почтовый ящик");
                    res = false;
                }
            }
            return res;
        }

        public string ConfigurationPath { get; set; }

        public int Interval { get; set; }

        public RelayCommand CopyToClipboardCommand { get; private set; }
    }
}
