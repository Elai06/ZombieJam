using System;
using Infrastructure.Windows.Enums;
using MVVMLibrary.Enums;
using UnityEngine;

namespace SirGames.Scripts.Windows
{
    [Serializable]
    public class CanvasLayerEntry
    {
        public Layer Layer;
        public Canvas Canvas;
    }
}