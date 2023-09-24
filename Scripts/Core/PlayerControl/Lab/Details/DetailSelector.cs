using System.Collections;
using UnityEngine;
using Kyzlyk.Collections.Extensions;
using Core.PlayerControl.Core;
using Core.PlayerControl.Marshalling;
using System;
using Kyzlyk.Helpers;
using Kyzlyk.Core;
using System.Linq;

namespace Core.PlayerControl.Lab.Details
{
    public sealed class DetailSelector : Selector<Detail>
    {
        public DetailSelector(Player player, Detail[] details) 
            : base(player, details)
        {
            T[] GetModFromDetails<T>()
                => details
                .Where((detail) => detail is T)
                .Cast<T>()
                .ToArray();

            _combaters = GetModFromDetails<ICombater>();
            _attackSupports = GetModFromDetails<IAttackSupport>();
            _defenderSupports = GetModFromDetails<IDefenderSupport>();
            _counterAttackers = GetModFromDetails<ICounterAttacker>();

            details.ForEach(d => d.Init(player));
        }

        private Coroutine _selectionUpdateCoroutine;

        private readonly ICombater[] _combaters;
        private readonly IAttackSupport[] _attackSupports;
        private readonly IDefenderSupport[] _defenderSupports;
        private readonly ICounterAttacker[] _counterAttackers;

        private int _currentCounterAttackerIndex;

        private IAbilityExecutor _aggressorExecutor;
        private Player _aggressorPlayer;

        private ICoroutineExecutor CoroutineExecutor => Player;

        public override void Activate()
        {
            _selectionUpdateCoroutine ??= CoroutineExecutor.StartCoroutine(SelectionUpdate());
        }

        public override void Deactivate()
        {
            if (_selectionUpdateCoroutine != null)
            {
                CoroutineExecutor.StopCoroutine(_selectionUpdateCoroutine);
                _selectionUpdateCoroutine = null;
            }
        }

        private IEnumerator SelectionUpdate()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.D) && _combaters.Length > 0)
                {
                    Use(_combaters[0] as Detail);
                }
                
                yield return null;
            }
        }

        private void OnCounterAttackEnd()
        {
            if (_currentCounterAttackerIndex >= _counterAttackers.Length)
            {
                _currentCounterAttackerIndex = 0;

                _aggressorExecutor = null;
                _aggressorPlayer = null;

                return;
            }

            _currentCounterAttackerIndex++;
            CounterAttack();
        }

        private ICounterAttacker CounterAttack()
        {
            if (_currentCounterAttackerIndex >= _counterAttackers.Length) 
                return null;
            
            ICounterAttacker counterAttacker = _counterAttackers[_currentCounterAttackerIndex];
            counterAttacker.CounterAttack(_aggressorExecutor, _aggressorPlayer);
            
            return counterAttacker;
        }

        private void SupportAttack(ref UnsignPercent damage)
        {
            for (int i = 0; i < _attackSupports.Length; i++)
            {
                damage += _attackSupports[i].Support(Player.Behaviour);
            }
        }
        
        private void SupportDefend(ref UnsignPercent damage, IAbilityCover attackerInfo)
        {
            for (int i = 0; i < _attackSupports.Length; i++)
            {
                damage -= _defenderSupports[i].Support(Player.Behaviour, attackerInfo);
            }
        }

        protected override void OnExecuted(IAbilityExecutor detail, EventArgs args)
        {
            base.OnExecuted(detail, args);

            if (detail is ICounterAttacker)
                OnCounterAttackEnd();
        }

        protected override void OnInteracted(IAbilityExecutor detail, Player touchedPlayer)
        {
            base.OnInteracted(detail, touchedPlayer);

            UnsignPercent damage = UnsignPercent.Zero;

            if (detail is not ICounterAttacker)
                SupportAttack(ref damage);

            Player.Behaviour.Attack(touchedPlayer.Behaviour, detail, damage);
        }

        public override void OnAttack(IAbilityExecutor attackerExecutor, CoreBehaviour attackerBehaviour, ref UnsignPercent damage)
        {
            if (attackerExecutor is ICounterAttacker)
                return;
         
            SupportDefend(ref damage, attackerExecutor.Cover);
            
            if (attackerExecutor is ICounterAttackHandler attackHandler && attackHandler.Handle())
            {
                _aggressorExecutor = attackerExecutor;
                _aggressorPlayer = attackerBehaviour.Player;

                CounterAttack();
            }
        }
    }
}