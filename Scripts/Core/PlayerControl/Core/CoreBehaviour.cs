using GameEnvironment.Stream;
using Core.PlayerControl.Lab.ShockWaves;
using System.Linq;
using Kyzlyk.Core.Exceptions;
using Core.PlayerControl.Lab.Details;
using Core.PlayerControl.Lab;
using Kyzlyk.Helpers;
using Kyzlyk.Debugging.Debuggers;

namespace Core.PlayerControl.Core
{
    public abstract class CoreBehaviour : IBehaviourHandler, IStationStateSwitcher, IControllable
    {
        public CoreBehaviour(Player player, StatusParameter durability, ShockWaveSelector shockWaveSelector, DetailSelector detailSelector)
        {
            Player = player;
            player.Behaviour = this;

            Durability = durability;
            
            ShockWaveSelector = shockWaveSelector;
            DetailSelector = detailSelector;
        }
        
        private BaseState _currentState;
        private BaseState[] _allStates;

        public ShockWaveSelector ShockWaveSelector { get; }
        public DetailSelector DetailSelector { get; }

        public Player Player { get; }

        public bool IsActive { get; private set; }

        public StatusParameter Durability { get; }

        protected void AdjustStates(BaseState[] states, int startStateIndex)
        {
            _currentState = states[startStateIndex];
            _allStates = states;

            _currentState.Start();
        }

        public virtual void Suspend()
        {
            IsActive = false;
        }

        public virtual void Continue()
        {
            IsActive = true;
        }

        public void SwitchState<T>() where T : BaseState
        {
            _currentState.Stop();
            _currentState = _allStates.First(s => s.GetType() == typeof(T));
            _currentState.Start();
        }

        private void OnAttacked(IAbilityExecutor attackerExecutor, CoreBehaviour attackerBehaviour, UnsignPercent damage)
        {
            DetailSelector.OnAttack(attackerExecutor, attackerBehaviour, ref damage);
            ShockWaveSelector.OnAttack(attackerExecutor, attackerBehaviour, ref damage);

            Durability.SubtractStaticly(damage);
        }

        public void Attack(CoreBehaviour targetBehaviour, IAbilityExecutor executor, UnsignPercent damage)
        {
            targetBehaviour.OnAttacked(executor, this, damage);
        }

        public void OnCollidedWithPlayer(Player other)
        {
            if (MethodDebugger.IsObjectsNull(this, nameof(OnCollidedWithPlayer), MessageType.Error, nameof(other)))
                return;
            
            IdenticalException.CompareOrThrow(Player, other);

            _currentState.OnCollidedWithPlayer(other);
        }
    }
}