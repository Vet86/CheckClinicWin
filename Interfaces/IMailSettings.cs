namespace CheckClinic.Interfaces
{
    public interface IMailSettings
    {
        string MailSender { get; }
        string PasswordSender { get; }
        string NameSender { get; }
        string Smtp { get; }
        int Port { get; }
    }
}
