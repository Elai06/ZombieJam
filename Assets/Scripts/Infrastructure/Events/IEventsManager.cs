using Gameplay.Windows.Gameplay;

namespace Infrastructure.Events
{
    public interface IEventsManager
    {
        void Initialize(IGameplayModel gameplayModel);
        void SendEventDay(string eventName, string parameters = "");
        void SendEvent(string eventName, string parameters = "");
    }
}