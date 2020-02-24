namespace CheckClinic.Interfaces
{
    public interface IDoctor
    {
        string Id { get; }

        string DoctorName { get; }

        int FreeTickets { get; }
    }
}
