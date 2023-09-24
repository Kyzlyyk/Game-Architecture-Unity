using Kyzlyk.Attributes;
using Kyzlyk.Collections.Extensions;
using Kyzlyk.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Layout.Interactables
{
    public sealed class InteractablesLayout : Singleton<InteractablesLayout>, ILayoutModule
    {
        [SerializeField] private MonoBehaviour[] _interactables;
        [SerializeField] private Controller _controller;

        [RequireInterface(typeof(IPowerUpConstructor))]
        [SerializeField]
        private Object _powerUpConstructor;

        private IPowerUpConstructor PowerUpConstructor =>
            (IPowerUpConstructor)_powerUpConstructor;
        
        public IReadOnlyList<IInteractable> Interactables => _interactables0;
        public Controller Controller => _controller;

        private IInteractable[] _interactables0;

        private void OnValidate()
        {
            if (_interactables == null) return;

            for (int i = _interactables.Length - 1; i >= 0; i--)
            {
                if (!_interactables[i].TryGetComponent<IInteractable>(out var interactable))
                {
                    Debug.LogError($"One of the '{nameof(_interactables)}' objects is not inherited from '{nameof(IInteractable)}' interface!");
                    _interactables = _interactables.Remove(_interactables[i]);
                }
            }
        }

        void ILayoutModule.Draw()
        {
            _interactables0 = new IInteractable[_interactables.Length];
            for (int i = 0; i < _interactables.Length; i++)
            {
                _interactables0[i] = _interactables[i].GetComponent<IInteractable>();
            }
            _interactables = null;
            
            if (_powerUpConstructor != null)
            {
                PowerUpConstructor.AddBuffs().AddDebuffs();
            }
        }
    }
}
