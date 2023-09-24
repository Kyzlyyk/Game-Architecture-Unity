using UnityEngine;
using Core.Layout.Space;
using System.Collections.Generic;
using Kyzlyk.Helpers.Extensions;

namespace Gameplay.Modes.ChunkModifications
{
    internal sealed class RoomSpace : SpaceLayout
    {
        [Space(20f)]
        [SerializeField] private Chunk _innerChunk;
        [SerializeField] private Chunk _bordersChunk;
        [SerializeField] private Vector2 _offset;

        [Space]
        [SerializeField] private int _fillingCoefficient;
        [SerializeField] private bool _toDrawBorders = true;

        private List<Vector2> _space;
        public IList<Vector2> Space => _space;

        public override Chunk Environment => _innerChunk;

        public override IEnumerable<Chunk> GetAllChunks()
        {
            return new Chunk[2] { _innerChunk, _bordersChunk };
        }

        protected override void DrawWithCustom()
        {
            if (_toDrawBorders)
            {
                InitSpace();
                DrawBorders();
            }
        }

        protected override void DrawDefault()
        {
            InitSpace();
            DrawBorders();
            DrawRandom();
        }

        private void DrawRandom()
        {
            const int paddingByHeight = 2;
            const int paddingByWidth = 2;

            _space.AddRange(ChunkGenerator
                .GenerateWithPlatforms(
                _innerChunk.Width - paddingByWidth, _innerChunk.Height - paddingByHeight, _innerChunk.Builder, _fillingCoefficient, _offset));
        }

        private void DrawBorders()
        {
            byte[][] borders = GetRoomBorders();

            _space.Capacity += borders.Length;
            _bordersChunk.LinkModuleDependencies();
            _space.AddRange(ChunkGenerator.GenerateFromEnd(borders, _bordersChunk, _offset));
        }

        private void InitSpace()
        {
            _space = new List<Vector2>(_innerChunk.Size.Perimeter() + (_toDrawBorders ? _bordersChunk.Size.Perimeter() : 0));
        }

        private byte[][] GetRoomBorders()
        {
            byte[][] space = new byte[_bordersChunk.Height][];

            for (int i = 0; i < _bordersChunk.Height; i++)
            {
                space[i] = new byte[_bordersChunk.Width];

                for (int j = 0; j < _bordersChunk.Width; j++)
                {
                    if ((i == 0 || i == _bordersChunk.Height - 1) || (j == 0 || j == _bordersChunk.Width - 1))
                        space[i][j] = 1;
                }
            }

            return space;
        }

#if UNITY_EDITOR
        [ContextMenu("Set default offset")]
        private void SetDefaultOffset()
        {
            _offset = new Vector2(-_innerChunk.Width / 2, -_innerChunk.Height / 2);
        }
#endif
    }
}