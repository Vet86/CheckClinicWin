namespace CheckClinic.Interfaces
{
    public interface IMailSettings
    {
        string MailSender { get; }
        string PasswordSender { get; }
        string NameSender { get; }
        string SendEmail(string content);
    }
}
