using UnityEngine;
using Core.PlayerControl;
using Kyzlyk.GSystems.InteractiveObjects.PowerUps;

namespace Gameplay.PowerUps
{
    public class PowerUp_Elasticity : PowerUp<Thrower>
    {
        [SerializeField] private ElasticitySettings _settings;

        private ElasticitySettings _previousSettings;

        protected override void Pickup()
        {
            _previousSettings = DirectTarget.ElasticitySettings;
            DirectTarget.ElasticitySettings = _settings;
        }

        protected override void ResetEffect()
        {
            DirectTarget.ElasticitySettings = _previousSettings;
        }
    }
}