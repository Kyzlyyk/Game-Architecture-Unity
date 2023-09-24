using GameEnvironment.Stream;
using System;
using UnityEngine;

namespace Core.Layout
{
    public abstract class Controller : ScriptableObject, IControllable
    {
        public bool IsActive { get; private set; }
        public bool IsReserved => _reservationKey != null;

        private Type _reservationKey;

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                IsActive = false;
        }
#endif

        public abstract void Init();

        public void Reserve(Type key)
        {
            if (_reservationKey == null)
                _reservationKey = key;
        }

        public void Unreserve(Type key)
        {
            if (_reservationKey == key)
                _reservationKey = null;
        }

        internal void UnreserveImmediately()
        {
            Unreserve(_reservationKey);
        }

        public void ReturnControl()
        {
            if (IsReserved)
                return;

            ReturnControlInternal();
        }

        public void Suspend()
        {
            if (!IsActive || IsReserved) return;

            IsActive = false;
            SuspendInternal();
        }

        public void Continue()
        {
            if (IsActive || IsReserved) return;

            IsActive = true;
            ContinueInternal();
        }

        protected abstract void SuspendInternal();
        protected abstract void ContinueInternal();
        protected abstract void ReturnControlInternal();
    }
}