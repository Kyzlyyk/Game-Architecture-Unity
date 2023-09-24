using Core.Layout.Space;
using Core.Layout.Design;
using Core.Layout.Interface;
using Core.Layout.PlayerControl;
using Core.Layout.Interactables;
using UnityEngine;
using Kyzlyk.Helpers.Utils;
using System.Collections;
using Kyzlyk.Core;

namespace Core.Layout
{
	public sealed class GLayout : Singleton<GLayout>, ILayoutModule
	{
		[SerializeField] private Controller _mainController;
		
		[Header("Space")]
		[SerializeField] private SpaceLayout _spaceModule;

		[Header("Player Layout")]
		[SerializeField] private PlayerLayout _playerModule;

		[Header("Interactables Layout")]
		[SerializeField] private InteractablesLayout _interactablesModule;

		[Header("Design Layout")]
		[SerializeField] private DesignLayout _designModule;

		[Header("Interface Layout")]
		[SerializeField] private HUD _HUD;

		public Controller Controller => _mainController;
		
		private ILayoutModule[] _modules;

		private int _loadedModulesCount;

        private void Awake()
        {
            _modules = new ILayoutModule[]
			{
				_HUD,
				_playerModule, 
				_spaceModule,
				_interactablesModule,
				_designModule,
			};

            Preload();
        }

        private void Preload()
		{
			for (int i = 0; i < _modules.Length; i++)
			{
				if (_modules[i] is IDelayedLoader loader)
				{
                    loader.OnLoaded += LoaderModule_OnLoaded;
					loader.StartLoad();

					continue;
				}

                _loadedModulesCount++;
            }
        }

        private void LoaderModule_OnLoaded(object sender, System.EventArgs e)
        {
			_loadedModulesCount++;
			((IDelayedLoader)sender).OnLoaded -= LoaderModule_OnLoaded;
        }

        public void Draw()
		{
			StartCoroutine(Draw_Coroutine());
		}

		private IEnumerator Draw_Coroutine()
		{
			yield return new WaitUntil(() => _loadedModulesCount == _modules.Length);

            for (int i = 0; i < _modules.Length; i++)
            {
                _modules[i].Draw();
            }

            var layoutParts = UnityUtility.GetAllObjectsOnScene<IInitializableLayoutPart>();

            _mainController.Init();
            
            for (int i = 0; i < layoutParts.Count; i++)
            {
                layoutParts[i].OnLayoutAssembled();
            }
        }

		public void Quit()
		{
			for (int i = 0; i < _modules.Length; i++)
			{
				if (_modules[i] is IDelayedLoader loader)
					loader.Dispose();
			}
		}
	}

	public interface IInitializableLayoutPart
	{
		void OnLayoutAssembled();
	}
}