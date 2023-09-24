using Core.PlayerControl;
using GameEnvironment.Stream;
using UnityEngine;

namespace Core.Backstage
{
    internal class BackstageModule : ScriptableObject, ITransitor<(PlayerProperties, PlayerProperties)>
    {
        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;

        private PlayerProperties _mainGroupProperties;
        private PlayerProperties _enemyGroupProperties;

        public (PlayerProperties, PlayerProperties) Transit()
        {
            return (_mainGroupProperties, _enemyGroupProperties);
        }
    }
}