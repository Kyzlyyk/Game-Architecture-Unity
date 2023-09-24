using System.Collections.Generic;
using Kyzlyk.Core;
using GameEnvironment.Stream;

namespace Core.Layout.Space
{
    public abstract class SpaceLayout : Singleton<SpaceLayout>, ILayoutModule
    {
        [UnityEngine.SerializeField] private Controller _controller;

        public abstract Chunk Environment { get; }
        public Controller Controller => _controller;

        public void Draw()
        {
            Environment.LinkModuleDependencies();
            Mode mode = GStream.Instance.GetActiveMode();

            if (mode.IsCustomEditingMapsAllowed)
            {
                SpaceAtlas package = new(mode);
                package.UnpackMode();

                if (package.Space != null)
                {
                    for (int i = 0; i < package.Space.Length; i++)
                    {
                        Environment.Builder.CreateGMaterial(package.Space[i], default, false);
                    }
                    
                    Environment.Builder.Apply();
                    DrawWithCustom();
                    
                    return;
                }
            }
         
            DrawDefault();
        }

        protected virtual void DrawWithCustom() { }
        protected abstract void DrawDefault();
        public abstract IEnumerable<Chunk> GetAllChunks();
    }
}