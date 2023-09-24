using GameEnvironment.Stream;
using Kyzlyk.Enviroment.SaveSystem;
using System;
using UnityEngine;

namespace Core.Layout
{
    public abstract class LayoutAtlas : ISaveable
    {
        public LayoutAtlas(Mode mode)
        {
            _mode = mode;
            _saveService = new BinarySaveService();//new AssetBundleSaveService();
        }

        public string SaveKey => _mode.name + _mode.GetLoadedMap() + GetType().Name;
        public ISaveService SaveService => _saveService;
        private readonly ISaveService _saveService; 

        public Mode Mode => _mode;
        private readonly Mode _mode;

        public abstract void UnpackMode();
        public abstract void PackMode();
    }

    [Serializable]
    public struct SerializablePosition
    {
        public SerializablePosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public static implicit operator Vector2Int(SerializablePosition position) => new Vector2Int(position.X, position.Y);
        public static implicit operator SerializablePosition(Vector2Int vector) => new SerializablePosition(vector.x, vector.y);
    }
}
