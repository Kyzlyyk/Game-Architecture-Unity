using GameEnvironment.Stream;

namespace Core.Layout.Design
{
    public sealed class DesignAtlas : LayoutAtlas
    {
        public DesignAtlas(Mode mode) : base(mode)
        {
        }
        
        public DesignAtlas(string[] materialAssetNames, Mode mode) : base(mode)
        {
            _materialAssetNames = materialAssetNames;
        }

        public string[] MaterialAssetNames => _materialAssetNames;
        private string[] _materialAssetNames;

        public override void PackMode()
        {
            SaveService.SaveData(this, MaterialAssetNames);
        }

        public override void UnpackMode()
        {
            SaveService.TryLoadData(this, out _materialAssetNames, false);
        }
    }
}