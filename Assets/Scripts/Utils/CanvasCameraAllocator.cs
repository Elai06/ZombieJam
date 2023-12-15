using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class CanvasCameraAllocator : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private void Start()
        {
            if (Camera.main != null && _canvas.worldCamera == null)
            {
                _canvas.worldCamera = Camera.main;
            }
        }
    }
}