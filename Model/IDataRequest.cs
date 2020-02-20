using System;

namespace CheckClinic.Model
{
    public interface IDataRequest
    {
        void SetInterval(TimeSpan timeSpan);
        void Add(string doctorId, string s);
        //void
        void Start();
        void Stop();

    }
}
