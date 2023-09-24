using UnityEngine;
using System.Collections.Generic;
using System;
using Core.Layout.Space;

namespace Core.Building.Modules
{
    [ExecuteAlways]
    [RequireComponent(typeof(PolygonCollider2D))]
    public sealed class FlexibleSurface : MonoBehaviour, IWrapper, ISpaceEditor
    {
        [SerializeField] private Builder _builder;
        public Builder Builder { get => _builder; set => _builder = value; }

        private PolygonCollider2D _polygonCollider;
        private readonly Dictionary<Vector2Int, List<Vector2>> _pathSets = new();

        public event EventHandler<Collision2D> OnCollided;
        public event EventHandler<Collider2D> OnTriggered;

        public PolygonCollider2D Collider => _polygonCollider;

        public void InitDependencies()
        {
            _polygonCollider = GetComponent<PolygonCollider2D>();
        }

        public void WrapGMaterial(Vector2Int gMaterialPosition)
        {
            List<Vector2> vertices = new()
            {
                gMaterialPosition,
                gMaterialPosition + new Vector2(1, 0),
                gMaterialPosition + new Vector2(1, 1),
                gMaterialPosition + new Vector2(0, 1),
            };

            Vector2Int firstVertex = Builder.ToLocal(vertices[0]);

            if (CheckAccessToSide(vertices))
            {
                if (!_pathSets.TryAdd(firstVertex, vertices))
                    return;
            }

            AdjustAround(firstVertex);
        }

        public void UnwrapGMaterial(Vector2Int gMaterialPosition)
        {
            if (_pathSets.ContainsKey(gMaterialPosition))
            {
                _pathSets.Remove(gMaterialPosition);
            }

            AdjustAround(gMaterialPosition);
        }

        public void UnwrapAll()
        {
            _polygonCollider.pathCount = 0;
            _pathSets.Clear();
        }

        public void ApplyWrap()
        {
            _polygonCollider.pathCount = 0;

            foreach (var pathSet in _pathSets)
            {
                _polygonCollider.pathCount++;
                _polygonCollider.SetPath(_polygonCollider.pathCount - 1, pathSet.Value);
            }
        }

        private List<Vector2> SortVertices(IReadOnlyList<Vector3> unsortedVertices)
        {
            return new List<Vector2>
            {
                unsortedVertices[0],
                unsortedVertices[2],
                unsortedVertices[3],
                unsortedVertices[1]
            };
        }

        private void AdjustAround(Vector2Int gMaterialPosition)
        {
            Vector2Int[] gMaterialPositions = new Vector2Int[4];

            gMaterialPositions[0] = gMaterialPosition + new Vector2Int(-1, 0);
            gMaterialPositions[1] = gMaterialPosition + new Vector2Int(1, 0);
            gMaterialPositions[2] = gMaterialPosition + new Vector2Int(0, 1);
            gMaterialPositions[3] = gMaterialPosition + new Vector2Int(0, -1);

            List<Vector2> vertices = new(4);

            for (int i = 0; i < gMaterialPositions.Length; i++)
            {
                if (!Builder.HasGMaterial(gMaterialPositions[i]))
                    continue;

                vertices.AddRange(new Vector2[]
                {
                    gMaterialPositions[i],
                    gMaterialPositions[i] + new Vector2(1, 0),
                    gMaterialPositions[i] + new Vector2(1, 1),
                    gMaterialPositions[i] + new Vector2(0, 1),
                });

                if (CheckAccessToSide(vertices) && !_pathSets.ContainsKey(gMaterialPositions[i]))
                {
                    _pathSets.Add(gMaterialPositions[i], vertices.GetRange(0, vertices.Count));

                    vertices.Clear();

                    continue;
                }

                if (!CheckAccessToSide(vertices) && _pathSets.ContainsKey(gMaterialPositions[i]))
                {
                    _pathSets.Remove(gMaterialPositions[i]);
                }

                vertices.Clear();
            }
        }

        private bool CheckAccessToSide(IReadOnlyList<Vector2> vertices)
        {
            return !Builder.HasGMaterial(vertices[0] + new Vector2(-1, 0)) ||
                   !Builder.HasGMaterial(vertices[0] + new Vector2(0, -1)) ||
                   !Builder.HasGMaterial(vertices[1]) ||
                   !Builder.HasGMaterial(vertices[3]);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollided?.Invoke(this, collision);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnTriggered?.Invoke(this, collider);
        }
    }
}