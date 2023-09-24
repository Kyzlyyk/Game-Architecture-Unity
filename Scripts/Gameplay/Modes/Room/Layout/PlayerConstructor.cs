using Core.Layout.PlayerControl;
using Core.Layout.Interface;
using Core.Layout;
using Core.PlayerControl;
using Core.Layout.Interactables;
using Core.PlayerControl.Lab.ShockWaves;
using Kyzlyk.Helpers.Extensions;
using Kyzlyk.Core;
using Kyzlyk.Collections.Extensions;
using GameEnvironment.Stream;
using UnityEngine;
using Core.PlayerControl.Lab.Details;
using Core.PlayerControl.Core;

namespace Gameplay.Modes.Room.Layout
{
    [CreateAssetMenu(menuName = "Layout/Constructors/Player")]
    internal class PlayerConstructor : ScriptableObject, IPlayerConstructor
    {
        [Header("Global")]
        [SerializeField] protected DetailsMarshal DetailsMarshal;
        [SerializeField] protected ShockWavesMarshal ShockWavesMarshal;
        [Space]
        [SerializeField] private int _maxShockWavesForPlayer;
        [SerializeField] private int _maxDetailsForPlayer;

        [Header("Status")]
        [SerializeField] private int _maxHealthValue;
        [SerializeField] private int _defaultHealthValue;

        [Space]
        [Header("Main Player Set")]
        [SerializeField] protected Player _mainPlayerPrefab;
        
        [Header("Controlled Player Set")]
        [SerializeField] protected Player[] _controlledPlayerPrefabs;

        [Header("Uncontrolled Player Set")]
        [SerializeField] protected Player[] _uncontrolledPlayerPrefabs;

        public void Spawn(out Player main, out Player[][] playerGroups)
        {
            PlayerAtlas atlas = new(GStream.Instance.GetActiveMode());
            atlas.UnpackMode();

            // main player setup
            main = SpawnPlayer(_mainPlayerPrefab, 0, atlas.MainSpawner);
            AssemblePlayer(main, ShockWavesMarshal.ConvertEquipedCoversToExecutors(), DetailsMarshal.ConvertEquipedCoversToExecutors(), true);
            //

            // main group setup
            Mode mode = Singleton<GStream>.Instance.GetActiveMode();
            PlayerProperties mainProperties = mode.MainGroupProperties;

            Player[] players = SpawnPlayers(
                mainProperties.Count,
                0,
                _controlledPlayerPrefabs, 
                atlas.ControlledSpawners
            );

            AssemblePlayers(players, mainProperties, true);
            //

            // enemy groups setup
            PlayerProperties enemyProperties = mode.EnemyGroupProperties;

            Player[] enemies = SpawnPlayers(
                enemyProperties.Count, 
                1,
                _uncontrolledPlayerPrefabs, 
                atlas.UncontrolledSpawners
            );

            AssemblePlayers(enemies, enemyProperties, false);
            //
            
            playerGroups = new Player[2][]
            {
                players.AddLast(main),
                enemies,
            };
        }

        private void AssemblePlayer(Player player, ShockWave[] shockWaves, Detail[] details, bool isControlled)
        {
            player.Behaviour = new RoomCoreBehaviour(player, isControlled, 
                new StatusParameter(_maxHealthValue, _defaultHealthValue),
                new ShockWaveSelector(player, shockWaves),
                new DetailSelector(player, details));
        }

        private void AssemblePlayers(Player[] players, PlayerProperties properties, bool isControlled)
        {
            ShockWave[] shockWaves = ShockWavesMarshal
                .Filter(properties.RareRangeStart, properties.RareRangeEnd)
                .ToArray();
            
            Detail[] details = DetailsMarshal
                .Filter(properties.RareRangeStart, properties.RareRangeEnd)
                .ToArray();

            for (int i = 0; i < players.Length; i++)
            {
                AssemblePlayer
                    (
                        players[i], 
                        shockWaves.RandomizeArray(_maxShockWavesForPlayer, false), 
                        details.RandomizeArray(_maxDetailsForPlayer, false), 
                        isControlled
                    );
            }
        }

        private Player[] SpawnPlayers(int playersCount, byte group, Player[] prefabs, SerializablePosition[] positions)
        {
            Player[] players = new Player[playersCount];
            
            for (int i = 0; i < playersCount; i++)
            {
                players[i] = SpawnPlayer(prefabs.Random(), group, positions[i]);
            }

            return players;
        }

        private Player SpawnPlayer(Player prefab, byte group, SerializablePosition position)
        {
            Player player = Instantiate(prefab, new Vector3(position.X, position.Y), prefab.transform.rotation);

            Player target = Player.Create(player.gameObject, null, group, new RoomMovementModule
                (
                    player.TryAddComponent<Thrower>(),
                    new RoomAutopilot(player, Singleton<PlayerLayout>.Instance, Singleton<InteractablesLayout>.Instance),
                    Singleton<HUD>.Instance
                ));

            return target;
        }
    }
}