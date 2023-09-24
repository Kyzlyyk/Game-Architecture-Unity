using GameEnvironment.Stream;
using Kyzlyk.Core;
using UnityEngine;

namespace Core.FX.Animations
{
    public abstract class GAnimation : ScriptableObject
    {
        public abstract GEventHandler<GAnimation> OnPlayed { get; set; }
        protected ICoroutineExecutor Executor => GStream.CoroutineExecutor;
        public abstract void Play();
    }
}