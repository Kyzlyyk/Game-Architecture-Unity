using Kyzlyk.Helpers.Math;
using Kyzlyk.Helpers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.PlayerControl.AI
{
    public class Autopilot
    {
        public Autopilot(Player player)
        {
            Player = player;
            _targets = Array.Empty<ITarget>();
        }

        public bool IsTurnedOn { get; private set; }
        public Player Player { get; }

        public bool ToPickUpPowerUps { get; set; }
        public bool ToAvoidObstacles { get; set; }
        public Priority FocusedPriority { get; set; }

        protected IList<ITarget> _targets;
        protected ITarget _priorityTarget;

        protected virtual void SetTargets()
        {
            _targets = UnityUtility.GetAllObjectsOnScene<ITarget>(false);
            _targets.Remove(Player);
        }

        public void TurnOn()
        {
            if (IsTurnedOn)
                return;
            
            IsTurnedOn = true;
            OnTurnOn();
        }

        public void TurnOff()
        {
            if (!IsTurnedOn)
                return;
            
            IsTurnedOn = false;
            OnTurnOff();
        }

        protected virtual void OnTurnOn() { }
        protected virtual void OnTurnOff() { }

        public UnitVector GetDirection()
        {
            if (!IsTurnedOn)
                return UnitVector.Zero;

            SetTargets();
            _priorityTarget = FindClosestTarget().Target;

            return (UnitVector)_priorityTarget.Position;
        }

        private ITarget GetHighestPriorityTarget()
        {
            if (_targets.Count == 0)
                return null;

            ITarget highestPriorityTarget = _targets[0];
            for (int i = 1; i < _targets.Count; i++)
            {
                if (_targets[i].Priority > highestPriorityTarget.Priority)
                    highestPriorityTarget = _targets[i];
            }

            return highestPriorityTarget;
        }

        private ITarget GetPriorityTarget(Priority priority)
        {
            return _targets.First(t => t.Priority == priority);
        }

        private (ITarget Target, float Distance) FindClosestTarget()
        {
            if (_targets.Count == 0)
                return (null, 0f);

            ITarget closestTarget = _targets[0];
            float closestDistance = 0f;
            for (int i = 1; i < _targets.Count; i++)
            {
                float distance = Vector2.Distance(Player.Position, _targets[i].Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = _targets[i];
                }
            }

            return (closestTarget, closestDistance);
        }

        private bool TryFindWay(Vector2 destination, out Vector2[] way)
        {
            CircleSector sector =
                new(
                    360f,
                    1f,
                    Player.Position,
                    UnitVector.VectorToDeg360((UnitVector)destination)
                );

            way = new Vector2[2];
            return true;
        }
    }
}
