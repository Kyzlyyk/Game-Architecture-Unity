#if UNITY_EDITOR
using UnityEngine;

namespace Core.Layout.Design 
{
    public class DesignEditor : Editor
    {
        [SerializeField] private Material _globalMaterial;

        public override void Clear()
        {
        }
        
        public override void Draw()
        {
        }
        
        [ContextMenu("Save")]
        public override void Save()
        {
            DesignAtlas atlas = new(new string[] { _globalMaterial.name }, Mode);
            atlas.PackMode();
        }
    }
}
#endif