using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using RPG.Inventories;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    [SerializeField] ItemType type;
    [SerializeField] ItemEffect effect;

    [Header("Item Info")]
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] int price;
    [SerializeField] Sprite sprite;
    
    [Header("Item Effect")]
    [SerializeField] int modifier;// how much heal or inflict damage 
    [SerializeField] bool isPercentage = false;//for certain amount of weapons to deal 30% more damage 

    public Sprite GetItemSprite()
    {
        return sprite;
    }
    public ItemType GetItemType()
    {
        return type;
    }

    public ItemEffect GetItemEffect()
    {
        return effect;
    }

    public string GetName()
    {
        return Regex.Replace(name, @"\s+", "");
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetPrice()
    {
        return price;
    }

    public void Use(int partyMemberIndex)
    {
        GameManager.instance.RemoveItem(Regex.Replace(name, @"\s+", ""), 1);
        Stats selectedChar = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

        if(type == ItemType.Consumable)
        {
            switch (effect)
            {
                case ItemEffect.IncreaseStatTemporarily:
                    Debug.Log(selectedChar.currentHP);
                    selectedChar.currentHP = Mathf.Min(selectedChar.currentHP + modifier, selectedChar.maxHP);
                    Debug.Log(selectedChar.currentHP);
                    break;
                default:
                    selectedChar.currentHP = Mathf.Min(selectedChar.currentHP + modifier, selectedChar.maxHP);
                    break;
            }
        }

    }
}
