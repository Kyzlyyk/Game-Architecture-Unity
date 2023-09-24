using Core.PlayerControl.Marshalling;
using UnityEngine;

namespace Core.PlayerControl.Lab.ShockWaves
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_ShockWaves + "/Marshal")]
    internal sealed class ShockWavesMarshal : Marshal<ShockWaveCard, ShockWave>
    {
        protected override void InitializateDefaultItems()
        {
            base.InitializateDefaultItems();
            for (int i = 0; i < _defaultItems.Length; i++)
            {
                Equip_Internal(_defaultItems[i]);
            }
        }

        protected override void InitializeItems()
        {

        }
    }
}