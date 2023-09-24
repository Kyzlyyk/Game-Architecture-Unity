using UnityEngine;
using Core.PlayerControl.Marshalling;
using System.Collections.Generic;
using Kyzlyk.Helpers;

namespace Core.PlayerControl.Lab.Details
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_Details + "/Marshal")]
    internal sealed class DetailsMarshal : Marshal<DetailCard, Detail>
    {
        [Space]
        [SerializeField] private int _maxCombaters;
        [SerializeField] private int _maxAttackSupports;
        [SerializeField] private int _maxDefendSupports;
        [SerializeField] private int _maxCounterAttackers;

        private void OnValidate()
        {
            if (_defaultItems == null) return;

            for (int i = 0; i < _defaultItems.Length; i++)
            {
                if (!_defaultItems[i].IsCrafted)
                    _defaultItems[i].CraftMomentaly();
            }
        }

        protected override void CraftItemInternal(DetailCard item)
        {
            base.CraftItemInternal(item);
        }

        public override List<Detail> Filter(byte rareCoeficientRange1, byte rareCoeficientRange2)
        {
            List<Detail> details = new();

            bool TryAddMod<T>(Detail detail, ref int modCount, int maxModCount)
            {
                if (detail is T)
                {
                    if (modCount >= maxModCount)
                        return false;
                    
                    details.Add(detail);
                    modCount++;
                }

                return true;
            }
            
            int combaters = 0, defendSupports = 0, attackSupports = 0, counterAttackers = 0;
            for (int i = 0; i < AllCovers.Count; i++)
            {
                if (Range.Contains(rareCoeficientRange1, rareCoeficientRange2, AllCovers[i].Rare.Coeficient))
                {
                    Detail detail = (Detail)AllCovers[i].Executor;

                    if (TryAddMod<ICombater>(detail, ref combaters, _maxCombaters))
                        continue;

                    else if (TryAddMod<IAttackSupport>(detail, ref attackSupports, _maxAttackSupports))
                        continue;
                    
                    else if (TryAddMod<IDefenderSupport>(detail, ref defendSupports, _maxDefendSupports))
                        continue;

                    TryAddMod<ICounterAttacker>(detail, ref counterAttackers, _maxAttackSupports);
                }
            }

            return details;
        }

        protected override void InitializeItems()
        {
            for (int i = 0; i < _allItems.Length; i++)
            {
                if (_allItems[i].IsCrafted)
                    CraftItem(_allItems[i], false);
                else
                    _allItems[i].OnCrafted += Craft_Internal;
            }
        }
    }
}