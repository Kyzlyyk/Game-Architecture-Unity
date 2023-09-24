using UnityEngine;

namespace Gameplay.PlayerComponents
{
    public class StayPutTracker : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _XReturnAnimation;

        [SerializeField] private Vector3 _StaticPosition;
        [SerializeField] private float _Speed;
        [SerializeField] private float _Duration;
        
        private float _elapsedTime;

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _Duration)
                _elapsedTime = 0;
        }

        private void FixedUpdate()
        {
            int direction = 0;

            if (Mathf.FloorToInt(_StaticPosition.x) > Mathf.FloorToInt(transform.position.x))
                direction = 1;
            else if (Mathf.FloorToInt(_StaticPosition.x) < Mathf.FloorToInt(transform.position.x))
                direction = -1;

            float progress = _elapsedTime / _Duration;

           // print(_XReturnAnimation.Evaluate(progress));

            transform.position += new Vector3(_Speed * direction, 0f);
        }
    }
}