using System;
using UnityEngine;

namespace Core.PlayerControl.Lab.ShockWaves
{
    public enum HandleMode
    {
        TouchHandle = 1,
        IgnoreSurface = 2,
        IgnoreSurfaceAndTouchHandle = TouchHandle | IgnoreSurface,
        None = 0
    }

    public enum ShellType
    {
        Point,
        CustomShape,
        Composite,
    }

    public abstract class Shell : MonoBehaviour
    {
        public abstract event EventHandler<Vector2> OnTouched;
        
        private HandleMode _handleMode;
        public HandleMode HandleMode
        {
            get => _handleMode;

            set
            {
                if (value == HandleMode.IgnoreSurface || value == HandleMode.IgnoreSurfaceAndTouchHandle)
                    SetCollisionDetectionWithLayer(true);

                else if (_handleMode == HandleMode.IgnoreSurface || _handleMode == HandleMode.IgnoreSurfaceAndTouchHandle)
                    SetCollisionDetectionWithLayer(false);

                _handleMode = value;
            }
        }

        public int IgnoreSurfaceLayer { get; set; } = SharedConstants.GMaterialLayerInt;
        public ShellType Type { get; set; }
        public Bounds[] Composite { get; set; }

        #region inspector
        [SerializeField] private Vector2[] _shape;
        public Vector2[] Shape
        {
            get => _shape;
            set => _shape = value;
        }
        #endregion

        public void SetBoundaryShape(Bounds bounds)
        {
            Vector2 offset = bounds.center;

            _shape = new Vector2[]
            {
                new Vector2(0, 0) + offset,
                new Vector2(1, 0) * bounds.size.x + offset,
                new Vector2(1, 1) * bounds.size + offset,
                new Vector2(0, 1) * bounds.size.y + offset
            };
        }

        private void SetCollisionDetectionWithLayer(bool ignore)
        {
            Physics2D.
                IgnoreLayerCollision(transform.gameObject.layer, IgnoreSurfaceLayer, ignore);
        }
    }
}