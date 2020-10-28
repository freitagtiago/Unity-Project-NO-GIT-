using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Inventories
{
    public class Item : MonoBehaviour
    {
        [Header("Item Type")]
        [SerializeField] ItemConfig item;
        [SerializeField] TimeEffectController activateEffect;
        

        /*[Header("Item Info")]
        [SerializeField] string name;
        [SerializeField] string description;
        [SerializeField] int price;
        [SerializeField] Sprite sprite;
        [SerializeField] bool stackable = false;

        [Header("Item Effect")]
        [SerializeField] int modifier;// how much heal or inflict damage 
        [SerializeField] bool isPercentage = false;//for certain amount of weapons to deal 30% more damage 
        */

        public void Use(BaseStats target)
        {
            if(item.GetItemType() == ItemType.BattleItem)
            {
                ApplyBattleItemEffect(target);
            }
            else if(item.GetItemType() == ItemType.Consumable)
            {
                ApplyConsumableItemEffect(target);
            }
            else if(item.GetItemType() == ItemType.Equipable)
            {
                ApplyEquipableItemEffect(target);
            }
            else if(item.GetItemType() == ItemType.TimeEffect)
            {
                ApplyTimeEffectItem();
            }
            else if(item.GetItemType() == ItemType.StatusCondition)
            {
                ApplyStatusConditionEffect(target);
            }
            else if(item.GetItemType() == ItemType.NoEffect)
            {
                Debug.Log("It's not an usable item");
            }
        }

        private void ApplyStatusConditionEffect(BaseStats target)
        {
            if(item.GetItemEffect() == ItemEffect.HealStatusCondition)
            {
                // TEST IF THE ITEM HEAL THE SPECIFIED CONDITION
                //REMOVE STATUS CONDITION
            }
            else if (item.GetItemEffect() == ItemEffect.InflictStatusCondition)
            {
                //TEST IF IS IN BATTLE
                // APPLY THE CONDITION ON TARGET
            }
        }

        private void ApplyTimeEffectItem()
        {
            if(item.GetItemEffect() == ItemEffect.GainXP)
            {
                StartCoroutine(activateEffect.GainXPRoutine());
            } 
            else if(item.GetItemEffect() == ItemEffect.IncreaseMoney)
            {
                StartCoroutine(activateEffect.IncreaseMoneyRoutine());
            }
            else if(item.GetItemEffect() == ItemEffect.RepelEnemy)
            {
                StartCoroutine(activateEffect.RepelEnemyRoutine());
            }
        }

        private void ApplyEquipableItemEffect( BaseStats target)
        {
            if (item.GetItemEffect() == ItemEffect.IncreaseStatTemporarily)
            {
                //target.ModifyStatByEquip(item.GetAttributeToModify(), item.GetAdditiveModifiers(), item);
            }
        }

        private void ApplyConsumableItemEffect(BaseStats target)
        {
           if (item.GetIsPercentage())
           {
               int value = Mathf.FloorToInt((target.GetStat(item.GetAttributeToModify()) * item.GetAdditiveModifiers())/ 100);
               target.ModifyStatPermanent(item.GetAttributeToModify(), value);
            }
           else
           {
               target.ModifyStatPermanent(item.GetAttributeToModify(), item.GetAdditiveModifiers());
           }
        }

        private void ApplyBattleItemEffect(BaseStats target)
        {
            if (item.GetItemEffect() == ItemEffect.HealStat)
            {
                target.ModifyStatPermanent(item.GetAttributeToModify(), item.GetAdditiveModifiers());
            }
            else if (item.GetItemEffect() == ItemEffect.IncreaseStatTemporarily)
            {
                target.ModifyStatOnBattle(item.GetAttributeToModify(), item.GetAdditiveModifiers());
            }
            else if (item.GetItemEffect() == ItemEffect.MoreDamageNextTurn)
            {
                //turn some variable on combat class to true
                //subscribe to some coroutine or action to turn that variable to false
            }
        }

        #region PUBLIC METHODS
        public ItemConfig GetItemConfig()
        {
            return item;
        }
        #endregion
    }

}

