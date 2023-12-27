using Gameplay.Configs.Region;
using Gameplay.Configs.Sprites;
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
        [SerializeField] private SpritesConfig spritesSpritesConfig;

        public RegionConfig RegionConfig => _regionConfig;

        public SpritesConfig SpritesConfig => spritesSpritesConfig;

        public WindowData GetWindowData(WindowType windowType)
        {
            return _windowsStaticData.GetWindows().Find(x => x.WindowType == windowType);
        }
    }
}