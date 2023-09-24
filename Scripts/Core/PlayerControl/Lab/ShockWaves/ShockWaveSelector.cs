using Core.PlayerControl.Core;
using Core.PlayerControl.Marshalling;
using Kyzlyk.Helpers;
using System.Collections;
using UnityEngine;

namespace Core.PlayerControl.Lab.ShockWaves
{
    public sealed class ShockWaveSelector : Selector<ShockWave>
    {
        public ShockWaveSelector(Player player, ShockWave[] shockWaves) 
            : base(player, shockWaves)
        {
        }

        private Coroutine _selectionUpdateCoroutine;

        public override void Activate()
        {
            _selectionUpdateCoroutine ??= Player.StartCoroutine(SelectionUpdate());
        }

        public override void Deactivate()
        {
            if (_selectionUpdateCoroutine != null)
            {
                Player.StopCoroutine(_selectionUpdateCoroutine);
                _selectionUpdateCoroutine = null;
            }
            
            ClosePanel();
        }

        private void ShowPanel()
        {
        }

        private void ClosePanel()
        {
        }

        private IEnumerator SelectionUpdate()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.S) && Executors.Count > 0)
                {
                    Use(Executors[0]);
                    ShowPanel();
                }
                
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ClosePanel();
                }

                yield return null;
            }
        }

        public override void OnAttack(IAbilityExecutor attackerExecutor, CoreBehaviour attackerBehaviour, ref UnsignPercent damage)
        {
        }
    }
}