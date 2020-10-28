using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image slotImage;
    public TMP_Text amountText;
    public int slotValue;

    public void Pressed()
    {
        if (!Shop.instance.IsShopMenuOpen())
        {
            if (!string.IsNullOrEmpty(GameManager.instance.inventory[slotValue]))
            {
                //MenuController.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.inventory[slotValue]));
            }
        }
        else 
        {
            if (Shop.instance.IsBuyMenuOpen())
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[slotValue]));
            }

            if (Shop.instance.IsSellMenuOpen())
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.inventory[slotValue]));
            }
        }

        if (GameManager.instance.GetIsBatlleActive())
        {
            //BattleManager.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.inventory[slotValue]));
        }
        
    }
}
