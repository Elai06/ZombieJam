﻿using System.Collections.Generic;
using Gameplay.Configs.Zombies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Parking
{
    public class PositionSpawner : MonoBehaviour
    {
        [SerializeField] private List<SpawnPosition> _spawnGrid = new();
        [SerializeField] private Material _cubeMaterial;
        [SerializeField] private GameObject _cubePrefab;
        [SerializeField] private SpawnPosition _spawnPositionPrefab;
        [SerializeField] private int _square = 25;
        [SerializeField] private Transform _startPosition;

        [Button("SpawnGrid")]
        public void CreateSpawnPosition()
        {
            var startPosition = _startPosition.position;
            for (int i = 1; i <= _square; i++)
            {
                var spawnPrefab = Instantiate(_spawnPositionPrefab, startPosition, Quaternion.identity, transform);
                _spawnGrid.Add(spawnPrefab);

                if (spawnPrefab.IsCooperative && spawnPrefab.CooperativePositions.Count == 0)
                {
                    _spawnGrid.Remove(spawnPrefab);
                }

                if (i == 0) continue;

                if (i % 6 == 0)
                {
                    startPosition.x = _startPosition.position.x;
                    startPosition -= new Vector3(0, 0, 1f);
                }
                else
                {
                    startPosition += new Vector3(1f, 0, 0);
                }
            }
        }

        [Button("Clear")]
        private void Clear()
        {
            for (int i = 0; i < _spawnGrid.Count; i++)
            {
                var element = _spawnGrid[i].gameObject;
                DestroyImmediate(element);
            }

            _spawnGrid.Clear();
        }

        [Button("RemoveMissingGridElements")]
        private void RemoveMissingGridElements()
        {
            for (int i = 0; i < _spawnGrid.Count; i++)
            {
                var element = _spawnGrid[i];
                if (element == null)
                {
                    _spawnGrid.Remove(element);
                }
            }
        }

        [Button("SwitchMaterial")]
        private void SpawnCubes()
        {
            for (int i = 0; i < _spawnGrid.Count; i++)
            {
                var spawnPosition = _spawnGrid[i].transform.GetChild(0).GetComponent<MeshRenderer>();

                if (spawnPosition != null)
                {
                    spawnPosition.material = _cubeMaterial;
                }
            }
        }

        public List<SpawnPosition> GetSpawnPositions()
        {
            return _spawnGrid;
        }
    }
}