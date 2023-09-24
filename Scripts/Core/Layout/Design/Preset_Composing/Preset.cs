using UnityEngine;

namespace Core.Layout.Design.PresetComposing
{
    public sealed class Preset
    {
        public Preset(Material material)
        {
            Material = material;
        }
        
        public Material Material { get; }
    }
}