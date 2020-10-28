using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;
using TMPro;
using RPG.Stats;

namespace RPG.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Item Config")]
        [SerializeField] TMP_Text selectedItemName;
        [SerializeField] TMP_Text selectedItemDescription;
        [SerializeField] TMP_Text selectedItemType;
        [SerializeField] Image selectedItemSprite;
        [SerializeField] TMP_Text useOrEquipText;
        [SerializeField] Button useOrEquipButton;
        [SerializeField] Button discardButton;

        [Header("General")]
        Inventory playerInventory;
        [SerializeField] InventorySlotUI prefabSlot = null;
        [SerializeField] PlayerTeam playerTeam;
        [SerializeField] int memberSelected = 0;

        [Header("SubPanel")]
        [SerializeField] GameObject panelUseFor;
        [SerializeField] PanelUseFor[] partyMembersUseForPanel;
        [SerializeField] PartyMemberButton[] partyMemberButtons;
        [SerializeField] SlotsSelectedMember[] slotsSelectedMember;
        [SerializeField] TMP_Text selectedCharName;

        //[SerializeField] EquipmentSlot[] equimentSlots;

        int indexOfSelectItem = -1;

        #region STRUCTS
        [System.Serializable]
        public struct PanelUseFor
        {
            public GameObject charPanel;
            public Image charImage;
            public TMP_Text textCharName;
        }

        [System.Serializable]
        public struct PartyMemberButton
        {
            public GameObject button;
            public Image charImage;
        }

        [System.SerializableAttribute]
        public struct SlotsSelectedMember
        {
            public ItemConfig item;
            public CharEquipmentSlot typeOfSlot;
            public TMP_Text itemName;
            public Image itemSprite;
        }
        #endregion

        private void Start()
        {
            playerInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>().GetPlayerInventory();
            playerTeam = GameObject.FindWithTag("Player").GetComponent<PlayerTeam>();
            playerInventory.OnInventoryUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerInventory.GetSize(); i++)
            {
                if (playerInventory.IsEmptySlot(i))
                {
                    continue;
                }
                var itemUI = Instantiate(prefabSlot, transform);
                itemUI.Setup(playerInventory, i);
            }
            if(indexOfSelectItem < 0)
            {
                ResetSelectedItem();
            }
            else
            {
                selectedItemSprite.gameObject.SetActive(true);
            }
            RedrawEquipmentPanel();
        }

        private void RedrawEquipmentPanel()
        {
            CharacterSheet[] team = playerTeam.GetPlayerTeam();

            for(int i = 0; i < team.Length; i++)
            {
                if (team[i] != null)
                {
                    partyMemberButtons[i].button.SetActive(true);
                    partyMemberButtons[i].charImage.sprite = team[i].GetSprite();
                }
                else
                {
                    partyMemberButtons[i].button.SetActive(false);
                }
            }
            LoadEquipments(0);
        }

        private void ReturnItemToInventory(CharEquipmentSlot slotNeeded, BaseStats member, ItemConfig currentEquippedItem)
        {
            member.ReturnAfterUnnequip(currentEquippedItem.GetAttributeToModify(), currentEquippedItem, slotNeeded);
            playerInventory.AddItem(currentEquippedItem, 1);
        }
        private void AddItemToEquipmentSlot(ItemConfig item, CharEquipmentSlot slotNeeded, BaseStats member)
        {
            member.ModifyStatByEquip(item.GetAttributeToModify(), item.GetAdditiveModifiers(), item, slotNeeded);
            playerInventory.RemoveItem(item, 1);
        }

        #region PUBLIC METHODS
        public void SelectedItem(int index)
        {
            indexOfSelectItem = index;
            selectedItemName.text = playerInventory.GetItemInSlot(index).GetDisplayName().ToUpper();
            selectedItemDescription.text = playerInventory.GetItemInSlot(index).GetDescription().ToUpper();
            selectedItemType.text = playerInventory.GetItemInSlot(index).GetTypeString().ToUpper();
            selectedItemSprite.gameObject.SetActive(true);
            selectedItemSprite.sprite = playerInventory.GetItemInSlot(index).GetIcon();
            if(playerInventory.GetItemInSlot(index).GetItemType() == ItemType.Equipable)
            {
                useOrEquipButton.gameObject.SetActive(false);
                discardButton.gameObject.SetActive(true);
            }
            else if (playerInventory.GetItemInSlot(index).GetItemType() == ItemType.NoEffect)
            {
                useOrEquipButton.gameObject.SetActive(false);
                discardButton.gameObject.SetActive(false);
            }
            else
            {
                useOrEquipButton.gameObject.SetActive(true);
                discardButton.gameObject.SetActive(true);
                useOrEquipText.text = "USE";
            }
        }

        public void Close()
        {
            ResetSelectedItem();
            useOrEquipButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
        }

        private void ResetSelectedItem()
        {
            indexOfSelectItem = -1;
            selectedItemName.text = "???";
            selectedItemDescription.text = "NO ITEMS SELECTED";
            selectedItemType.text = "???";
            selectedItemSprite.sprite = null;
            selectedItemSprite.gameObject.SetActive(false);
            useOrEquipButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
        }

        public void SetupUseForPanel()
        {
            if (indexOfSelectItem < 0)
            {
                return;
            }

            panelUseFor.SetActive(true);
            CharacterSheet[] team = playerTeam.GetPlayerTeam();
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] != null)
                {
                    partyMembersUseForPanel[i].charPanel.SetActive(true);
                    partyMembersUseForPanel[i].charImage.sprite = team[i].GetSprite();
                    if(partyMembersUseForPanel[i].charImage == null)
                    {
                        partyMembersUseForPanel[i].charImage.gameObject.SetActive(false);
                    }
                    else
                    {
                        partyMembersUseForPanel[i].charImage.gameObject.SetActive(true);
                    }
                    partyMembersUseForPanel[i].textCharName.text = team[i].GetCharName().ToUpper();
                    if (playerInventory.GetItemInSlot(indexOfSelectItem).GetItemType() == ItemType.Equipable)
                    {
                        return;
                    }
                }
                else
                {
                    partyMembersUseForPanel[i].charPanel.SetActive(false);
                    partyMembersUseForPanel[i].charImage = null;
                    partyMembersUseForPanel[i].textCharName.text = null;
                }
            }
        }

        public void UseEquipItem(int member)
        {
            if(indexOfSelectItem < 0)
            {
                return;
            }

            ItemConfig itemToUse = playerInventory.GetItemInSlot(indexOfSelectItem);
            BaseStats target = playerTeam.GetPlayerTeam()[member].GetStats();

            if(itemToUse.GetItemType() == ItemType.NoEffect)
            {
                return;
            }
           if(itemToUse.GetItemType() == ItemType.Equipable)
            {
                return;
            }

            itemToUse.Use(target);
            playerInventory.RemoveItem(playerInventory.GetItemInSlot(indexOfSelectItem), 1);
            ResetSelectedItem();

            panelUseFor.SetActive(false);
        }

        public void Discard()
        {
            if (indexOfSelectItem < 0)
            {
                return;
            }

            playerInventory.RemoveItem(playerInventory.GetItemInSlot(indexOfSelectItem), 1);
            ResetSelectedItem();
            panelUseFor.SetActive(false);
        }

        public void LoadEquipments(int index)
        {
            memberSelected = index;
            CharacterSheet charSelected = playerTeam.GetSpecificMember(index);
            ItemConfig[] equippedItems = new ItemConfig[3];
            equippedItems[0] = charSelected.GetStats().GetEquippedItem(CharEquipmentSlot.LeftHand);
            equippedItems[1] = charSelected.GetStats().GetEquippedItem(CharEquipmentSlot.Book);
            equippedItems[2] = charSelected.GetStats().GetEquippedItem(CharEquipmentSlot.RightHand);
            charSelected.GetStats().GetAllEquippedItem();
            selectedCharName.text = charSelected.GetCharName().ToUpper();

            for (int i =0; i < equippedItems.Length; i++)
            {
                if (equippedItems[i] != null)
                {
                    slotsSelectedMember[i].item = equippedItems[i] ;
                    slotsSelectedMember[i].itemName.text = equippedItems[i].GetDisplayName().ToUpper();
                    slotsSelectedMember[i].itemSprite.gameObject.SetActive(true);
                    slotsSelectedMember[i].itemSprite.sprite = equippedItems[i].GetIcon();
                }
                else
                {
                    slotsSelectedMember[i].item = null;
                    slotsSelectedMember[i].itemName.text = "NO EQUIPPED ITEM";
                    slotsSelectedMember[i].itemSprite.sprite = null;
                    slotsSelectedMember[i].itemSprite.gameObject.SetActive(false);
                }
            }        
        }

        public void EquipOn(int typeOfSlot)
        {
            CharEquipmentSlot slotNeeded;

            switch (typeOfSlot)
            {
                case 0:
                    slotNeeded = CharEquipmentSlot.LeftHand;
                    break;
                case 1:
                    slotNeeded = CharEquipmentSlot.Book;
                    break;
                case 2:
                    slotNeeded = CharEquipmentSlot.RightHand;
                    break;
                default:
                    slotNeeded = CharEquipmentSlot.LeftHand;
                    break;
            }

            BaseStats member = playerTeam.GetSpecificMember(memberSelected).GetStats();
            ItemConfig currentEquippedItem = member.GetEquippedItem(slotNeeded);

            if(currentEquippedItem != null)
            {
                ReturnItemToInventory(slotNeeded, member, currentEquippedItem);
                currentEquippedItem = null;
            }

            if (indexOfSelectItem < 0) return;

            ItemConfig item = playerInventory.GetItemInSlot(indexOfSelectItem);
            
            if (item == null) return;
            if (item.GetItemType() != ItemType.Equipable) return;
            if(slotNeeded != item.GetCanEquipOn()) return;

            AddItemToEquipmentSlot(item, slotNeeded, member);
            LoadEquipments(memberSelected);
            ResetSelectedItem();
        }
        #endregion
    }

}
