using System;
using UnityEngine;
using UnityEditor;
using Kyzlyk.Helpers.Utils;
using Core.Layout.Design.PresetComposing;

namespace Gameplay.Layout.Design.Preseters
{
    [RequireComponent(typeof(Light))]
    public sealed class BacklightPreseter : MonoBehaviour, IPreseter
    {
        [SerializeField] private bool _isRandomColor;
        [SerializeField, HideInInspector] private Color _color;

        [SerializeField] private bool _isColorShimmer;

        private Light _light;

        public int Layer => gameObject.layer;

        public Material Material { get; set; }

        public void Apply(Preset style)
        {
            //TODO
        }

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void Start()
        {
            if (_isRandomColor)
            {
                _light.color = UnityUtility.GetRandomColor();
            }
        }

        [Serializable]
        private struct Rangef
        {
            [SerializeField] float _start;
            [SerializeField] float _end;

            public float Start => _start;
            public float End => _end;

            public float Random()
            {
                return UnityEngine.Random.Range(Start, End);
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(BacklightPreseter))]
        public sealed class BacklightInspector : Editor
        {
            private SerializedProperty _color;
            private SerializedProperty _isRandomColor;

            private void OnEnable()
            {
                _color = serializedObject.FindProperty(nameof(_color));
                _isRandomColor = serializedObject.FindProperty(nameof(_isRandomColor));
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                serializedObject.Update();

                if (!_isRandomColor.boolValue)
                {
                    
                    EditorGUILayout.PropertyField(_color);
                }

                serializedObject.ApplyModifiedProperties();
            }
        }

#endif
    }
}