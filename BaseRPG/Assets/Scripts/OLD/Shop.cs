using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject sellPanel;
    [SerializeField] TMP_Text moneyText;

    public string[] itemsForSale = new string[40];

    [SerializeField] InventorySlot[] buyItemSlots;
    [SerializeField] InventorySlot[] sellItemSlot;

    [SerializeField] Item selectedItem;
    [SerializeField] TMP_Text buyItemName;
    [SerializeField] TMP_Text buyItemDescription;
    [SerializeField] TMP_Text buyItemValue;

    [SerializeField] TMP_Text sellItemName;
    [SerializeField] TMP_Text sellItemDescription;
    [SerializeField] TMP_Text sellItemValue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OpenShopMenu();
        }
    }

    public void OpenShopMenu()
    {
        shopPanel.SetActive(true);
        OpenBuyPanel();
        moneyText.text = "$" + GameManager.instance.GetMoney();
        GameManager.instance.SetMenuIsOpen(true);
    }

    public void CloseShopMenu()
    {
        shopPanel.SetActive(false);
        GameManager.instance.SetMenuIsOpen(false);
    }

    public void OpenBuyPanel()
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);
        buyItemSlots[0].Pressed();
        ShowItems(0);

    }

    public void OpenSellPanel()
    {
        sellPanel.SetActive(true);
        buyPanel.SetActive(false);
        sellItemSlot[0].Pressed();
        ShowItems(1);
    }

    public bool IsShopMenuOpen()
    {
        return shopPanel.activeInHierarchy;
    }
    public bool IsBuyMenuOpen()
    {
        return buyPanel.activeInHierarchy;
    }

    public bool IsSellMenuOpen()
    {
        return sellPanel.activeInHierarchy;
    }

    private void ShowItems(int action)
    {
        if(action == 0)
        {
            for (int i = 0; i < buyItemSlots.Length; i++)
            {
                buyItemSlots[i].slotValue = i;

                if (!string.IsNullOrEmpty(itemsForSale[i]))
                {
                    buyItemSlots[i].slotImage.gameObject.SetActive(true);
                    buyItemSlots[i].slotImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).GetItemSprite();
                    buyItemSlots[i].amountText.text = "";
                }
                else
                {
                    buyItemSlots[i].slotImage.gameObject.SetActive(false);
                    buyItemSlots[i].amountText.text = "";
                }
            }
        } else if(action == 1)
        {
            GameManager.instance.SortItems();
            for (int i = 0; i < sellItemSlot.Length; i++)
            {
                sellItemSlot[i].slotValue = i;

                if (!string.IsNullOrEmpty(GameManager.instance.inventory[i]))
                {
                    sellItemSlot[i].slotImage.gameObject.SetActive(true);
                    sellItemSlot[i].slotImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.inventory[i]).GetItemSprite();
                    sellItemSlot[i].amountText.text = "";
                }
                else
                {
                    sellItemSlot[i].slotImage.gameObject.SetActive(false);
                    sellItemSlot[i].amountText.text = "";
                }
            }
        }
        
    }

    public void SelectBuyItem(Item itemToBuy)
    {
        selectedItem = itemToBuy;
        if (selectedItem == null) return;

        buyItemName.text = selectedItem.GetName();
        buyItemDescription.text = selectedItem.GetDescription();
        buyItemValue.text = "Price: " + selectedItem.GetPrice().ToString();

    }

    public void SelectSellItem(Item itemToSell)
    {
        selectedItem = itemToSell;
        if (selectedItem == null) 
        {
            return;
        } 
        
        sellItemName.text = selectedItem.GetName();
        sellItemDescription.text = selectedItem.GetDescription();
        sellItemValue.text = "Value:" + (Mathf.FloorToInt(selectedItem.GetPrice() * .5f)).ToString();
    }

    public void BuyItem()
    {
        if (selectedItem == null) return;

        if(GameManager.instance.GetMoney() >= selectedItem.GetPrice())
        {
            GameManager.instance.ReduceMoney(selectedItem.GetPrice());
            GameManager.instance.AddItem(selectedItem.GetName(),1);
        }

        moneyText.text = "$" + GameManager.instance.GetMoney();
    }

    public void SellItem()
    {

        if(selectedItem != null)
        {
            Debug.Log(selectedItem.GetName());
            GameManager.instance.IncreaseMoney(Mathf.FloorToInt(selectedItem.GetPrice() * 0.5f));
            GameManager.instance.RemoveItem(selectedItem.GetName(), 1);
        }
        moneyText.text = "$" + GameManager.instance.GetMoney();
    }
}
