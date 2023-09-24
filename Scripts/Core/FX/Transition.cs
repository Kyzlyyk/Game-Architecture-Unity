#nullable enable
using System;
using System.Collections;

namespace Core.FX.Animations
{
    public abstract class Transition : GAnimation
    {
        public Func<IEnumerator>? GetEnumeratorInFreezeZone;

        public override void Play()
        {
            Executor.StartCoroutine(Play_Coroutine());
        }

        private IEnumerator Play_Coroutine()
        {
            yield return Executor.StartCoroutine(Start());
            
            if (GetEnumeratorInFreezeZone != null)
                yield return Executor.StartCoroutine(GetEnumeratorInFreezeZone());
            
            yield return Executor.StartCoroutine(End());
        }

        protected abstract IEnumerator Start();       
        protected abstract IEnumerator End();       
    }
}