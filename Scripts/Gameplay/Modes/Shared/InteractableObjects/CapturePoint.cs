using System;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using System.Collections.Generic;
using Kyzlyk.AI;
using Core.PlayerControl.AI;
using Core.Layout.Interactables;
using Core.Layout.PlayerControl;
using Core.PlayerControl;
using Kyzlyk.Collections;
using Kyzlyk.Attributes;
using Kyzlyk.Core;
using UnityEngine.Serialization;

namespace Gameplay.Layout.Interactable
{
    [RequireComponent(typeof(AIAreaDetector))]
    public class CapturePoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _singleUse;

        [Header("Capture Settings")]
        [SerializeField] private float _timeToIncreaseCapturingPercentage;
        [SerializeField][Range(1, 100)] private int _percentageIncrease;
        
        [HideInInspector][ReadOnlyProperty][SerializeField] private int _capturedPercentage;
        [HideInInspector][ReadOnlyProperty][SerializeField] private int _ownerGroupIndex;

        [HideInInspector][SerializeField] private bool _openCaptureStatus;
            
        private readonly KeyList<int, Team> _capturerTeams = new(2);
        private AIAreaDetector _detectionArea;
        private float _timerToIncreaseCapturingPercentage;
        
        public int CapturedPercentage => _capturedPercentage;
        public IReadOnlyList<Player> OwnerGroup => Singleton<PlayerLayout>.Instance.Groups[_ownerGroupIndex];
        public int OwnerGroupIndex => _ownerGroupIndex;

        public Vector2 Position => transform.position;
        public Priority Priority => Priority.High;
        public bool Hostile => false;

        public event EventHandler<CapturePoint, PointOwnerEventArgs> OnPointCaptured;

        private void Awake()
        {
            _detectionArea = GetComponent<AIAreaDetector>();
            _detectionArea.EntityEntered += DetectionArea_EntityEntered;
            _detectionArea.EntityExited += DetectionArea_EntityExited;
        }

        private void Update()
        {
            if (_detectionArea.AnyEntityDetected)
                IncreaseCapturedPercentageInCapturers();
        }

        private void IncreaseCapturedPercentageInCapturers()
        {
            if (_capturerTeams.Count == 0 || (_capturedPercentage == 100 && _singleUse))
                return;

            _timerToIncreaseCapturingPercentage += Time.deltaTime;

            if (_timerToIncreaseCapturingPercentage >= _timeToIncreaseCapturingPercentage)
            {
                _timerToIncreaseCapturingPercentage = 0;

                (int percentage, int largestGroup) = GetTargetPercentageAndGroup();

                if (_capturedPercentage <= 0 || _ownerGroupIndex == '\0')
                    _ownerGroupIndex = largestGroup;

                if (_ownerGroupIndex != largestGroup && percentage > 0)
                    percentage = -percentage;
                
                _capturedPercentage += percentage;
                _capturedPercentage = Mathf.Max(_capturedPercentage, 0);

                if (CapturedPercentage >= 100)
                {
                    Capture();

                    //if (CurrentPercentage > 100)
                    _capturedPercentage = 100;
                }
            }
        }

        private (int Percentage, int Group) GetTargetPercentageAndGroup()
        {
            int largestGroup = _capturerTeams.GetKey(0);
            int largestGroupIndex = 0;

            for (int i = 1; i < _capturerTeams.Count; i++)
            {
                if (_capturerTeams[i].Value.Capturers.Count > _capturerTeams[i - 1].Value.Capturers.Count)
                {
                    largestGroup = _capturerTeams.GetKey(i);
                    largestGroupIndex = i;
                }
            }

            int capturerDifference = _capturerTeams[largestGroupIndex].Value.Capturers.Count;

            for (int i = 0; i < _capturerTeams.Count; i++)
            {
                if (i == largestGroupIndex) continue;

                capturerDifference -= _capturerTeams[i].Value.Capturers.Count;
            }

            return ((capturerDifference * _percentageIncrease), largestGroup);
        }

        private void DetectionArea_EntityEntered(Collider2D[] before, Collider2D[] after)
        {
            ICapturer target = FindUniqeCapturer(after, before);

            void AddNewTeam()
            {
                var team = new Team();
                team.TryAddCapturer(target);

                _capturerTeams.Add(target.GroupIndex, team);
            }

            if (_capturerTeams.Count == 0)
            {
                AddNewTeam();
                return;
            }
            
            if (_capturerTeams.TryGetValue(target.GroupIndex, out Team team))
                team.TryAddCapturer(target);
            else
                AddNewTeam();
        }

        private void DetectionArea_EntityExited(Collider2D[] before, Collider2D[] after)
        {
            ICapturer target = FindUniqeCapturer(after, before);

            if (_capturerTeams.TryGetValue(target.GroupIndex, out Team team))
            {
                for (int i = 0; i < team.Capturers.Count; i++)
                {
                    if (team.Capturers[i].ID == target.ID)
                        team.Capturers.RemoveAt(i);
                }
            }
        }

        [CanBeNull]
        private ICapturer FindUniqeCapturer(Collider2D[] first, Collider2D[] second)
        {
            if (first.Length > second.Length)
                return first.Except(second, new CapturerEqualityComparer()).FirstOrDefault().gameObject.GetComponent<ICapturer>();
            
            else if (second.Length > first.Length)
                return second.Except(first, new CapturerEqualityComparer()).FirstOrDefault().gameObject.GetComponent<ICapturer>();

            return null;
        }

        private void Capture()
        {
            OnPointCaptured?.Invoke(this, new PointOwnerEventArgs(_ownerGroupIndex, _capturerTeams.GetValue(_ownerGroupIndex).Capturers));
        }

        private class Team
        {
            public List<ICapturer> Capturers { get; } = new List<ICapturer>();

            public bool TryAddCapturer(ICapturer capturer)
            {
                if (Capturers.Any(c => c.ID == capturer.ID)) 
                    return false;
                
                Capturers.Add(capturer);
                return true;
            }
        }

        private struct CapturerEqualityComparer : IEqualityComparer<Collider2D>
        {
            public bool Equals(Collider2D x, Collider2D y)
            {
                return x.gameObject.GetComponent<ICapturer>().ID == y.gameObject.GetComponent<ICapturer>().ID;
            }

            public int GetHashCode(Collider2D obj)
            {
                return obj.gameObject.GetComponent<ICapturer>().ID;
            }
        }

        public sealed class PointOwnerEventArgs : EventArgs
        {
            public PointOwnerEventArgs(int group, IEnumerable<ICapturer> capturers)
            {   
                Group = group;
                Capturers = capturers;
            }

            public int Group { get; }
            public IEnumerable<ICapturer> Capturers { get; }
        }
    }
}