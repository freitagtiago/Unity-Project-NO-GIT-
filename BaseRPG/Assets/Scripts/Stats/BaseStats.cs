using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Header("Level")]
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        private int currentLevel = 1;
        [SerializeField] private Progression progression = null;
        Experience experience;

        [Header("General")]
        [SerializeField] private CharacterTypes charType = CharacterTypes.Neutral;
        [SerializeField] private StatusCondition currentStatusCondition = StatusCondition.None;
        [SerializeField] private ItemConfig[] equippedItem; //0 Left Hand, 1 Book, 2 Right Hand
        public Action OnLevelUp;

        [SerializeField] private GameObject levelupParticles;
        // Start is called before the first frame update


        [Header("Stats")]
        Dictionary<Stats.StatsAtributte, StatData> statCollection;
        [Header("Skills")]
        [SerializeField] List<SkillConfig> availableSkills;

        private void Awake()
        {
            statCollection = new Dictionary<StatsAtributte, StatData>();
            experience = GetComponent<Experience>();
            equippedItem = new ItemConfig[3];
        }

        private void OnEnable()
        {
            if(experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if(experience != null)
            {
                experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
            FillStats();
        }

        // Update is called once per frame
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                LevelUpEffect();
                currentLevel = newLevel;
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelupParticles, transform);
        }
        private float GetBaseStat(StatsAtributte stat)
        {
            return progression.GetStat(stat, charType, GetLevel());
        }

        private void FillStats()
        {
            int[] valuesModified = new int[3] { 0, 0, 0 };
            /* TO DO Preencher com save ou progression em caso de inimigo*/
            statCollection.Add(StatsAtributte.Attack,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.Defense,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.ExperienceReward,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.ExperienceToLevelUp,new StatData(80, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.Health,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.Lucky,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.MagicAttack,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.MagicDefense,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.SkillPoints,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.Speed,new StatData(10, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.MaxHealth,new StatData(20, false, false, 0, valuesModified));
            statCollection.Add(StatsAtributte.MaxSkillPoints,new StatData(35, false, false, 0, valuesModified));
        }

        private void SetEquippedItem(CharEquipmentSlot slotToEquip, ItemConfig item)
        {
            switch (slotToEquip)
            {
                case CharEquipmentSlot.LeftHand: 
                    equippedItem[0] = item;
                    return;
                case CharEquipmentSlot.Book:
                    equippedItem[1] = item;
                    return;
                case CharEquipmentSlot.RightHand:
                    equippedItem[2] = item;
                    return;
            }
            return;
        }

        #region PUBLIC METHODS
        public CharacterTypes GetStatType()
        {
            return charType;
        }

        public ItemConfig GetEquippedItem(CharEquipmentSlot equippedOn)
        {
            switch (equippedOn)
            {
                case CharEquipmentSlot.LeftHand: return equippedItem[0];      
                case CharEquipmentSlot.Book: return equippedItem[1];      
                case CharEquipmentSlot.RightHand: return equippedItem[2];
                default: return equippedItem[0];
            }
        }

        public ItemConfig[] GetAllEquippedItem()
        {
            return equippedItem;
        }

        public int GetLevel()
        {
            return currentLevel;
        }

        public int CalculateLevel()
        {
            if (experience == null) return currentLevel;
            float currentXp = experience.GetExperience();
            int penultimateLevel = progression.GetLevels(StatsAtributte.ExperienceToLevelUp, charType);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(StatsAtributte.ExperienceToLevelUp, charType, level);
                if (xpToLevelUp > currentXp)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }

        public StatusCondition GetStatusCondition()
        {
            return currentStatusCondition;
        }

        public void TakeDamage(int amount)
        {
            statCollection[StatsAtributte.Health].value = statCollection[StatsAtributte.Health].value - amount;

            if (statCollection[StatsAtributte.Health].value < 0)
            {
                statCollection[StatsAtributte.Health].value = 0;
            }
        }

        public void ReduceSP(int amount)
        {
            statCollection[StatsAtributte.SkillPoints].value = statCollection[StatsAtributte.SkillPoints].value - amount;

            if (statCollection[StatsAtributte.SkillPoints].value < 0)
            {
                statCollection[StatsAtributte.SkillPoints].value = 0;
            }
        }

        public void ModifyStatOnBattle(StatsAtributte stat, int value)
        {
            if (statCollection.ContainsKey(stat))
            {
                statCollection[stat].value += value;
                statCollection[stat].isModifiedOnBattle = true;
                if(statCollection[stat].value < 0)
                {
                    statCollection[stat].value = 0;
                }
            }
            else
            {
                Debug.LogError("Stat "+ stat + " not found");
            }
        }

        public void ModifyStatByEquip(StatsAtributte stat, int value, ItemConfig item, CharEquipmentSlot equipOn)
        {
            if (statCollection.ContainsKey(stat))
            {
                statCollection[stat].value += value;
                statCollection[stat].isModifiedByItem = true;
                statCollection[stat].SetSpecificValueModifiedByItem(equipOn, value);
                SetEquippedItem(equipOn, item);
                if (statCollection[stat].value < 0)
                {
                    statCollection[stat].value = 0;
                }
            }
            else
            {
                Debug.LogError("Stat " + stat + " not found");
            }
        }

        public void ModifyStatPermanent(StatsAtributte stat, int value)
        {
            if (statCollection.ContainsKey(stat))
            {
                statCollection[stat].value += value;
                if (statCollection[stat].value < 0)
                {
                    statCollection[stat].value = 0;
                }
            }
            else
            {
                Debug.LogError("Stat " + stat + " not found");
            }
        }

        public void HealStat(StatsAtributte stat, int value)
        {
            if (statCollection.ContainsKey(stat))
            {
                if(stat == StatsAtributte.Health || stat == StatsAtributte.SkillPoints)
                {
                    statCollection[stat].value += value;
                   if(stat == StatsAtributte.Health && statCollection[stat].value > statCollection[StatsAtributte.MaxHealth].value)
                    {
                        statCollection[stat].value = statCollection[StatsAtributte.MaxHealth].value;
                    } else if(stat == StatsAtributte.SkillPoints && statCollection[stat].value > statCollection[StatsAtributte.MaxSkillPoints].value)
                    {
                        statCollection[stat].value = statCollection[StatsAtributte.MaxSkillPoints].value;
                    }
                }
            }
            else
            {
                Debug.LogError("Stat " + stat + " not found");
            }
        }

        public void ReturnStatFromBattle()
        {
            foreach(KeyValuePair<StatsAtributte,StatData> stat in statCollection)
            {
                if (stat.Value.isModifiedOnBattle)
                {
                    statCollection[stat.Key].value -= stat.Value.valueModifierOnBattle;
                    if(statCollection[stat.Key].value < 0)
                    {
                        statCollection[stat.Key].value = 0;
                    }
                }
            }
        }


        public void ReturnAfterUnnequip(StatsAtributte stat, ItemConfig item, CharEquipmentSlot equippedOn)
        {
            if (statCollection.ContainsKey(stat) && statCollection[stat].isModifiedByItem && GetEquippedItem(equippedOn) != null)
            {
                statCollection[stat].value -= statCollection[stat].GetSpecificValueModifiedByItem(equippedOn);
                statCollection[stat].isModifiedByItem = false;
                SetEquippedItem(equippedOn, null);
            }
        }

        public int GetStat(StatsAtributte stat)
        {
            if (statCollection.ContainsKey(stat))
            {
                return statCollection[stat].value;
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region StatData Class
        [System.Serializable]
        class StatData
        {
            public int value;
            public bool isModifiedOnBattle;
            public bool isModifiedByItem;
            public int valueModifierOnBattle;
            public int[] valueModifierByItem = new int[3];
            
            public StatData(int valueStat, bool valueIsModifiedOnBattle, bool valueIsModifiedByItem, int valueValueModifierOnBattle, int[] valueValueModifierByItem)
            {
                value = valueStat;
                isModifiedOnBattle = valueIsModifiedOnBattle;
                isModifiedByItem = valueIsModifiedByItem;
                valueModifierOnBattle = valueValueModifierOnBattle;
                valueModifierByItem = valueValueModifierByItem;
            }

            public int GetSpecificValueModifiedByItem(CharEquipmentSlot slot)
            {
                switch(slot)
                {
                    case CharEquipmentSlot.LeftHand:
                        return valueModifierByItem[0];
                    case CharEquipmentSlot.Book:
                        return valueModifierByItem[1];
                    case CharEquipmentSlot.RightHand:
                        return valueModifierByItem[2];
                    default: return 0;
                }
            }

            public void SetSpecificValueModifiedByItem(CharEquipmentSlot slot, int value)
            {
                switch (slot)
                {
                    case CharEquipmentSlot.LeftHand:
                        valueModifierByItem[0] = value;
                        break;
                    case CharEquipmentSlot.Book:
                        valueModifierByItem[1] = value;
                        break;
                    case CharEquipmentSlot.RightHand:
                        valueModifierByItem[2] = value;
                        break;
                    default:  break;
                }
            }
        }

        public List<SkillConfig> GetAllAvailableSkills()
        {
            return availableSkills;
        }
        #endregion
    }
}

