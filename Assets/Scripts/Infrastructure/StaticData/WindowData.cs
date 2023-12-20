using System;
using Infrastructure.Windows;
using MVVMLibrary.Enums;

namespace Infrastructure.StaticData
{
    [Serializable]
    public class WindowData
    {
        public WindowType WindowType;
        public Layer Layer;
        public Window Window;
    }
}