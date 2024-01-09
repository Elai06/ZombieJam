using System.Collections.Generic;
using Infrastructure.StaticData;
using Infrastructure.Windows.Enums;
using MVVMLibrary.Enums;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;

namespace Infrastructure.Windows
{
    public class WindowFactory : IWindowFactory
    {
        private readonly GameStaticData _gameStaticData;

        private readonly Dictionary<Layer, Canvas> _layers;
        private readonly IInstantiateSpawner _inject;

        public WindowFactory(LayersContainer layersContainer, IInstantiateSpawner inject, GameStaticData gameStaticData)
        {
            _gameStaticData = gameStaticData;
            _inject = inject;
            _layers = new Dictionary<Layer, Canvas>();
            foreach (var canvasLayerEntry in layersContainer.CanvasLayerEntry)
            {
                _layers.Add(canvasLayerEntry.Layer, canvasLayerEntry.Canvas);
            }
        }

        public Window Create(WindowType windowType)
        {
            var windowStaticData = _gameStaticData.GetWindowData(windowType);
            var parent = _layers[windowStaticData.Layer].transform;
            var prefab = windowStaticData;
            var windowInstance = _inject.Instantiate<Window>(prefab.Window, parent);
            windowInstance.SetLayer(windowStaticData.Layer);
            return windowInstance;
        }
    }
}