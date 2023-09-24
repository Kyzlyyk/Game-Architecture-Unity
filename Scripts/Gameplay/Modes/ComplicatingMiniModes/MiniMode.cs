using System;
using UnityEngine;

namespace Gameplay.Modes.Mini
{
    public abstract class MiniMode : MonoBehaviour
    {
        [SerializeField] private float _duration;
        public float Duration => _duration;
        
        public abstract event EventHandler OnEndTransitionEnded;
 
        public abstract void Begin();
        public abstract void End();
    }
}
