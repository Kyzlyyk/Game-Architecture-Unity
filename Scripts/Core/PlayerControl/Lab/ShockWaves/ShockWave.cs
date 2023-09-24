using Kyzlyk.Helpers.Math;
using Kyzlyk.Core;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Core.Building;
using Core.Layout.Space;
using DG.Tweening;
using GameEnvironment.Stream;
using Core.PlayerControl.Marshalling;
using System;

namespace Core.PlayerControl.Lab.ShockWaves
{
    public abstract class ShockWave : ScriptableObject, IAbilityExecutor
    {
        [SerializeField] private ShockWaveCard _card;

        [Header("Features")]
        [SerializeField] private bool _removeAfterBlow;
        [SerializeField] private bool _move;
        [SerializeField] private bool _shakeCamera;
        [Tooltip("Determines whether shockwave removing gmaterials in main chunk")]
        [SerializeField] private bool _toDrill;

        [Header("Settings")]
        [SerializeField] private float _buildingDuration;
        [SerializeField] private float _removingDuration;
        [Tooltip("Delay between end of building and removing wave if enabled")]
        [SerializeField] private float _delayBetweenEndBuildingAndRemove;
        [SerializeField] private float _moveSpeed;

        [Header("Camera Shake Settings")]
        [SerializeField] private float _strength = 1;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private int _randomness = 90;
        [SerializeField] private bool _fadeOut = true;

        public Chunk Environment { get; set; }

        protected float MoveChunkSpeed => _moveSpeed;
        protected float BuildingDuration => _buildingDuration;
        protected float RemovingDuration => _removingDuration;

        protected bool ToDrill => _toDrill;

        protected virtual float CameraShakeDuration
        {
            get
            {
                return _buildingDuration +
                    (_removeAfterBlow ? _removingDuration + _delayBetweenEndBuildingAndRemove : 0f);
            }
        }

        protected LaunchWaveConfig LaunchConfig { get; private set; }

        protected ICoroutineExecutor CoroutineExecutor => GStream.CoroutineExecutor;

        private bool _hasLastWaveFinished = true;

        public abstract event EventHandler<IAbilityExecutor, Player> OnInteracted;
        public event EventHandler<IAbilityExecutor, EventArgs> OnExecuted;

        public abstract IInteractor Interactor { get; }

        public bool IsActive { get; private set; }

        public IAbilityCover<IAbilityExecutor> Cover => _card;

        private void OnValidate()
        {
            if (_move)
                _removeAfterBlow = true;
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _hasLastWaveFinished = true;
#endif
                if (Environment == null)
                    Environment = Singleton<SpaceLayout>.Instance.Environment;
#if UNITY_EDITOR
            }
#endif
        }

        public void Execute(Player player)
        {
            LaunchWave(player.transform.position, player.MovementModule.CurrentDirection);
        }

        private void LaunchWave(Vector2 startPoint, UnitVector direction)
        {
            if (!_hasLastWaveFinished)
            {
                Debug.LogError("The last Wave hasn't finished its journey yet!");
                return;
            }

            _hasLastWaveFinished = false;

            LaunchConfig = new LaunchWaveConfig()
            {
                Direction = direction,
                StartPoint = startPoint,
            };

            PrepareBeforeLaunch();

            if (_shakeCamera)
                ShakeCamera();

            if (_toDrill)
                InitDrillMode(Environment.Builder);

            if (_move)
                InitMovementMode();

            CoroutineExecutor.StartCoroutine(LaunchWave_Coroutine());
        }

        protected virtual void ShakeCamera()
        {
            Camera.main.transform.DOShakePosition(CameraShakeDuration, randomness: _randomness, vibrato: _vibrato, strength: _strength, fadeOut: _fadeOut);
        }

        private IEnumerator LaunchWave_Coroutine()
        {
            if (_removeAfterBlow)
                BuildWave(_buildingDuration);
            else
                BuildPermanentWave(_buildingDuration, Environment.Builder);

            yield return new WaitForSeconds(_buildingDuration);

            if (_removeAfterBlow)
            {
                yield return new WaitForSeconds(_delayBetweenEndBuildingAndRemove);

                RemoveWave(_removingDuration);
                yield return new WaitForSeconds(_removingDuration);

                DisposeWave();
            }

            OnExecuted?.Invoke(this, EventArgs.Empty);
            _hasLastWaveFinished = true;
        }

        protected abstract void DisposeWave();
        protected abstract void InitMovementMode();
        protected abstract void BuildWave(float duration);
        protected abstract void BuildPermanentWave(float duration, Builder builder);
        protected abstract void RemoveWave(float duration);
        protected abstract void InitDrillMode(Builder environment);

        protected virtual void PrepareBeforeLaunch() { }

        public virtual void Suspend()
        {
            IsActive = false;
        }

        public virtual void Continue()
        {
            IsActive = true;
        }

        protected struct LaunchWaveConfig
        {
            public UnitVector Direction;
            public Vector2 StartPoint;
        }
    }
}