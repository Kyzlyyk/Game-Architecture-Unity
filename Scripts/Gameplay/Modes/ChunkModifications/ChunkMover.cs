using UnityEngine;

namespace Gameplay.Modes.ChunkModifications
{
    public class ChunkMover : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        public float Speed 
        { 
            get => _speed;
            set => _speed = value; 
        }

        private void Update()
        {
            transform.position =
                Vector2.Lerp(transform.position, (Vector2)transform.position - new Vector2(Speed, 0), Time.deltaTime);
        }
    }
}