using Kyzlyk.Helpers.Math;
using System;
using UnityEngine;

namespace Core.PlayerControl
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Thrower : MonoBehaviour
    {
        public ElasticitySettings ElasticitySettings;

        [Space]
        [Tooltip("When this object's velocity will be less than this value, then invoke the '" + nameof(OnEndThrowed) + "' event")]
        [SerializeField] private float _minVelocity;
        [Space]
        [Tooltip("If this time is out, this object will be throwed to random direction.")]
        [SerializeField] private float _maxTimeOfReflection = 10;

        public event EventHandler OnEndThrowed;
        
        private Rigidbody2D _rigidbody;
        public Vector2 Velocity
        {
            get
            {
                if (IsVelocityApproximatelyZero())
                    return Vector2.zero;
                else 
                    return _rigidbody.velocity;
            }
        }

        private Vector2 _previousVelocity;
        
        private bool _throwed;

        private float _elapsedTimeOfReflection;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity += (-_previousVelocity) / ElasticitySettings.DecelerationTime;

            if (IsVelocityApproximatelyZero())
            {
                if (_throwed)
                {
                    OnEndThrowed?.Invoke(this, EventArgs.Empty);
                    _throwed = false;
                }
            }
        }

        private void Update()
        {
            _previousVelocity = _rigidbody.velocity;

            ThrowRandomIfLong();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            float speed = _previousVelocity.magnitude;

            Vector2 vector = Vector2.Reflect(_previousVelocity.normalized, -collision.GetContact(0).normal);
            
            Vector2 force = Mathf.Min(
                ElasticitySettings.ReflectionForce * speed, ElasticitySettings.ReflectionForce)
                * vector;

            _rigidbody.AddForce(force, ElasticitySettings.ForceMode);
        }

        public void Throw(UnitVector direction)
        {
            Throw_Unsafe(direction);
        }

        private void Throw_Unsafe(Vector2 direction)
        {
            _rigidbody.AddForce(direction * ElasticitySettings.ThrowForce, ElasticitySettings.ForceMode);
            
            _throwed = true;
        }

        private bool IsVelocityApproximatelyZero()
        {
            float minVelocity = Mathf.Max(_minVelocity, 0.05f);

            return (Mathf.Abs(_rigidbody.velocity.x) < minVelocity) && Mathf.Abs(_rigidbody.velocity.y) < minVelocity;
        }

        private void ThrowRandomIfLong()
        {
            if (_throwed)
            {
                _elapsedTimeOfReflection += Time.deltaTime;

                if (_elapsedTimeOfReflection >= _maxTimeOfReflection)
                {
                    Throw_Unsafe(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
                    _elapsedTimeOfReflection = 0;
                }
            }
        }
    }
}