using Kyzlyk.Core;
using UnityEngine;
using Core.PlayerControl.Marshalling;
using System;
using Kyzlyk.Helpers.Extensions;
using Kyzlyk.Collections.Extensions;

namespace Core.PlayerControl.Lab.Details
{
    public abstract class Detail : ScriptableObject, IAbilityExecutor
    {
        [SerializeField] private DetailCard _card;
        [Space]
        [SerializeField] private GameObject _shellPrefab;

        public IAbilityCover<IAbilityExecutor> Cover => _card;

        public abstract IInteractor Interactor { get; }

        public bool IsActive { get; private set; }

        public abstract event EventHandler<IAbilityExecutor, Player> OnInteracted;
        public abstract event EventHandler<IAbilityExecutor, EventArgs> OnExecuted;

        public abstract void Execute(Player anchorPlayer);

        public abstract void Init(Player player);

#if UNITY_EDITOR
        private void OnEnable()
        {
            int counter = 0; 
            Type[] interfaces = GetType().GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].GetInterface(nameof(IAbilityExecutor)) != null)
                    counter++;
            }

            string GetMessage(string clarification)
                => $"The '{GetType().Name}' Detail must have {clarification} one interface modificator: {string.Join(", ", interfaces.Convert(type => type.Name))}";
            
            if (counter > 1)
                throw new Exception(GetMessage("only"));
            
            if (counter == 0)
                throw new Exception(GetMessage("at least"));
        }
#endif

        protected GameObject CreateShell(Player player, params Type[] requireComponents)
        {
            _shellPrefab.RequireComponents(true, requireComponents);
             return Instantiate(_shellPrefab, player.transform.position, _shellPrefab.transform.rotation, player.transform);
        }
        
        protected GameObject GetShell(Player player)
        {
            return player.transform.Find(_shellPrefab.name + "(Clone)").gameObject;
        }
        
        protected T GetShellComponent<T>(Player player) where T : Component
        {
            return GetShell(player).GetComponent<T>();
        }

        public virtual void Continue()
        {
            IsActive = true;
        }

        public virtual void Suspend()
        {
            IsActive = false;
        }
    }
}