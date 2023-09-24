using Core.PlayerControl.AI;
using Core.Layout.Interface;
using Kyzlyk.Helpers.Math;
using System;
using UnityEngine;

namespace Core.PlayerControl
{
    public sealed class RoomMovementModule : IMovementModule, IMovementSettingsProvider, IPlayerSystemPart
    {
        public RoomMovementModule(Thrower thrower, Autopilot autopilot, HUD HUD)
        {
            _thrower = thrower;
            _autopilot = autopilot;
            _HUD = HUD;

            _thrower.OnEndThrowed += Thrower_OnEndThrowed;
            Joystick.OnReleased += Joystick_OnReleased;

            TurnOff();
        }

        public UnitVector CurrentDirection => (UnitVector)_thrower.Velocity.normalized;
        public UnitVector LastDirection { get; private set; }

        public bool IsMoving => CurrentDirection != UnitVector.Zero;

        private bool _auto;
        public bool Auto
        {
            get => _auto;
            set
            {
                if (value && !_auto)
                {
                    Joystick.OnReleased -= Joystick_OnReleased;
                    _autopilot.TurnOn();
                }
                else if (!value && _auto)
                {
                    Joystick.OnReleased += Joystick_OnReleased;
                    _autopilot.TurnOff();
                }

                _auto = value;
            }
        }

        public event EventHandler OnBeforeStartingMove;
        public event EventHandler OnAfterStartingMove;
        public event EventHandler OnMovementEnd;

        private readonly HUD _HUD;
        private readonly Thrower _thrower;
        private readonly Autopilot _autopilot;

        private Joystick Joystick => _HUD.Joystick;

        private bool _pause;

        private void Joystick_OnReleased(object sender, JoystickEventArgs args)
        {
            if (_pause) return;

            if (args.Direction.Compare(UnitVector.Zero, 0.1f)) 
                return;

            Move(-args.Direction);
        }

        private void Thrower_OnEndThrowed(object sender, EventArgs e)
        {
            EndMove();
        }

        private void Move(UnitVector direction)
        {
            if (_pause) return;

            OnAfterStartingMove?.Invoke(this, EventArgs.Empty);
            _thrower.Throw(direction);
            OnBeforeStartingMove?.Invoke(this, EventArgs.Empty);

            _HUD.Lock<JoystickElement>();
        }

        private void EndMove()
        {
            LastDirection = CurrentDirection;
            OnMovementEnd?.Invoke(this, EventArgs.Empty);
        }

        public void TurnOn()
        {
            _pause = false;
         
            if (Auto)
                Move(_autopilot.GetDirection());
            else
            {
                _HUD.Unlock<JoystickElement>();
            }
        }

        public void TurnOff()
        {
            _pause = true;
        }

        public void MoveImmediately(UnitVector direction)
        {
            Move(direction);
        }

        public bool CanReach(Vector2 destination)
        {
            throw new NotImplementedException();
        }
    }
}
