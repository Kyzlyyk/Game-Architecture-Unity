using Core.Backstage;
using Core.PlayerControl;
using Kyzlyk.Core;
using Kyzlyk.Debugging.Annotations;
using UnityEngine;

namespace GameEnvironment.Stream
{
    internal sealed class GStream : Singleton<GStream>, ICoroutineExecutor
    {
        public static ICoroutineExecutor CoroutineExecutor => Instance;

        [Header("Backstage settings")]
        [SerializeField] private BackstageModule _afterQuitModeBackstage;

        [Header("Mode settings")]
        [TempForTest][SerializeField] private Mode _startModeTest;
        [SerializeField] private GameObject _adjusterPrefab;

        protected override bool IsPersistance => true;

        private Mode _activeMode;

        private CustomSceneManager _sceneManager;

        private void OnValidate()
        {
            _activeMode = _startModeTest;
        }

        protected override void OnAwake()
        {
            _sceneManager = new CustomSceneManager();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitFromMode();
            }
        }

        public async void LoadMode(Mode mode)
        {
            _activeMode = mode;
            Adapter<(PlayerProperties, PlayerProperties)>.Process(_afterQuitModeBackstage, _activeMode); 
            await _sceneManager.SwitchSceneAsync(mode.name);
            mode.LoadMap("Test");
        }

        public async void ExitFromMode()
        {
            _activeMode.Quit();
            await _sceneManager.SwitchSceneAsync(_afterQuitModeBackstage.SceneName);
            _activeMode = null;
        }

        public Mode GetActiveMode()
        {
            return _activeMode;
        }
    }
}