using Gameplay.Configs.Region;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Configs/GameStaticData")]
    public class GameStaticData : ScriptableObjectInstaller
    {
        [SerializeField] private WindowsStaticData _windowsStaticData;
        [SerializeField] private RegionConfig _regionConfig;

        public RegionConfig RegionConfig => _regionConfig;

        public WindowData GetWindowData(WindowType windowType)
        {
            return _windowsStaticData.GetWindows().Find(x => x.WindowType == windowType);
        }
    }
}