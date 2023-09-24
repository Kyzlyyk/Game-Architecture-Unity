using GameEnvironment.Stream;
using System;

namespace Core.Layout.PlayerControl
{
    public sealed class PlayerAtlas : LayoutAtlas
    {
        public PlayerAtlas(Mode mode) : base(mode) { }

        public PlayerAtlas(PlayerAtlas atlas)
            : base(atlas.Mode)
        {
            _controlledSpawners = atlas.ControlledSpawners;
            _uncontrolledSpawners = atlas.UncontrolledSpawners;
            _mainPlayerSpawner = atlas.MainSpawner;
        }

        internal PlayerAtlas(SerializablePosition mainSpawner, SerializablePosition[] controlledSpawners, SerializablePosition[] uncontrolledSpawners, Mode mode)
            : base(mode)
        {
            _mainPlayerSpawner = mainSpawner;
            _controlledSpawners = controlledSpawners;
            _uncontrolledSpawners = uncontrolledSpawners;
        }

        public SerializablePosition MainSpawner => _mainPlayerSpawner;
        public SerializablePosition[] ControlledSpawners => _controlledSpawners;
        public SerializablePosition[] UncontrolledSpawners => _uncontrolledSpawners;
        
        private SerializablePosition _mainPlayerSpawner;
        private SerializablePosition[] _controlledSpawners;
        private SerializablePosition[] _uncontrolledSpawners;

        public override void UnpackMode()
        {
            if (SaveService.TryLoadData(this, out SaveObject save))
            {
                _mainPlayerSpawner = save.MainPlayerSpawner;
                _controlledSpawners = save.ControlledSpawners;
                _uncontrolledSpawners = save.UncontrolledSpawners;
            }
        }

        public override void PackMode()
        {
            SaveService.SaveData(this, new SaveObject()
            {
                MainPlayerSpawner = _mainPlayerSpawner,
                ControlledSpawners = _controlledSpawners,
                UncontrolledSpawners = _uncontrolledSpawners
            });
        }

        [Serializable]
        private class SaveObject
        {
            public SerializablePosition MainPlayerSpawner;
            public SerializablePosition[] ControlledSpawners;
            public SerializablePosition[] UncontrolledSpawners;
        }
    }
}