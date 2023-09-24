using System;
using Core.Layout.Interactables;
using Core.PlayerControl.AI;
using Kyzlyk.AI;
using UnityEngine;

namespace Gameplay.Layout.Interactable.Affectors
{
    [RequireComponent(typeof(AIAreaDetector))]
    public class Magnet : MonoBehaviour, IInteractable
    {
        [Range(0f, 10f)]
        [SerializeField] private float _magneticForce = 1f;
        [SerializeField] private Filter _filter;
        [SerializeField] private MagneticSource _magneticSource;

        private Predicate<GameObject> _filterPredicate;
        private AIAreaDetector _areaDetector;

        public Vector2 Position => transform.position;
        public Priority Priority => Priority.Low;
        public bool Hostile => false;

        private void Awake()
        {
            _areaDetector = GetComponent<AIAreaDetector>();

            if (_filter == null)
                _filterPredicate = o => true;
            else
                _filterPredicate = _filter.GetPredicate();
        }

        private void Update()
        {
            for (int i = 0; i < _areaDetector.AllDetectedEntities.Length; i++)
            {
                //if (!_filterPredicate(_areaDetector.DetectedEntities[i].gameObject)) return;

                _areaDetector.AllDetectedEntities[i].transform.position -= _magneticForce * 0.01f * GetMagneticCenter(_areaDetector.AllDetectedEntities[i].transform.position);
            }
        }

        private Vector3 GetMagneticCenter(Vector3 entityPosition)
        {
            return _ = _magneticSource switch
            {
                MagneticSource.Magnet => entityPosition - transform.position,
                MagneticSource.AreaCenter => entityPosition - (Vector3)_areaDetector.Center,

                _ => Vector3.zero
            };
        }

        [Serializable] 
        public class Filter
        {
            [SerializeField] private GameObject _prefab;

            public Predicate<GameObject> GetPredicate()
            {
                return new Predicate<GameObject>(g => _prefab);
            }
        }

        private enum MagneticSource
        {
            Magnet = 0,
            AreaCenter = 1,
        }
    }
}