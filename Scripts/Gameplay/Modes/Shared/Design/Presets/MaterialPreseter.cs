using Core.Layout.Design.PresetComposing;
using UnityEngine;

namespace Gameplay.Layout.Design.Preseters
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialPreseter : MonoBehaviour, IPreseter
    {
        [SerializeField] private bool _applyNull;

        public int Layer => gameObject.layer;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Apply(Preset preset)
        {
            bool valueIsNull = preset.Material == null;

            if ((_applyNull && valueIsNull) || !valueIsNull)
            {
                _renderer.sharedMaterial = preset.Material;
            }
        }
    }
}