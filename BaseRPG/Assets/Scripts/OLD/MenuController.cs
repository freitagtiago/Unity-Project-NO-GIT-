using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject[] windows = new GameObject[2];
    [SerializeField] GameObject[] statPanel = new GameObject[3];
    Stats[] stats;

    [SerializeField] TMP_Text[] textNames = new TMP_Text[3];
    [SerializeField] TMP_Text[] hpText = new TMP_Text[3];
    [SerializeField] TMP_Text[] mpText = new TMP_Text[3];
    [SerializeField] TMP_Text[] xpText = new TMP_Text[3];
    [SerializeField] TMP_Text[] xpToLevelUpText = new TMP_Text[3];
    [SerializeField] TMP_Text[] levelText = new TMP_Text[3];
    [SerializeField] TMP_Text[] strengthText = new TMP_Text[3];
    [SerializeField] TMP_Text[] defenseText = new TMP_Text[3];
    [SerializeField] Slider[] xpSlider = new Slider[3];

    public InventorySlot[] slots;

    public static MenuController instance;

    public string selectedItem;
    public Item activeItem;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text useButtonText;

    [SerializeField] GameObject partyPanel;
    public TMP_Text[] charChoiceNames;
    [SerializeField] TMP_Text moneyAmount;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (menuPanel.activeInHierarchy)
            {
                menuPanel.SetActive(false);
                GameManager.instance.SetMenuIsOpen(false);
            }
            else
            {
                menuPanel.SetActive(true);
                GameManager.instance.SetMenuIsOpen(true);
                UpdateMainStats();
            }
            PlayButtonSound();
        }
    }

    public void UpdateMainStats()
    {
        stats = GameManager.instance.GetStats();

        for(int i = 0; i < stats.Length; i++)
        {
            if(stats[i] != null)
            {
                statPanel[i].SetActive(true);

                textNames[i].text = stats[i].charName;
                hpText[i].text = "HP: " + stats[i].currentHP + "/" + stats[i].maxHP;
                mpText[i].text = "MP: " + stats[i].currentMP +"/"+stats[i].maxMP;
                xpText[i].text = "Exp. Points:" + stats[i].currentXp;
                xpToLevelUpText[i].text = stats[i].currentXp + "/" + stats[i].xpForLevel[stats[i].level + 1];
                levelText[i].text = "Lvl: " + stats[i].level;
                strengthText[i].text = "Strength: " + stats[i].strength;
                defenseText[i].text = "Defense: " + stats[i].defense;
                xpSlider[i].maxValue = stats[i].xpForLevel[stats[i].level+1];
                xpSlider[i].value = stats[i].currentXp;
            }
            else
            {
                statPanel[i].SetActive(false);
            }
        }
        moneyAmount.text = "$ " + GameManager.instance.GetMoney();
    }

    public void ToggleWindow(int index)
    {
        for(int i = 0; i < windows.Length; i++){
            if(i != index)
            {
                windows[i].SetActive(false);
            }
            else
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
        }
        partyPanel.gameObject.SetActive(false);
    }

    public void Close()
    {
        for (int i = 0; i < windows.Length; i++)
        {
           windows[i].SetActive(false);
        }
        menuPanel.SetActive(false);
        GameManager.instance.SetMenuIsOpen(false);

        partyPanel.gameObject.SetActive(false);
    }

    public void Save()
    {
        GameManager.instance.SaveData();
        QuestController.instance.SaveQuestData();
    }

    public void Quit()
    {
        SceneManager.LoadScene(3);
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.playerInstance.gameObject);
        Destroy(AudioController.instance.gameObject);
        Destroy(gameObject);
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].slotValue = i;

            if (!string.IsNullOrEmpty(GameManager.instance.inventory[i]))
            {
                slots[i].slotImage.gameObject.SetActive(true);
                slots[i].slotImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.inventory[i]).GetItemSprite();
                slots[i].amountText.text = GameManager.instance.amount[i].ToString();
            }
            else
            {
                slots[i].slotImage.gameObject.SetActive(false);
                slots[i].amountText.text = "";
            }
        }
    }

   /* public void SelectItem(Item selected)
    {
        activeItem = selected;
        if (activeItem.GetItemType() == ItemType.Equipable || activeItem.GetItemType() == ItemType.Weapon)
        {
            useButtonText.text = "Equip";
        }
        else
        {
            useButtonText.text = "Use";
        }
        itemName.text = activeItem.GetName();
        itemDescription.text = activeItem.GetDescription();

    }*/

    public void Discard()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.name, 1);
        }
    }

    public void UseOrEquipe()
    {
        activeItem.Use(0);
        ClosePartyChoicePanel();
    }

    public void OpenPartyChoicePanel()
    {
        partyPanel.SetActive(true);
        for(int i = 0; i < charChoiceNames.Length; i++)
        {
            charChoiceNames[i].text = GameManager.instance.partyMembers[i];
            charChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.gameObject.activeInHierarchy);
        }
    }

    public void ClosePartyChoicePanel()
    {
        partyPanel.SetActive(false);
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestController.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioController.instance.PlaySFX(4);
    }
}
