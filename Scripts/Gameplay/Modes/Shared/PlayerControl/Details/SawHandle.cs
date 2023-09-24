using DG.Tweening;
using Core.Layout.PlayerControl;
using Core.PlayerControl;
using Core.PlayerControl.Lab.Details;
using Kyzlyk.Collections.Extensions;
using Kyzlyk.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Core.PlayerControl.Marshalling;
using Gameplay.Layout.PlayerControl.Interactors;
using Core.PlayerControl.Lab;
using UnityEditor;

namespace Gameplay.Layout.PlayerControl.Details
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_Details + "/SawHandle")]
    internal class SawHandle : Detail, ICombater, ICounterAttackHandler
    {
        [SerializeField] private float _maxDistanceToTied;
        [SerializeField, Min(1f)] private int _maxPlayersCanTied;
        [Space]
        [SerializeField] private DOPunchArgs _punchArguments;
        [SerializeField] private DOShakeArgs _shakeArguments;

        private PlayerLayout PlayerLayout => Singleton<PlayerLayout>.Instance;
        private LineRenderer _lineRenderer;

        private bool _isTied;
        private int _shaked;

        private PlayerOffset[] _groupIndices;
        private int _detectedIncludingAnchor;
        private int DetectedWithoutAnchor => _detectedIncludingAnchor - 1;

        private int MaxPlayersCanTied => _maxPlayersCanTied + 1;

        public override IInteractor Interactor => _interactor;

        private readonly IInteractor _interactor = new Damager();
        
        public override event EventHandler<IAbilityExecutor, Player> OnInteracted;
        public override event EventHandler<IAbilityExecutor, EventArgs> OnExecuted;

        private Coroutine _updateCoroutine;

        private Player _currentPlayer;

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _detectedIncludingAnchor = 0;
                _isTied = false;
                _updateCoroutine = null;
                _groupIndices = null;
                _shaked = 0;
            }
        }
#endif

        public override void Init(Player player)
        {
            CreateShell(player, typeof(LineRenderer));

            _groupIndices = new PlayerOffset[MaxPlayersCanTied + 1];
        }

        public override void Execute(Player player)
        {
            _currentPlayer = player;

            if (_isTied || !AnyEnemyDetected())
                return;

            _isTied = true;

            _lineRenderer = GetShellComponent<LineRenderer>(player);

            TieDetectedEnemies();
            _updateCoroutine = _currentPlayer.StartCoroutine(DrawLineBetweenPlayers());
        }

        private void End()
        {
            _isTied = false;
            _shaked = 0;
            _lineRenderer.positionCount = 0;
            _detectedIncludingAnchor = 0;

            _currentPlayer.StopCoroutine(_updateCoroutine);
            OnExecuted?.Invoke(this, EventArgs.Empty);
        }

        private bool IsEnemyWithinDetectionArea(Player target)
            => Vector2.Distance(target.transform.position, _currentPlayer.transform.position) <= _maxDistanceToTied;

        private bool AnyEnemyDetected()
        {
            for (int i = 0; i < PlayerLayout.Groups.Count; i++)
            {
                if (i == _currentPlayer.GroupIndex)
                    continue;

                for (int j = 0; j < PlayerLayout.Groups[i].Count; j++)
                {
                    if (IsEnemyWithinDetectionArea(PlayerLayout.Groups[i][j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private IEnumerator DrawLineBetweenPlayers()
        {
            while (true)
            {
                for (int i = 0; i < _detectedIncludingAnchor; i++)
                {
                    _lineRenderer.SetPosition(i, PlayerLayout.Groups[_groupIndices[i].Group][_groupIndices[i].Index].transform.position);
                }

                yield return null;
            }
        }

        private void TieDetectedEnemies()
        {
            SetDetectedPlayer(new PlayerOffset(_currentPlayer.GroupIndex,
                PlayerLayout.Groups[_currentPlayer.GroupIndex].IndexOf(_currentPlayer, _currentPlayer)));

            bool MaxEnemyTied() => DetectedWithoutAnchor >= MaxPlayersCanTied;

            for (int i = 0; i < PlayerLayout.Groups.Count; i++)
            {
                if (MaxEnemyTied())
                    break;

                if (i == _currentPlayer.GroupIndex)
                    continue;

                IReadOnlyList<Player> targetGroup = PlayerLayout.Groups[i];

                for (int j = 0; j < targetGroup.Count; j++)
                {
                    if (MaxEnemyTied())
                        break;

                    if (IsEnemyWithinDetectionArea(PlayerLayout.Groups[i][j]))
                    {
                        SetDetectedPlayer(new PlayerOffset(i, j));
                        _currentPlayer.StartCoroutine(InteractWithTarget(targetGroup[j]));
                    }
                }
            }

            _lineRenderer.positionCount = _detectedIncludingAnchor;
        }

        private void SetDetectedPlayer(PlayerOffset offset)
        {
            if (_detectedIncludingAnchor >= _groupIndices.Length)
                throw new ArgumentOutOfRangeException(nameof(_detectedIncludingAnchor), _maxPlayersCanTied, "More players found than the maximum expected!");

            _groupIndices[_detectedIncludingAnchor++] = offset;
        }

        private IEnumerator InteractWithTarget(Player target)
        {
            OnInteracted?.Invoke(this, target);
            
            var tween = target.transform.DOShakePosition(_shakeArguments);
            yield return tween.WaitForCompletion();

            //target.transform.DOPunchPosition(UnitVector.GetRandom(8), _punchArguments);

            _shaked++;
         
            if (_shaked >= DetectedWithoutAnchor)
                End();
        }

        bool ICounterAttackHandler.Handle()
        {
            return true;
        }

        private readonly struct PlayerOffset
        {
            public PlayerOffset(int groupIndex, int playerIndex)
            {
                Group = groupIndex;
                Index = playerIndex;
            }

            public int Group { get; }
            public int Index { get; }
        }
    }
}