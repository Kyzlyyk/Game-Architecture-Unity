using DG.Tweening;
using Core.FX.Animations;
using Kyzlyk.Core;
using System.Collections;
using UnityEngine;

namespace Gameplay.FX.Transitions
{
    [CreateAssetMenu(menuName = "FX/Transitions/Zoom Camera")]
    public class ZoomCameraTransition : Transition
    {
        [SerializeField] private float _zoom;
        
        [Header("Start")]
        [SerializeField] private float _zoomTimeStart;
        [SerializeField] private Ease _easeStart;

        [Header("End")]
        [SerializeField] private float _zoomTimeEnd;
        [SerializeField] private Ease _easeEnd;

        private Camera _camera;

        private bool _isTransitionEnded;

        private float _startZ;

        public override GEventHandler<GAnimation> OnPlayed { get; set; }

        private void OnEnable()
        {
            _isTransitionEnded = true;
        }

        protected override IEnumerator End()
        {
            yield return Executor.StartCoroutine(Transition(_zoomTimeEnd, _startZ, _easeEnd));
        }

        protected override IEnumerator Start()
        {
            _camera = Camera.main;

            _startZ = _camera.transform.localPosition.z;
            float desinationZoom = _camera.transform.localPosition.z + _zoom;
            yield return Executor.StartCoroutine(Transition(_zoomTimeStart, desinationZoom, _easeStart));
        }   

        private IEnumerator Transition(float time, float destinationZ, Ease ease)
        {
            yield return new WaitUntil(() => _isTransitionEnded);

            _isTransitionEnded = false;

            var core = _camera.transform
                .DOLocalMoveZ(destinationZ, time)
                .SetEase(ease);
            
            yield return core.WaitForCompletion();

            _isTransitionEnded = true;
        }
    }
}