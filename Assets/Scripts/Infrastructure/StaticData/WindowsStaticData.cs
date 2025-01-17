using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "WindowsStaticData", menuName = "Configs/WindowsStaticData")]
    public class WindowsStaticData : ScriptableObject
    {
       [SerializeField] private List<WindowData> _WindowsData;

       public List<WindowData> GetWindows()
       {
           return _WindowsData;
       }
    }
}