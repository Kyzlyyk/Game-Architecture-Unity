#if UNITY_EDITOR

using GameEnvironment.Stream;
using Kyzlyk.Helpers.Extensions;
using UnityEngine;

namespace Core.Layout
{
    public abstract class Editor : MonoBehaviour
    {
        [SerializeField] private bool _isEditing;
        [Space]
        [SerializeField] private Mode _mode;

        protected bool IsEditing => _isEditing;
        protected Mode Mode => _mode;

        private Camera _camera;
        protected Camera Camera => _camera;

        protected Vector2Int SelectorPosition { get; private set; }

        private void Awake()
        {
            _camera = Camera.main;
            Init();
        }
        
        protected void SetSelectorPositionWithMouse()
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint2D(Input.mousePosition);
            SelectorPosition = Vector2Int.FloorToInt(mousePosition);
        }

        protected virtual void Init() { }

        public abstract void Draw();
        public abstract void Clear();
        public abstract void Save();
    }
}

#endif