using UnityEngine;

namespace Core.PlayerControl.AI
{
    public interface ITarget
    { 
        Vector2 Position { get; }
        Priority Priority { get; }
        bool Hostile { get; }
    }

    public enum Priority
    {
        Low = 1,
        Superior = 2,
        Medium = 3,
        High = 4,
        Total = 5
    }
}