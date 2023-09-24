using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEnvironment.Stream
{
    public sealed class CustomSceneManager
    {
        private TaskCompletionSource<bool> _isModeLoaded;

        public async Task SwitchSceneAsync(string sceneName)
        {
            _isModeLoaded = new TaskCompletionSource<bool>();
            SceneManager.LoadSceneAsync(sceneName).completed += SceneManager_OnModeLoaded;
            
            await _isModeLoaded.Task;
        }

        private void SceneManager_OnModeLoaded(AsyncOperation obj)
        {
            obj.completed -= SceneManager_OnModeLoaded;
            _isModeLoaded.SetResult(true);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}