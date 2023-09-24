using System;
using UnityEngine;
using Kyzlyk.Enviroment.SaveSystem;
using Kyzlyk.Attributes;

namespace Core.PlayerControl.Lab.Details
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_Details + "/Card")]
    public class DetailCard : ScriptableObject, IAbilityCover<Detail>
    {
        [SerializeField] private Detail _detail;

        [Space]
        [Header("Characteristics")]
        [SerializeField] private int _price;
        [SerializeField] private Rare _rare;
        [SerializeField] private Class _class;
        [SerializeField] private string _description;

        [Header("Appearance")]
        [SerializeField] private Sprite _icon;

        
        [Header("Crafting Info")]
        [SerializeField] private float _timeToCraftInMinutes;
        [ReadOnlyProperty][SerializeField] private int _craftingPercentage;

        public int CraftingCompletionPercentage => _craftingPercentage;
        public bool IsCrafted => _craftingPercentage >= 100;
        public bool IsCrafting { get; private set; }
        public bool AutoUpdatePercentage { get; set; } = true;
        
        public event Action<DetailCard> OnCrafted;

        public IAbilityExecutor Executor => _detail;
        
        public Rare Rare => _rare;
        public Class Class => _class;
        public Sprite Icon => _icon;

        private DateTime _startCraftingDate;

        public string SaveKey => GetType().Name + "-" + name;
        public ISaveService SaveService => new BinarySaveService();

        private void Awake()
        {
            LoadSavedData();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (_rare == null)
            {
                Debug.LogError($"Field 'Rare' of the '{name}' Detail must be initialized!");
            }

            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                LoadSavedData();
        }
#endif

        private void OnValidate()
        {
            _timeToCraftInMinutes = Mathf.Max(_timeToCraftInMinutes, .001f);
        }

        [ContextMenu("Craft")]
        public void StartCrafting()
        {
            if (!IsCrafting && !IsCrafted)
            {
                IsCrafting = true;
                _startCraftingDate = DateTime.UtcNow;
                SaveUtility.SaveDateTime(this, _startCraftingDate);
                SaveData();
            }
        }

        public void UpdateCraftingPercentage()
        {
            UpdateCraftingPercentage(true);
        }

        private void UpdateCraftingPercentage(bool notifyIfCrafted)
        {
            TimeSpan elapsed = DateTime.UtcNow - _startCraftingDate;
            float minutesElapsed = (float)Math.Clamp(elapsed.TotalMinutes, 0f, _timeToCraftInMinutes);

            _craftingPercentage = (int)(minutesElapsed / _timeToCraftInMinutes * 100f);

            if (IsCrafted)
                Craft(notifyIfCrafted);
        }

        public void LoadSavedData()
        {
            _startCraftingDate = SaveUtility.LoadDateTime(this, DateTime.UtcNow);

            if (SaveService.TryLoadData<SaveObject>(this, out var data))
            {
                IsCrafting = data.IsCrafting;
            }

            UpdateCraftingPercentage(notifyIfCrafted: false);
        }

        public void SaveData()
        {
            SaveService.SaveData(this, new SaveObject()
            {
                IsCrafting = this.IsCrafting,
            });
        }

        private void Craft(bool notify = true)
        {
            IsCrafting = false;
            SaveData();

            if (notify)
                OnCrafted?.Invoke(this);
        }

        [ContextMenu("Delete save")]
        public void DeleteSavedData()
        {
            _craftingPercentage = 0;
            IsCrafting = false;

            SaveService.DeleteData(this);
            SaveUtility.DeleteDateTime(this);
        }

        [ContextMenu("Craft momentaly")]
        internal void CraftMomentaly()
        {
            if (_startCraftingDate == default)
                _startCraftingDate = DateTime.UtcNow;

            TimeSpan timeToSubtract = TimeSpan.FromMinutes(_timeToCraftInMinutes);
            _startCraftingDate = _startCraftingDate.Subtract(timeToSubtract);
            
            SaveUtility.SaveDateTime(this, _startCraftingDate);
            UpdateCraftingPercentage();
        }

        [Serializable]
        private sealed class SaveObject
        {
            public bool IsCrafting;
        }
    }
}