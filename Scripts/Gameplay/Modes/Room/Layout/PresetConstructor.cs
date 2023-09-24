using UnityEngine;
using Kyzlyk.Helpers.Utils;
using Core.Layout.Design.PresetComposing;
using Kyzlyk.Collections.Extensions;

namespace Gameplay.Modes.Room.Layout
{
    [CreateAssetMenu(menuName = "Layout/Constructors/Preset")]
    public class PresetConstructor : ScriptableObject, IPresetConstructor
    {
        [Header("Global")]
        [SerializeField] private Material _globalMaterial;
        
        [SerializeField] private LayerMask _maskPresets;

        protected IPreseter[] Preseters { get; private set; }
        
        public void ApplyMaskPresets(Preset presetStyle)
        {
            Preseters.ForEach(p =>
            {
                if (UnityUtility.IsLayerIncluded(_maskPresets, p.Layer))
                    p.Apply(presetStyle);
            });
        }
    }
}