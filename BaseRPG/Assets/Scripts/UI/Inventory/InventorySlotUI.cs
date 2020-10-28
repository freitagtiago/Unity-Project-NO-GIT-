using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        Inventory inventory;
        int index;
        [SerializeField] TMP_Text itemName;
        [SerializeField] TMP_Text itemAmount;
    
        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;

            itemName.text = inventory.GetItemInSlot(index).GetDisplayName();
            itemAmount.text = inventory.GetAmountOfItemInSlot(index).ToString();
        }

        public void Pressed()
        {
            FindObjectOfType<InventoryUI>().SelectedItem(index);
        }
    }
}

