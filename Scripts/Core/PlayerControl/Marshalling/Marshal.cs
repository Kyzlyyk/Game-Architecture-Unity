using System;
using UnityEngine;
using Core.PlayerControl.Lab;
using System.Collections.Generic;
using Kyzlyk.Collections.Extensions;

namespace Core.PlayerControl.Marshalling
{
    internal abstract class Marshal<TCover, TExecutor> : ScriptableObject where TCover : IAbilityCover<TExecutor> where TExecutor : IAbilityExecutor
    {
        [SerializeField] protected TCover[] _allItems;
        [SerializeField] protected TCover[] _defaultItems;

        public IReadOnlyList<TCover> AllCovers => _allItems;
        public IReadOnlyList<TCover> CraftedCovers => _craftedItems;
        public IReadOnlyList<TCover> EquipedCovers => _equipedItems;

        protected readonly List<TCover> _craftedItems = new();
        protected readonly List<TCover> _equipedItems = new();

        internal delegate void CraftHandler(object sender, CraftingEventArgs args);
        internal event CraftHandler OnDetailCrafted;
        
        protected virtual void Awake()
        {
            InitializateDefaultItems();
            Start();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                InitializateDefaultItems();
                Start();
            }
#endif
            if (_allItems != null)
                InitializeItems();
        }

        protected virtual void Start()
        {
        }

        public TExecutor[] ConvertCoversToExecutors()
            => AllCovers.Convert(cover => cover.DirectExecutor);
        
        public TExecutor[] ConvertCraftedCoversToExecutors()
            => CraftedCovers.Convert(cover => cover.DirectExecutor);
        
        public TExecutor[] ConvertEquipedCoversToExecutors()
            => EquipedCovers.Convert(cover => cover.DirectExecutor);

        public virtual List<TExecutor> Filter(byte rareCoeficientRange1, byte rareCoeficientRange2)
        {
            List<TExecutor> items = new();

            for (int i = 0; i < AllCovers.Count; i++)
            {
                if (Kyzlyk.Helpers.Range.Contains(rareCoeficientRange1, rareCoeficientRange2, AllCovers[i].Rare.Coeficient))
                    items.Add(AllCovers[i].DirectExecutor);
            }

            return items;
        }

        public List<TExecutor> Filter(Rare rareRange1, Rare rareRange2)
        {
            return Filter(rareRange1.Coeficient, rareRange2.Coeficient);
        }

        protected virtual void InitializateDefaultItems()
        {
            _craftedItems.AddRange(_defaultItems);
        }

        internal void Craft_Internal(TCover item)
        {
            CraftItem(item, true);
        }

        protected void CraftItem(TCover item, bool notify)
        {
            CraftItemInternal(item);
            
            if (notify)
                OnDetailCrafted?.Invoke(this, new CraftingEventArgs(item));
        }
        
        protected virtual void CraftItemInternal(TCover item)
        {
            if (_craftedItems.Contains(item)) return;

            _craftedItems.Add(item);
        }

        internal void Equip_Internal(TCover item)
        {
            if (_craftedItems.Contains(item))
            {
                _equipedItems.Add(item);
            }
        }

        internal void Unequip_Internal(TCover item)
        {
            _equipedItems.Remove(item);
        }

        protected abstract void InitializeItems();

#if UNITY_EDITOR
        [ContextMenu("Delete save data in all details")]
        protected void DeleteSaveInAllDetails()
        {
            foreach (TCover item in _allItems)
            {
                item.SaveService.DeleteData(item);
            }
        }
#endif

        internal sealed class CraftingEventArgs : EventArgs
        {
            public CraftingEventArgs(TCover item)
            {
                Item = item;
            }

            public TCover Item { get; }
        }
    }
}