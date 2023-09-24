using Core.PlayerControl.Core;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Core.PlayerControl.AI;
using Kyzlyk.Core;
using Core.Layout.PlayerControl;
using Gameplay.Layout.Interactable;
using Kyzlyk.Helpers.Extensions;

namespace Core.PlayerControl
{
    [RequireComponent(typeof(Thrower))]
    public class Player : MonoBehaviour, IEqualityComparer<Player>, ICapturer, ITarget, ICoroutineExecutor
    {
        public int ID { get; private set; }

        public byte GroupIndex { get; private set; }

        public PlayerLayout Layout => Singleton<PlayerLayout>.Instance;
        public Player Main => Layout.Main;

        public Vector2 Position => transform.position;
        public Priority Priority => Priority.Medium;
        public bool Hostile => true;

        public IMovementModule MovementModule { get; private set; }

        public CoreBehaviour Behaviour { get; set; }

        private void Awake()
        {
            ID = GetHashCode();

            IEnumerable<IPlayerModule> playerModules = GetComponents<MonoBehaviour>().OfType<IPlayerModule>();
            foreach (var module in playerModules)
            {
                module.Player = this;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == SharedConstants.PlayerLayerInt)
            {
                Behaviour.OnCollidedWithPlayer(collision.gameObject.GetComponent<Player>());
            }
        }

        public static Player Create(GameObject obj, CoreBehaviour behaviour, byte group, IMovementModule movementModule)
        {
            Player target = obj.TryAddComponent<Player>();
            
            target.GroupIndex = group;
            target.MovementModule = movementModule;
            target.Behaviour = behaviour;

            return target;
        }

        public bool Equals(Player x, Player y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Player obj)
        {
            return obj.ID;
        }
    }
}