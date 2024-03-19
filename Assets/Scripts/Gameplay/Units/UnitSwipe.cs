using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Enums;
using Infrastructure.Input;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitSwipe : MonoBehaviour, ISwipeObject
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private Material _outlineMaterial;
        [SerializeField] private Material _material;
        [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers;
        [SerializeField] private List<MeshRenderer> _meshRenderers;

        public ESwipeDirection SwipeDirection { get; set; }

        public void Swipe(ESwipeSide swipe)
        {
            _unit.Swipe(swipe);
        }

        public void SetSwipeObject()
        {
            SwitchMaterial(_outlineMaterial);
        }

        public async void ResetSwipeObject()
        {
            await Task.Delay(250);
            SwitchMaterial(_material);
        }

        private void SwitchMaterial(Material material)
        {
            if (_skinnedMeshRenderers.Count > 0)
            {
                foreach (var mesh in _skinnedMeshRenderers)
                {
                    mesh.material = material;
                }
            }
            else
            {
                foreach (var mesh in _meshRenderers)
                {
                    mesh.material = material;
                }
            }
        }
    }
}