using Core.Building;
using Kyzlyk.Helpers.Extensions;
using System;
using UnityEngine;

namespace Core.PlayerControl.Lab.ShockWaves
{
    public class GMaterialShell : Shell
    {
        public Builder Builder { get; set; }

        public override event EventHandler<Vector2> OnTouched;
        public event EventHandler OnHandled;

        private void Update()
        {
            if (HandleMode is HandleMode.TouchHandle or HandleMode.IgnoreSurfaceAndTouchHandle)
            {
                if (Type == ShellType.Point)
                {
                    if (Builder.HasGMaterial(transform.position))
                    {
                        OnTouched?.Invoke(this, transform.position);
                    }
                }
                else if (Type == ShellType.CustomShape)
                {
                    ProcessShapeTouches();
                }
                else if (Type == ShellType.Composite)
                {
                    ProcessCompositeTouches();
                }
            }
        }

        private void ProcessCompositeTouches()
        {
            const int boundSides = 4;
            Vector2[] polygon = new Vector2[Composite.Length * boundSides];
            for (int i = 0; i < Composite.Length; i++)
            {
                Array.Copy(Composite[i].GetEdgePoints(), 0, polygon, (i + 1) * boundSides - 1, boundSides);
            }

            if (polygon == null)
                return;

            ProcessTouches(polygon);
        }

        private void ProcessShapeTouches()
        {
            ProcessTouches(Shape);
        }

        private void ProcessTouches(Vector2[] polygon)
        {
            bool anyTouched = false;
            Vector2 offset = (Vector2)transform.position;

            for (int i = 0, j = 1; i < polygon.Length; i++, j++)
            {
                if (j == polygon.Length)
                    j = 0;

                if (Builder.CheckWay(polygon[i] + offset, polygon[j] + offset,
                    out Vector2 contact))
                {
                    anyTouched = true;
                    OnTouched?.Invoke(this, contact);
                }
            }

            if (anyTouched)
                OnHandled?.Invoke(this, EventArgs.Empty);
        }
    }
}