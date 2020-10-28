using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        [Header("Item Type")]
        [SerializeField] ItemType itemType;
        [SerializeField] ItemEffect itemEffect;
        [SerializeField] StatsAtributte attributteToModify;
        [SerializeField] CharEquipmentSlot canEquipOn;

        [Header("Item Info")]
        [SerializeField] int id;
        [SerializeField] string itemName;
        [SerializeField][TextArea] string description;
        [SerializeField] int price;
        [SerializeField] Sprite sprite;
        [SerializeField] bool stackable = false;
        [SerializeField] bool timeEffectItem = false;

        [Header("Item Effect")]
        [SerializeField] int modifier;// how much heal or inflict damage 
        [SerializeField] bool isPercentage = false;//for certain amount of weapons to deal 30% more damage 
        [SerializeField] StatusCondition causeStatusCondition = StatusCondition.None;
        [SerializeField] TimeEffectController activateEffect;

        #region PUBLIC GETTER METHODS
        public bool IsStackable()
        {
            return stackable;
        }

        public bool IsTimeEffect()
        {
            return timeEffectItem;
        }
        public CharEquipmentSlot GetCanEquipOn()
        {
              return canEquipOn;
        }

        public string GetDisplayName()
        {
            return itemName;
        }

        public string GetDescription()
        {
            return description;
        }
        public Sprite GetIcon()
        {
            return sprite;
        }

        public int GetPrice()
        {
            return price;
        }

        public string GetTypeString()
        {
            switch (itemType)
            {
                case ItemType.Consumable: return "Consumable";
                case ItemType.BattleItem: return "Battle";
                case ItemType.Equipable: return "Equipable";
                case ItemType.NoEffect: return "None";
                case ItemType.TimeEffect: return "Timer";
                case ItemType.StatusCondition: return "Status Condition";
                default:return "None";
            }
        }

        public int GetItemId()
        {
            return id;
        }

        public ItemType GetItemType()
        {
            return itemType;
        }

        public ItemEffect GetItemEffect()
        {
            return itemEffect;
        }

        public StatsAtributte GetAttributeToModify()
        {
            return attributteToModify;
        }

        public StatusCondition GetStatusCondition()
        {
            return causeStatusCondition;
        }

        public int GetAdditiveModifiers()
        {
            if (!isPercentage)
            {
                return modifier;
            }
            else
            {
                return 0;
            }
            
        }

        public bool GetIsPercentage()
        {
            return isPercentage;
        }
        #endregion

        #region PUBLIC METHODS

        public void Use(BaseStats target)
        {
            if (itemType == ItemType.BattleItem)
            {
                ApplyBattleItemEffect(target);
            }
            else if (itemType == ItemType.Consumable)
            {
                ApplyConsumableItemEffect(target);
            }
            else if (itemType == ItemType.Equipable)
            {
                return;
            }
            else if (itemType == ItemType.TimeEffect)
            {
                ApplyTimeEffectItem();
            }
            else if (itemType == ItemType.StatusCondition)
            {
                ApplyStatusConditionEffect(target);
            }
            else if (itemType == ItemType.NoEffect)
            {
                Debug.Log("It's not an usable item");
            }
        }

        private void ApplyStatusConditionEffect(BaseStats target)
        {
            if (itemEffect == ItemEffect.HealStatusCondition)
            {
                // TEST IF THE ITEM HEAL THE SPECIFIED CONDITION
                //REMOVE STATUS CONDITION
            }
            else if (itemEffect == ItemEffect.InflictStatusCondition)
            {
                //TEST IF IS IN BATTLE
                // APPLY THE CONDITION ON TARGET
            }
        }

        private void ApplyTimeEffectItem()
        {
            if (itemEffect == ItemEffect.GainXP)
            {
                activateEffect.StartCoroutine(activateEffect.GainXPRoutine());
            }
            else if (itemEffect == ItemEffect.IncreaseMoney)
            {
                activateEffect.StartCoroutine(activateEffect.IncreaseMoneyRoutine());
            }
            else if (itemEffect == ItemEffect.RepelEnemy)
            {
                activateEffect.StartCoroutine(activateEffect.RepelEnemyRoutine());
            }
        }

        private void ApplyEquipableItemEffect(BaseStats target, ItemConfig item, CharEquipmentSlot equipedOn)
        {
            if (itemEffect == ItemEffect.IncreaseStatTemporarily)
            {
                if (target.GetEquippedItem(equipedOn) == item)
                {
                    target.ReturnAfterUnnequip(attributteToModify, item, equipedOn);
                }
                else
                {
                    target.ModifyStatByEquip(attributteToModify, modifier, item, equipedOn);
                }
            }
        }

        private void ApplyConsumableItemEffect(BaseStats target)
        {
            if (isPercentage)
            {
                int value = Mathf.FloorToInt((target.GetStat(attributteToModify) * modifier) / 100);
                target.ModifyStatPermanent(attributteToModify, value);
            }
            else
            {
                if(attributteToModify ==  StatsAtributte.Health || attributteToModify == StatsAtributte.SkillPoints)
                {
                    target.HealStat(attributteToModify, modifier);
                }
                else
                {
                    target.ModifyStatPermanent(attributteToModify, modifier);
                }
            }
        }

        private void ApplyBattleItemEffect(BaseStats target)
        {
            if (itemEffect == ItemEffect.HealStat)
            {
                target.ModifyStatPermanent(attributteToModify, modifier);
            }
            else if (itemEffect == ItemEffect.IncreaseStatTemporarily)
            {
                target.ModifyStatOnBattle(attributteToModify, modifier);
            }
            else if (itemEffect == ItemEffect.MoreDamageNextTurn)
            {
                //turn some variable on combat class to true
                //subscribe to some coroutine or action to turn that variable to false
            }
        }
        #endregion

    }
}



