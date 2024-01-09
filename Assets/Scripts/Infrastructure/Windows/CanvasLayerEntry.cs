using System;
using Infrastructure.Windows.Enums;
using UnityEngine;

namespace Infrastructure.Windows
{
    [Serializable]
    public class CanvasLayerEntry
    {
        public Layer Layer;
        public Canvas Canvas;
    }
}