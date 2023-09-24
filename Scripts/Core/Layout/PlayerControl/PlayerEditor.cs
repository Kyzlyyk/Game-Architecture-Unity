#if UNITY_EDITOR

using Kyzlyk.Collections;
using Kyzlyk.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Layout.PlayerControl
{
    public sealed class PlayerEditor : Editor
    {
        [SerializeField] private SpawnerType _spawnerType;
        [SerializeField] private int _maxControlledSpawnerCount;
        [SerializeField] private int _maxUncontrolledSpawnerCount;
        [Space]
        [SerializeField] private GameObject _controlledSpawnerPrefab;
        [SerializeField] private GameObject _uncontrolledSpawnerPrefab;
        [SerializeField] private GameObject _mainSpawnerPrefab;

        private SerializablePosition _mainSpawner;
        private List<SerializablePosition> _controlledSpawners;
        private List<SerializablePosition> _uncontrolledSpawners;

        private KeyList<SerializablePosition, GameObject> _spawnerObjects;

        private int SpawnerCapacity => _controlledSpawners.Capacity + _uncontrolledSpawners.Capacity + 1;

        protected override void Init()
        {
            _controlledSpawners = new List<SerializablePosition>(_maxControlledSpawnerCount);
            _uncontrolledSpawners = new List<SerializablePosition>(_maxUncontrolledSpawnerCount);

            _spawnerObjects = new KeyList<SerializablePosition, GameObject>(SpawnerCapacity);
        }

        private void Update()
        {
            if (!IsEditing) return;

            SetSelectorPositionWithMouse();

            if (Input.GetMouseButtonDown(0))
            {
                CreateSpawnersWithSelectorPosition();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (_spawnerType == SpawnerType.Controlled)
                    RemoveSpawnersWithSelectorPosition(_controlledSpawners);   
                
                else if (_spawnerType == SpawnerType.Uncontrolled)
                    RemoveSpawnersWithSelectorPosition(_uncontrolledSpawners);
            }
        }

        private void RemoveSpawnersWithSelectorPosition(List<SerializablePosition> spawners)
        {
            if (spawners.Remove(SelectorPosition))
                RemoveSpawner(SelectorPosition);
        }

        private void CreateSpawnersWithSelectorPosition()
        {
            switch (_spawnerType)
            {
                case SpawnerType.Main:
                    {
                        RemoveSpawner(_mainSpawner);
                        _mainSpawner = SelectorPosition;
                        InstantiateSpawner(_mainSpawnerPrefab, SelectorPosition);
                        
                        break;
                    }
                case SpawnerType.Controlled:
                    {
                        SetSpawners(_controlledSpawners, _controlledSpawnerPrefab, SelectorPosition, _maxControlledSpawnerCount);
                    }
                    break;
                case SpawnerType.Uncontrolled:
                    {
                        SetSpawners(_uncontrolledSpawners, _uncontrolledSpawnerPrefab, SelectorPosition, _maxUncontrolledSpawnerCount);
                    }
                    break;
            }
        }

        private void SetSpawners(List<SerializablePosition> playerSpawners, GameObject prefab, Vector2Int position, int maxCount)
        {
            if (playerSpawners.Count >= maxCount)
            {
                Debug.LogWarning("Enough player spawners on the scene!");
                return;
            }

            if (playerSpawners.Contains(position))
                return;

            InstantiateSpawner(prefab, position);

            playerSpawners.Add(position);
        }

        private void InstantiateSpawner(GameObject prefab, Vector2Int position)
        {
            _spawnerObjects.Add(position,
               Instantiate(
                   prefab,
                   position.ToVector3(),
                   Quaternion.identity
                   )
               );
        }

        private void RemoveSpawner(SerializablePosition position)
        {
            GameObject spawnerObject = _spawnerObjects.GetValue(position);
            
            _spawnerObjects.Remove(position);
            Destroy(spawnerObject);
        }

        [ContextMenu("Draw Edits")]
        public override void Draw()
        {
            if (_spawnerObjects == null)
                _spawnerObjects = new KeyList<SerializablePosition, GameObject>(SpawnerCapacity);
            else 
                _spawnerObjects.Clear();

            InitSpawnersWithBlend();

            InstantiateSpawner(_mainSpawnerPrefab, _mainSpawner);

            for (int i = 0; i < _controlledSpawners.Count; i++)
            {
                InstantiateSpawner(_controlledSpawnerPrefab, _controlledSpawners[i]);
            }
            
            for (int i = 0; i < _uncontrolledSpawners.Count; i++)
            {
                InstantiateSpawner(_uncontrolledSpawnerPrefab, _uncontrolledSpawners[i]);
            }
        }

        private void InitSpawnersWithBlend()
        {
            PlayerAtlas package = new(Mode);
            package.UnpackMode();

            _mainSpawner = package.MainSpawner;
            _controlledSpawners = package.ControlledSpawners.ToList();
            _uncontrolledSpawners = package.UncontrolledSpawners.ToList();
        }

        [ContextMenu("Clear Edits")]
        public override void Clear()
        {
            _mainSpawner = default;
            _controlledSpawners.Clear();
            _uncontrolledSpawners.Clear();
            
            foreach (var item in _spawnerObjects)
            {
                DestroyImmediate(item.Value, false);
            }
        }

        [ContextMenu("Save Edits")]
        public override void Save()
        {
            PlayerAtlas package 
                = new PlayerAtlas(
                    _mainSpawner, 
                    _controlledSpawners.ToArray(),
                    _uncontrolledSpawners.ToArray(),
                    Mode
                );
            
            package.PackMode();
        }

        private enum SpawnerType
        {
            Main,
            Controlled,
            Uncontrolled
        }
    }
}

#endif