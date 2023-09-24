using GameEnvironment.Stream;
using System;

namespace Core.Layout.Space
{
    public sealed class SpaceAtlas : LayoutAtlas
    {
        public SpaceAtlas(Mode mode) : base(mode) { }

        internal SpaceAtlas(SerializablePosition[] space, Mode mode)
            : base(mode)
        {
            _space = space;
        }

        public SerializablePosition[] Space => _space;

        private SerializablePosition[] _space;

        public override void UnpackMode()
        {
            if (SaveService.TryLoadData(this, out SaveObject save))
            {
                _space = save.Space;
            }
        }

        public override void PackMode()
        {
            SaveService.SaveData(this, new SaveObject()
            {
                Space = _space,
            });
        }

        [Serializable]
        private class SaveObject
        {
            public SerializablePosition[] Space;
        }
    }
}