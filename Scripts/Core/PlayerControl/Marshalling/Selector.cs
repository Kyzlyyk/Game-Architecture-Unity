using Core.PlayerControl.Core;
using Core.PlayerControl.Lab;
using Kyzlyk.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.PlayerControl.Marshalling
{
    public abstract class Selector<T> where T : IAbilityExecutor
    {
        public Selector(Player player, T[] executors)
        {
            Player = player;
            Executors = executors;
        }
        
        protected Selector(Player player, IAbilityExecutor[] executors)
        {
            Player = player;
            Executors = executors.Cast<T>().ToArray();
        }

        protected readonly Player Player;

        public IReadOnlyList<T> Executors { get; }
        
        protected void Use(T executor)
        {
            executor.OnInteracted += OnInteracted;
            executor.OnExecuted += OnExecuted;
            executor.Execute(Player);

            Deactivate();
            Player.Behaviour.Suspend();
            Player.Layout.Controller.Suspend();
            Player.Layout.Controller.Reserve(GetType());
        }

        protected virtual void OnExecuted(IAbilityExecutor executor, EventArgs args)
        {
            Player.Layout.Controller.Unreserve(GetType());
            Player.Layout.Controller.ReturnControl();
            executor.OnExecuted -= OnExecuted;
            executor.OnInteracted -= OnInteracted;
        }
        
        protected virtual void OnInteracted(IAbilityExecutor executor, Player touchedPlayer)
        {
        }

        public abstract void Activate();
        public abstract void Deactivate();

        public abstract void OnAttack(IAbilityExecutor attackerExecutor, CoreBehaviour attackerBehaviour, ref UnsignPercent damage);
    }
}