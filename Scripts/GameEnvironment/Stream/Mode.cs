using Core.PlayerControl;
using System;
using System.Linq;
using UnityEngine;

namespace GameEnvironment.Stream 
{
    [CreateAssetMenu(menuName = "GAssembly/Mode")]
    public class Mode : ScriptableObject, ITransitable<(PlayerProperties, PlayerProperties)>
    {
        [SerializeField] private PlayerProperties _mainGroupProperties;
        [SerializeField] private PlayerProperties _enemyGroupProperties;
        [SerializeField] private bool _isRating;
        [SerializeField] private bool _isCustomEditingMapsAllowed;
        [SerializeField] private bool _hasAdjuster;

        [Space]
        [SerializeField] private string[] _mapModifications;

        public bool IsRating => _isRating;
        public bool IsCustomEditingMapsAllowed => _isCustomEditingMapsAllowed;
        public bool HasAdjuster => _hasAdjuster;

        private int _currentMapIndex;

        internal PlayerProperties MainGroupProperties => _mainGroupProperties;
        internal PlayerProperties EnemyGroupProperties => _enemyGroupProperties;

        private void OnEnable()
        {
            _currentMapIndex = 0;
        }

        internal void LoadMap(string name)
        {
            _currentMapIndex = Array.IndexOf(_mapModifications, name);
        }

        internal void LoadMapRandom()
        {
            _currentMapIndex = UnityEngine.Random.Range(0, _mapModifications.Length);
        }

        internal void Quit()
        {

        }

#nullable enable
        public string? GetLoadedMap()
        {
            if (_mapModifications == null || _currentMapIndex >= _mapModifications.Length)
                return null;

            return _mapModifications.FirstOrDefault(m => m.Equals(_mapModifications[_currentMapIndex]));
        }
#nullable restore

        void ITransitable<(PlayerProperties, PlayerProperties)>.Take((PlayerProperties, PlayerProperties) data)
        {
            _mainGroupProperties = data.Item1;
            _enemyGroupProperties = data.Item2;
        }
    }
}