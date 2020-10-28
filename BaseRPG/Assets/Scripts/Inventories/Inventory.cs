using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour
    {
        [Header("General Config")]
        [SerializeField] int inventorySize = 99;
        [SerializeField] int maxAmountPerSlot = 99;

        public event Action OnInventoryUpdate;
        [SerializeField] Slot[] inventory;
        [SerializeField] ItemConfig item;
        [SerializeField] ItemConfig equippableItem;

        private void Awake()
        {
            inventory = new Slot[inventorySize];
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                AddItem(item, 2);
                AddItem(equippableItem, 1);
                AddItem(equippableItem, 1);
            }
        }

        [System.Serializable]
        public struct Slot
        {
            public ItemConfig item;
            public int amount;
        }

        private void AddToSlot(int index, ItemConfig itemToAdd, int amountToAdd)
        { 
            inventory[index].item = itemToAdd;
            inventory[index].amount += amountToAdd;

            if (GetAmountOfItemInSlot(index) > maxAmountPerSlot) 
            {
                inventory[index].amount = maxAmountPerSlot;
            }
            SortItems();
            if(OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }
            
        }

        private void RemoveFromSlot(int index, ItemConfig itemToAdd, int amountToAdd)
        {
            inventory[index].amount -= amountToAdd;

            if (GetAmountOfItemInSlot(index) <= 0)
            {
                inventory[index].item = null;
                inventory[index].amount = 0;
            }
            SortItems();
            if (OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }
        }


        private bool AddToEmptySlot(ItemConfig item, int amount)
        {
            bool foundSlotToAdd = false;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i].item == null)
                {
                    AddToSlot(i, item, amount);
                    foundSlotToAdd = true;
                    i = inventory.Length;
                }
            }
            if (OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }

            return foundSlotToAdd;
        }


        private bool AddStackableItemToSlot(ItemConfig item, int amount)
        {
            bool foundSlotToAdd = false;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (IsEmptySlot(i))  continue;

                if (inventory[i].item.GetItemId() == item.GetItemId())
                {
                    AddToSlot(i, item, amount);
                    foundSlotToAdd = true;
                    i = inventory.Length;
                }
            }
            if (OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }
            return foundSlotToAdd;
        }

        public void SortItems()
        {
            bool itemAfterSpace = true;
            while (itemAfterSpace)
            {
                itemAfterSpace = false;

                for (int i = 0; i < inventory.Length - 1; i++)
                {
                    if (inventory[i].item == null)
                    {
                        inventory[i].item = inventory[i + 1].item;
                        inventory[i].amount = inventory[i + 1].amount;
                        inventory[i + 1].item = null;
                        inventory[i + 1].amount = 0;

                        if (inventory[i].item != null)
                        {
                            itemAfterSpace = true;
                        }
                        else
                        {
                            itemAfterSpace = false;
                        }
                    }
                }
            }
        }

        #region PUBLIC METHODS
        public void AddItem(ItemConfig item, int amount)
        {
            bool slotFound = false;
            if (item.IsStackable())
            {
                slotFound = AddStackableItemToSlot(item, amount);
                if (!slotFound)
                {
                    slotFound = AddToEmptySlot(item, amount);
                }
            }
            else
            {
                slotFound =  AddToEmptySlot(item, amount);
            }
            if (!slotFound)
            {
                Debug.LogError("There's no slot to add "+ item.GetDisplayName());
            }
        }
        public void RemoveItem(ItemConfig item, int amount)
        {
            bool foundItem = false;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (item.GetItemId() == inventory[i].item.GetItemId())
                {
                    RemoveFromSlot(i, item, amount);
                    foundItem = true;
                    i = inventory.Length;
                }
            }
            if (!foundItem)
            {
                Debug.LogError("Item "+ item.GetDisplayName() + " not found on inventory");
            }
            if (OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }
        }

        public Inventory GetPlayerInventory()
        {
            //var player = GameObject.FindWithTag("Player");
            //return player.GetComponent<Inventory>();
            return this;
        }

        public int GetSize()
        {
            return inventory.Length;
        }

        public ItemConfig GetItemInSlot(int index)
        {
            return inventory[index].item;
        }
        public int GetAmountOfItemInSlot(int index)
        {
            return inventory[index].amount;
        }

        public bool IsEmptySlot(int index)
        {
            if(inventory[index].item == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

