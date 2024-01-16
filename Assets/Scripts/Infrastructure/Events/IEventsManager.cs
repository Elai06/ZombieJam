namespace Infrastructure.Events
{
    public interface IEventsManager
    {
        void Initialize();
        void SendEventDay(string eventName, string parameters = "");
        void SendEventWithLevelDay(string eventName, string parameters = "");
    }
}