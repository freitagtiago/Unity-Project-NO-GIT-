using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string[] partyMembers;
    public static GameManager instance;
    private Stats[] stats = new Stats[3];
    private bool menuIsOpen = false;
    private bool isDialogActive = false;
    private bool isBetweenArea = false;
    private bool isBattleActive = false;
    [SerializeField]private int currentMoney;

    public string[] inventory = new string[40];
    public int[] amount = new int[40];
    public Item[] referenceItem = new Item[40];

    bool loadData = false;

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

        DontDestroyOnLoad(gameObject);
        partyMembers = new string[3];
        partyMembers[0] = "Player 1";
        partyMembers[1] = "Player 2";
        partyMembers[2] = "Player 3";
        SortItems();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loadData)
        {
            SetLoadedPosition();
            loadData = false;
        }

        if(menuIsOpen || isDialogActive || isBetweenArea || isBattleActive)
        {
            PlayerController.playerInstance.SetCanMove(false);
        }
        else
        {
            PlayerController.playerInstance.SetCanMove(true);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    public bool GetMenuIsOpen()
    {
        return menuIsOpen;
    }
    public bool GetIsDialogActive()
    {
        return isDialogActive;
    }
    public bool GetIsBetweenArea()
    {
        return isBetweenArea;
    }

    public bool GetIsBatlleActive()
    {
        return isBattleActive;
    }

    public void SetMenuIsOpen(bool value)
    {
        menuIsOpen = value;
        if (value)
        {
            PlayerController.playerInstance.CancelMovement();
        }
    }
    public void SetIsDialogActive(bool value)
    {
        isDialogActive = value;
        if (value)
        {
            PlayerController.playerInstance.CancelMovement();
        }
    }
    public void SetIsBetweenArea(bool value)
    {
        isBetweenArea = value;
        if (value)
        {
            PlayerController.playerInstance.CancelMovement();
        }
    }

    public void SetIsBattleActive(bool value)
    {
        isBattleActive = value;
        if (value)
        {
            PlayerController.playerInstance.CancelMovement();
        }
    }


    public Stats[] GetStats()
    {
        return stats;
    }

    public void SetStats(Stats stat, int index)
    {
        stats[index] = stat;
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i < referenceItem.Length; i++)
        {
            if(referenceItem[i].GetName() == itemToGrab)
            {
                return referenceItem[i];
            }
        }
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;
        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for (int i = 0; i < inventory.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(inventory[i]))
                {
                    inventory[i] = inventory[i + 1];
                    inventory[i + 1] = null;
                    amount[i] = amount[i + 1];
                    amount[i + 1] = 0;

                    if (!string.IsNullOrEmpty(inventory[i]))
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }   
    }


    public void AddItem(string itemToAdd, int amountToAdd)
    {
        int position = 0;
        bool foundSlot = false;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (string.IsNullOrEmpty(inventory[i]) || inventory[i] == itemToAdd)
            {
                position = i;
                i = inventory.Length;
                foundSlot = true;
            }
        }

        if (foundSlot)
        {
            bool isValidItem = false;

            for(int i = 0; i < referenceItem.Length; i++)
            {
                if(referenceItem[i].name == itemToAdd)
                {
                    isValidItem = true;
                    i = referenceItem.Length;
                }
            }
            if (isValidItem)
            {
                inventory[position] = itemToAdd;
                amount[position] += amountToAdd;
            }
            else
            {
                Debug.LogError("Invalid Item " + itemToAdd);
            }
        }
        MenuController.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove, int amountToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < inventory.Length; i++)
        {
            if (string.Equals(inventory[i], itemToRemove))
            {
                foundItem = true;
                itemPosition = i;
                i = inventory.Length;
            }
        }

        if(foundItem)
        {
            amount[itemPosition] -= amountToRemove;
            if(amount[itemPosition] < 1)
            {
                inventory[itemPosition] = null;
                amount[itemPosition] = 0;
            }
        }
        else
        {
            Debug.LogError("Item " + itemToRemove + " not found");
        }
        MenuController.instance.ShowItems();
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public void IncreaseMoney(int quantity)
    {
        currentMoney += quantity;
    }


    public void ReduceMoney(int quantity)
    {
        currentMoney -= quantity;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Current_Scene",SceneManager.GetActiveScene().buildIndex);

        PlayerPrefs.SetFloat("Player_Position_X", PlayerController.playerInstance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_Y", PlayerController.playerInstance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_Z", PlayerController.playerInstance.transform.position.z);


        Debug.Log("SAVE POSITION X " + PlayerController.playerInstance.transform.position.x +
            " / Y " +PlayerController.playerInstance.transform.position.y + 
            " /Z" + PlayerController.playerInstance.transform.position.z);


        PlayerPrefs.SetInt("Player_Money", currentMoney);
        for(int i = 0; i < stats.Length; i++)
        {
            if(stats[i] != null)
            {
                if (stats[i].gameObject.activeInHierarchy)
                {
                    PlayerPrefs.SetInt("Player_" + stats[i].charName + "_active", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("Player_" + stats[i].charName + "_active", 0);
                }

                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_Level", stats[i].level);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_MaxHP", stats[i].maxHP);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_MaxMP", stats[i].maxMP);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_currentHP", stats[i].currentHP);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_currentMP", stats[i].currentMP);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_currentXP", stats[i].currentXp);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_defense", stats[i].defense);
                PlayerPrefs.SetInt("Player+" + stats[i].charName + "_strength", stats[i].strength);
            }
        }
        
        for(int i = 0; i < inventory.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_"+ i, inventory[i]);
            PlayerPrefs.SetInt("ItemAmount_"+ i, amount[i]);
        }

    }

    public void LoadData()
    {
        PlayerController.playerInstance.areaTransition = "";
        SceneManager.LoadScene(PlayerPrefs.GetInt("Current_Scene"));

        currentMoney = PlayerPrefs.GetInt("Player_Money");

        for(int i = 0; i < stats.Length; i++)
        {
            if(stats[i] != null)
            {
                if (PlayerPrefs.GetInt("Player_" + stats[i].charName + "_active") == 0)
                {
                    stats[i].gameObject.SetActive(false);
                }
                else
                {
                    stats[i].gameObject.SetActive(true);
                }
                stats[i].level = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_Level");
                stats[i].maxHP = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_MaxHP");
                stats[i].maxMP = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_MaxMP");
                stats[i].currentHP = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_currentHP");
                stats[i].currentMP = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_currentMP");
                stats[i].currentXp = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_currentXP");
                stats[i].defense = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_defense");
                stats[i].strength = PlayerPrefs.GetInt("Player+" + stats[i].charName + "_strength");
            }
        }

        for(int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            amount[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }

        loadData = true;
    }

    void SetLoadedPosition()
    {
        PlayerController.playerInstance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_X")
                                                                       , PlayerPrefs.GetFloat("Player_Position_Y")
                                                                       , PlayerPrefs.GetFloat("Player_Position_Z"));
    }
}
