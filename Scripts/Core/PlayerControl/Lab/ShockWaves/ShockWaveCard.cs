using Kyzlyk.Enviroment.SaveSystem;
using UnityEngine;

namespace Core.PlayerControl.Lab.ShockWaves
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_ShockWaves + "/Card")]
    public sealed class ShockWaveCard : ScriptableObject, IAbilityCover<ShockWave>
    {
        [SerializeField] private ShockWave _shockWave;

        [Header("Appereance")]
        [SerializeField] private Rare _rare;
        [SerializeField] private Class _class;
        [SerializeField] private Sprite _icon;
        [SerializeField, TextArea] private string _description;

        public IAbilityExecutor Executor => _shockWave;
        
        public Rare Rare => _rare;
        public Class Class => _class;
        public Sprite Icon => _icon;
        public string Description => _description;

        public string SaveKey => name;
        public ISaveService SaveService => new BinarySaveService();

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (_rare == null)
            {
                Debug.LogError($"Field 'Rare' of the '{name}' ShockWave must be initialized!");
            }
        }
#endif
    }
}