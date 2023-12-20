using System;
using _Project.Scripts.Infrastructure.PersistenceProgress;

namespace Infrastructure.PersistenceProgress
{
    public interface IProgressService
    {
        PlayerProgress PlayerProgress { get; }
        bool IsLoaded { get; }
        event Action OnLoaded; 
        void InitializeProgress(PlayerProgress playerProgress);
    }
}