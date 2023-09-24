using System;
using UnityEngine;
using Core.Building;

namespace Gameplay.Modes.Mini
{
    public sealed class Reverse : MiniMode
    {
        [SerializeField] private Builder _builder;

        public override event EventHandler OnEndTransitionEnded;

        public override void Begin()
        {
            _builder.gameObject.SetActive(false);
        }

        public override void End()
        {
            _builder.gameObject.SetActive(true);
            OnEndTransitionEnded(this, EventArgs.Empty);
        }
    }
}