#if UNITY_EDITOR

using UnityEngine;
using Kyzlyk.Helpers.Extensions;
using Core.Building;
using System.Collections.Generic;
using System.Linq;

namespace Core.Layout.Space
{
    public sealed class SpaceEditor : Editor
    {
        [SerializeField] private Chunk _chunk;

        private List<SerializablePosition> _space;

        private Builder Builder => _chunk.Builder;

        protected override void Init()
        {
            _chunk.LinkModuleDependencies();

            _space = new List<SerializablePosition>(_chunk.Size.Perimeter());
        }

        private void Update()
        {
            if (!IsEditing) return;

            SetSelectorPositionWithMouse();

            if (!Camera.IsObjectVisible(new Bounds(new Vector3(SelectorPosition.x + .5f, SelectorPosition.y + .5f), new Vector3(1f, 1f))))
                return;
            
            if (Input.GetMouseButton(0))
            {
                PutGMaterial();
            }
            else if (Input.GetMouseButton(1))
            {
                RemoveGMaterial();
            }
        }

        [ContextMenu("Draw Edits")]
        public override void Draw()
        {
            SpaceAtlas atlas = InitAtlas();
            InitSpaceWithAtlas(atlas);

            for (int i = 0; i < _space.Count; i++)
            {
                Builder.CreateGMaterial(_space[i], default, false);
            }

            Builder.Apply();
        }

        [ContextMenu("Clear Edits")]
        public override void Clear()
        {
            if (_space == null)
                InitSpaceWithAtlas(InitAtlas());

            _space.Clear();
            Builder.Clear();
        }

        private void PutGMaterial()
        {
            if (Builder.CreateGMaterial(SelectorPosition, default))
                _space.Add(SelectorPosition);
        }

        private void RemoveGMaterial()
        {
            if (Builder.RemoveGMaterial(SelectorPosition))
                _space.Remove(SelectorPosition); 
        }

        private void OnDrawGizmos()
        {
            if (!IsEditing) return;

            Vector3 center = new(SelectorPosition.x + .5f, SelectorPosition.y + .5f, 1f);
            Gizmos.DrawCube(center, new Vector3(1f, 1f, 0f));
        }

        [ContextMenu("Save Edits")]
        public override void Save()
        {
            SpaceAtlas newAtlas = new(_space.ToArray(), Mode);
            newAtlas.PackMode();
        }

        private void InitSpaceWithAtlas(SpaceAtlas atlas)
        {
            _space = atlas.Space.ToList();
        }

        private SpaceAtlas InitAtlas()
        {
            SpaceAtlas atlas = new(Mode);
            atlas.UnpackMode();

            return atlas;
        }
    }
}

#endif