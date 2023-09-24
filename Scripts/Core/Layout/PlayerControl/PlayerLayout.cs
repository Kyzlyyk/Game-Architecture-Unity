using UnityEngine;
using Core.PlayerControl;
using System.Collections.Generic;
using Kyzlyk.Core;
using Kyzlyk.Collections.Extensions;
using Kyzlyk.Attributes;

namespace Core.Layout.PlayerControl
{
    public sealed class PlayerLayout : Singleton<PlayerLayout>, ILayoutModule, IInitializableLayoutPart
    {
        [SerializeField] private Controller _controller;

        [RequireInterface(typeof(IPlayerConstructor)), SerializeField]
        private Object _playerConstructor;
        private IPlayerConstructor PlayerConstructor => (IPlayerConstructor)_playerConstructor;

        public Controller Controller => _controller;

        public IReadOnlyList<IReadOnlyList<Player>> Groups => _playerGroups;
        public IReadOnlyList<Player> MainGroup => _playerGroups[_main.GroupIndex];
        
        public int ActivedPlayerCount { get; private set; }
        public int MainGroupIndex => _main.GroupIndex;
        public int MainIndex => MainGroup.IndexOf(Main, Main);

        public Player Main => _main;
        private Player _main;

        private Player[][] _playerGroups;

        void ILayoutModule.Draw()
        {
            PlayerConstructor.Spawn(out _main, out Player[][] playerGroups);

            ActivedPlayerCount = playerGroups.Reduce((group, count) => ++count, 0);
            _playerGroups = playerGroups;
        }

        public void OnLayoutAssembled()
        {
            _controller.Init();
        }
    }
}