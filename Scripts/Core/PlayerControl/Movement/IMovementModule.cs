using Kyzlyk.Helpers.Math;
using System;
using UnityEngine;

namespace Core.PlayerControl
{
    public interface IMovementModule : IMovementSettingsProvider
    {
        bool Auto { get; set;  }

        void MoveImmediately(UnitVector direction);
        bool CanReach(Vector2 destination);

        void TurnOn();
        void TurnOff();
    }

    public interface IMovementSettingsProvider
    {
        public event EventHandler OnBeforeStartingMove;
        public event EventHandler OnAfterStartingMove;
        public event EventHandler OnMovementEnd;

        public bool IsMoving { get; }

        UnitVector CurrentDirection { get; }
        UnitVector LastDirection { get; }
    }
}