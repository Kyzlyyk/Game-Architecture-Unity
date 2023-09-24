using Core.Layout.Design.BackgroundComposing;
using UnityEngine;
using Core.Layout.Design.PresetComposing;
using Kyzlyk.Helpers.Utils;
using System.Linq;
using Kyzlyk.Core;
using GameEnvironment.Stream;
using UnityEngine.ResourceManagement.AsyncOperations;
using Kyzlyk.Attributes;

namespace Core.Layout.Design
{
    public sealed class DesignLayout : Singleton<DesignLayout>, ILayoutModule, IDelayedLoader, IInitializableLayoutPart
    {
        [SerializeField] private Controller _controller;

        [Header("Global Presets")]
        [SerializeField] private bool _enableCustomMaterial;
        [SerializeField] private Preset _currentStyle;

        [Header("Constructors")]
        [RequireInterface(typeof(IBackgroundConstructor)), SerializeField]
        private Object _backgroundConstructor;
        [RequireInterface(typeof(IPresetConstructor)), SerializeField]
        private Object _presetConstructor;
        
        private IPreseter[] _preseters;

        private AssetLoader _assetProvider;

        private Material _cachedGlobalMaterial;

        public Controller Controller => _controller;

        public event System.EventHandler OnLoaded;

        void ILayoutModule.Draw()
        {
        }

        public void OnLayoutAssembled()
        {
            _preseters = UnityUtility.GetAllObjectsOnScene<IPreseter>().ToArray();
            
            Preset preset = new(_cachedGlobalMaterial);

            for (int i = 0; i < _preseters.Length; i++)
            {
                _preseters[i].Apply(preset);
            }
        }

        public void Dispose()
        {
            _assetProvider.Dispose();
        }

        async void IDelayedLoader.StartLoad()
        {
            _assetProvider = new AssetLoader();
            DesignAtlas atlas = new(GStream.Instance.GetActiveMode());
            atlas.UnpackMode();

            if (_assetProvider.TryGetAsset(atlas, out AsyncOperationHandle<Material> operation))
            {
                _cachedGlobalMaterial = await operation.Task;
            }
            
            OnLoaded(this, System.EventArgs.Empty);
        }
    }
}