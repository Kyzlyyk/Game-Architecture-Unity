using Core.PlayerControl;
using UnityEngine;

namespace Gameplay.PlayerComponents
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _smooth;
        
        private Vector3 _velocity;

        public Rigidbody2D Rigidbody { get; set; }
        public Joystick Joystick { get; set; }
        public Player Player { get; set; }

        private Vector2 _direction;

        private bool _wasReached;

        public void Move(Vector2 destination)
        {
            _direction = destination;
            _wasReached = false;
        }

        public void Update()
        {
            if (Vector2Int.RoundToInt(transform.position) == Vector2Int.RoundToInt(_direction))
                _wasReached = true;

            float horizontal;
            float vertical;
            if (!_wasReached)
            {
                horizontal = _direction.x;
                vertical = _direction.y;
            }
            else
            {
                horizontal = Joystick.Horizontal;
                vertical = Joystick.Vertical;
            }
            
            Vector3 targetVelocity = new Vector2(_speed * horizontal, _speed * vertical);
            Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, targetVelocity, ref _velocity, _smooth);
        }
    }
}