using UnityEngine;
using Kyzlyk.Helpers;
using Core.Layout.Space;
using Kyzlyk.Helpers.Extensions;
using Kyzlyk.Collections.Extensions;

namespace Gameplay.Modes.ChunkModifications
{
    public sealed class ChunkSwaper : MonoBehaviour
    {
        [SerializeField] private Chunk[] _chunks;
        [SerializeField] private int _randomFillingCoefficient = 10;
        [SerializeField] private float _deadZoneOffset;

        private int _currentChunkIndex;
        private int _nextChunkIndex = 1;

        private Chunk Current => _chunks[_currentChunkIndex];

        private void Awake()
        {
            if (_chunks.Length != 2)
                throw new System.Exception("Chunks.Length must be equal 2!");
        }

        private void Start()
        {
            _chunks.ForEach(c => GenerateChunk(c));
        }

        private void Update()
        {
            if (ChunkWithinDeadZone())
            {
                Current.Builder.Clear();
                GenerateChunk(Current);
                
                SwapChunk();
            }
        }

        private bool ChunkWithinDeadZone()
        {
            return Current.transform.position.x <= _deadZoneOffset - Current.Width / 2;
        }

        private void SwapChunk()
        {
            Current.TryConnect(Side.Right);

            (_currentChunkIndex, _nextChunkIndex) = (_nextChunkIndex, _currentChunkIndex);
        }

        private void GenerateChunk(Chunk chunk)
        {
            ChunkGenerator.GenerateWithPlatforms(chunk.Width, chunk.Height, chunk.Builder, _randomFillingCoefficient, new Vector2(0f, chunk.Height / -2));
        }

#if UNITY_EDITOR
        [ContextMenu("Set dafault dead zone")]
        private void SetDefaultDeadZone()
        {
            _deadZoneOffset = -SharedConstants.CameraWidth;
        }
#endif
    }
}
