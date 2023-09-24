using Core.PlayerControl.Lab.ShockWaves;
using Core.PlayerControl;
using System.Collections;
using Core.Building;
using UnityEngine;
using Kyzlyk.Helpers.Math;
using Core.Layout.Space;
using Kyzlyk.Helpers.Extensions;
using System;
using Core.PlayerControl.Lab;
using Kyzlyk.Core;
using Core.PlayerControl.Marshalling;
using Gameplay.Layout.PlayerControl.Interactors;

namespace Gameplay.Layout.PlayerControl.ShockWaves
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_ShockWaves + "/Hammer")]
    public class Hammer : ShockWave
    {
        [Header("Special Features")]
        [SerializeField] private Chunk _chunkPrefab;
        [SerializeField] private bool _ignoreBorders;
        [Space]
        [SerializeField] private Vector2Int _handleSize;
        [SerializeField] private Vector2Int _headSize;

        protected override float CameraShakeDuration => base.CameraShakeDuration;

        public override IInteractor Interactor => _interactor;
        private readonly IInteractor _interactor = new Damager();

        private Chunk _hammerChunk;
        private GMaterialShell _shell;

        private UnitVector _throwDirection;

        public override event EventHandler<IAbilityExecutor, Player> OnInteracted;

        protected override void PrepareBeforeLaunch()
        {
            _hammerChunk = Instantiate(_chunkPrefab);
            _throwDirection = LaunchConfig.Direction.RoundToDirection();

            _hammerChunk.Surface.OnTriggered += Surface_OnTriggered;
            Physics2D.IgnoreLayerCollision(_hammerChunk.Surface.gameObject.layer, SharedConstants.UndestroyableGMLayerInt, _ignoreBorders);
        }

        private void Surface_OnTriggered(object sender, Collider2D collider)
        {
            if (collider.gameObject.layer == SharedConstants.PlayerLayerInt)
            {
                Player player = collider.GetComponent<Player>();
                OnInteracted(this, player);
            }
        }

        protected override void BuildWave(float duration)
        {
            CoroutineExecutor.StartCoroutine(
                DoWaveAction(_hammerChunk.Builder, duration, WaveAction.Create));
        }

        protected override void BuildPermanentWave(float duration, Builder builder)
        {
            CoroutineExecutor.StartCoroutine(
                DoWaveAction(builder, duration, WaveAction.Create));
        }

        protected override void RemoveWave(float duration)
        {
            CoroutineExecutor.StartCoroutine(
                DoWaveAction(_hammerChunk.Builder, duration, WaveAction.Remove));
        }

        private IEnumerator DoWaveAction(Builder builder, float duration, WaveAction action)
        {
            float halfDelay = (duration / (_handleSize.x + _headSize.x)) * .5f;

            yield return CoroutineExecutor.StartCoroutine(
                DoActionWithWavePart(_handleSize, halfDelay, builder, action));

            yield return CoroutineExecutor.StartCoroutine(
                DoActionWithWavePart(_headSize, halfDelay, builder, action, new Vector2(_handleSize.x, _headSize.y * -.25f)));
        }

        private IEnumerator DoActionWithWavePart(Vector2 size, float delay, Builder builder, WaveAction action, Vector2 offset = default)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (action == WaveAction.Create)
                    {
                        Vector2 gmPosition = LocalToWorldPosition(new Vector2(x, y) + offset);
                        
                        if (ToDrill)
                            Environment.Builder.RemoveGMaterial(gmPosition);
                        
                        builder.CreateGMaterial(gmPosition, default, false);
                    }
                    
                    else if (action == WaveAction.Remove)
                        builder.RemoveGMaterial(LocalToWorldPosition(new Vector2(x, y) + offset), false);
                }

                builder.Apply();

                yield return new WaitForSeconds(delay);
            }
        }

        protected override void InitDrillMode(Builder builder)
        {
            _shell = _hammerChunk.gameObject.AddComponent<GMaterialShell>();

            _shell.Builder = builder;
            _shell.HandleMode = HandleMode.IgnoreSurfaceAndTouchHandle;
            _shell.Type = ShellType.CustomShape;
            _shell.IgnoreSurfaceLayer = SharedConstants.GMaterialLayerInt;

            int length = _handleSize.x + _headSize.x;
            Vector2 size = new(length, _headSize.y);
            _shell.SetBoundaryShape(new Bounds(LaunchConfig.StartPoint + (size * .5f), size));
            
            _shell.OnTouched += Shell_OnTouched;
            _shell.OnHandled += Shell_OnHandled;
        }

        private void Shell_OnHandled(object sender, EventArgs args)
        {
            Environment.Builder.Apply();
        }

        private void Shell_OnTouched(object sender, Vector2 contactPoint)
        {
            Environment.Builder.RemoveGMaterial(contactPoint, false);
        }

        protected override void InitMovementMode()
        {
            var rb = _hammerChunk.gameObject.AddComponent<Rigidbody2D>();

            rb.gravityScale = 0;
            rb.AddForce(MoveChunkSpeed * LaunchConfig.Direction.RoundToDirection().Vector2, ForceMode2D.Impulse);
        }

        protected override void DisposeWave()
        {
            if (ToDrill)
                _shell.OnTouched -= Shell_OnTouched;    

            Destroy(_hammerChunk.gameObject);
        }

        private Vector2 LocalToWorldPosition(Vector2 partPosition)
        {
            return MathUtility.RotateVector(partPosition, _throwDirection).Round()
                + LaunchConfig.StartPoint.Round();
        }

        private enum WaveAction
        {
            Remove = 0,
            Create = 1
        }
    }
}