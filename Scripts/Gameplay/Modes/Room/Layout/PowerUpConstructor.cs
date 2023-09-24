using UnityEngine;
using Core.Layout.Interactables;

namespace Gameplay.Modes.Room.Layout
{
    [CreateAssetMenu(menuName = "Layout/Constructors/PowerUp")]
    public class PowerUpConstructor : ScriptableObject, IPowerUpConstructor
    {
        public IPowerUpConstructor AddBuffs()
        {
            return this;
        }

        public IPowerUpConstructor AddDebuffs()
        {
            return this;
        }

        public IPowerUpConstructor AddTraps()
        {
            return this;
        }
    }
}