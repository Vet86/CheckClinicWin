namespace CheckClinic.Interfaces
{
    public interface ISpeciality
    {
        string Id { get; }

        string DoctorName { get; }

        int FreeTickets { get; }
    }
}
