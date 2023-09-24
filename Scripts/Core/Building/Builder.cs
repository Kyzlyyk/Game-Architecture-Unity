using UnityEngine;
using System.Linq;
using Kyzlyk.Helpers.GMesh;
using Kyzlyk.Helpers.Extensions;
using Core.Building.Modules;
using System.Collections.Generic;
using Core.Layout.Space;
using Unity.Profiling;

namespace Core.Building
{
    [ExecuteAlways]
    public sealed class Builder : CustomMesh, ISpaceEditor
    {
        public const byte GMaterialSidesCount = 4;

        [SerializeField] private Dimension _buildingType;
        public Dimension BuildingType => _buildingType;

        public bool FixedSize { get; set; }
        public Bounds Bounds { get; set; }
        public Supervisor Supervisor { get; private set; }

        private readonly Dictionary<Vector2Int, MeshStructure> _structure = new(50);

        private bool _wasRemoved;

        //TODO: Optimize builder removing 
        private readonly static ProfilerMarker Marker = new("Builder");

        public void InitDependencies()
        {
            base.Awake();
            Supervisor = new(this);
        }

        public bool HasGMaterial(Vector2Int position)
        {
            return _structure.ContainsKey(position);
        }

        public bool HasGMaterial(Vector2 position)
        {
            return HasGMaterial(ToLocal(position));
        }

        public bool CheckWay(Vector2 from, Vector2 to, out Vector2 contact)
        {
            float distance = Vector2.Distance(from, to);

            Vector2 direction = to - from;
            Vector2 nextPosition;

            for (int i = 0; i < distance; i++)
            {
                nextPosition = (from + new Vector2(i, i) * (direction / distance)).Floor();

                if (HasGMaterial(nextPosition))
                {
                    contact = nextPosition;
                    return true;
                }
            }

            contact = new Vector2();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="applyChanges">If true, all marked GMaterials will be removed immediately.
        /// Otherwise, call the Apply() method to apply changes.</param>
        public bool RemoveGMaterial(Vector2Int position, bool applyChanges = true)
        {
            if (!RemoveGMaterial(position))
                return false;

            if (applyChanges)
                Apply();

            return true;
        }

        public bool RemoveGMaterial(Vector2 position, bool applyChanges = true)
        {
            return RemoveGMaterial(ToLocal(position), applyChanges);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="applyChanges">If true, all marked GMaterials will be removed immediately.
        /// Otherwise, call the Apply() method when you need to display changes. </param>
        public void RemoveGMaterials(IEnumerable<Vector2Int> positions, bool applyChanges = true)
        {
            if (!positions.Any()) return;

            foreach (Vector2Int position in positions)
                RemoveGMaterial(position);

            if (applyChanges)
                Apply();
        }

        public void RemoveGMaterials(IEnumerable<Vector2> positions, bool applyChanges = true)
        {
            if (!positions.Any()) return;

            foreach (Vector2 position in positions)
                RemoveGMaterial(position);

            if (applyChanges)
                Apply();
        }

        private bool RemoveGMaterial(Vector2Int position)
        {
            if (_structure.Remove(position))
            {
                Supervisor.UnwrapGMaterial(position);
                _wasRemoved = true;

                return true;
            }

            return false;
        }

        private bool RemoveGMaterial(Vector2 position)
        {
            return RemoveGMaterial(ToLocal(position));
        }

        public bool CreateGMaterial(Vector2 position, GMaterialTexture texture, bool applyChanges = true)
        {
            return CreateGMaterial(ToLocal(position), texture, applyChanges);
        }

        public void CreateGMaterials(IEnumerable<Vector2Int> positions, GMaterialTexture texture, bool applyChanges = true)
        {
            if (!positions.Any()) return;

            foreach (Vector2Int position in positions)
                CreateGMaterial(position, texture, false);

            if (applyChanges)
                Apply();
        }

        public void CreateGMaterials(IEnumerable<Vector2> positions, GMaterialTexture texture, bool applyChanges = true)
        {
            if (!positions.Any()) return;

            foreach (Vector2 position in positions)
                CreateGMaterial(position, texture, false);

            if (applyChanges)
                Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>    
        /// <param name="texture"></param>
        /// <param name="applyChanges">If true, all marked GMaterials will be displayed immediately.
        /// Otherwise, call the Apply() method to apply and display changes.</param>
        public bool CreateGMaterial(Vector2Int position, GMaterialTexture texture, bool applyChanges = true)
        {
            if (_structure.ContainsKey(position)) return false;

            MeshStructure meshStructure = _buildingType switch
            {
                Dimension.TwoD => GMeshUtility.GetSquare((Vector2)position, Vertices.Count),
                Dimension.ThreeD => GMeshUtility.GetCube((Vector2)position, Vertices.Count),

                _ => null,
            };

            _structure.Add(position, meshStructure);

            Vertices.AddRange(meshStructure.Vertices);
            Triangles.AddRange(meshStructure.Triangles);

            // working with texture parameter...
            UVs.AddRange(meshStructure.UVs);

            Supervisor.WrapGMaterial(ToLocal(position));

            if (applyChanges)
                Apply();

            return true;
        }

        public override void Clear()
        {
            base.Clear();

            _structure.Clear();

            Supervisor.UnwrapAll();

            Apply();
        }

        public override void Apply()
        {
            Marker.Begin();

            if (_wasRemoved)
            {
                UpdateMeshStructure();
                _wasRemoved = false;
            }

            Mesh.Clear();

            Mesh.SetVertices(Vertices);
            Mesh.SetTriangles(Triangles, 0);

            Mesh.SetUVs(0, UVs);

            Mesh.RecalculateNormals();
    
            if (FixedSize)
                Mesh.bounds = Bounds;
            else
                Mesh.RecalculateBounds();

            Supervisor.ApplyWrap();

            Marker.End();
        }

        private void UpdateMeshStructure()
        {
            ClearStructure();

            void AddTriangles(int offset)
            {
                Triangles.Add(offset - 4);
                Triangles.Add(offset - 3);
                Triangles.Add(offset - 2);

                Triangles.Add(offset - 3);
                Triangles.Add(offset - 1);
                Triangles.Add(offset - 2);
            }

            int length = 0;

            if (_buildingType == Dimension.TwoD)
                length = 1;

            else if (_buildingType == Dimension.ThreeD)
                length = 6;

            foreach (var gmaterial in _structure)
            {
                Vertices.AddRange(gmaterial.Value.Vertices);

                for (int i = 0, j = 0; i < length; i++, j += 4)
                {
                    AddTriangles(Vertices.Count - j);
                }

                UVs.AddRange(gmaterial.Value.UVs);
            }
        }

        public static Vector2Int ToLocal(Vector2 position)
        {
            return Vector2Int.FloorToInt(position);
        }

        public enum Dimension
        {
            TwoD,
            ThreeD
        }
    }
}