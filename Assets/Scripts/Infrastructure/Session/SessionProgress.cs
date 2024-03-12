using System;

namespace Infrastructure.Session
{
    [Serializable]
    public class SessionProgress
    {
        public int SessionNumber;
        public int SessionsDuration;
    }
}