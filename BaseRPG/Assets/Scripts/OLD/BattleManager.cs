using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    
    bool isBattleActive;
    public GameObject battleScene;
    public Transform[] playerPartyPosition;
    public Transform[] enemyPartyPosition;

    public BattleChar[] playerParty;
    public BattleChar[] enemyParty;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;
    public GameObject battlePanel;
    [SerializeField] GameObject currentAttacker;

    public BattleMove[] movesList;

    [SerializeField] DisplayHPChanges displayerHP;
    [SerializeField] TMP_Text[] partyNames;
    [SerializeField] TMP_Text[] partyHP;
    [SerializeField] TMP_Text[] partyMP;

    [SerializeField] GameObject menuTargetSelection;
    [SerializeField] BattleTargetSelection[] battleTargetButtton;

    [SerializeField] GameObject magicMenuPanel;
    [SerializeField] BattleMagicSelect[] magicButtons;
    public BattleNotification battleNotice;
    [SerializeField] int chanceToFlee = 50;

    [SerializeField] GameObject inventoryPanel;
    Item activeItem;
    [SerializeField] TMP_Text useButtonText;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemDescription;
    [SerializeField] InventorySlot[] slots;
    [SerializeField] GameObject partyPanel;
    public TMP_Text[] charChoiceNames;
    private bool isGameOver = false;
    private bool fleeing;
    public int rewardXP;
    public string[] rewardItems;
    bool canFleeFromBattle = true;

    // Start is called before the first frame update
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

    }

    // Update is called once per frame
    void Update()
    {
        if (isBattleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    battlePanel.SetActive(true);
                } else
                {
                    battlePanel.SetActive(false);
                    StartCoroutine(EnemyMoveCall());
                    //Enemy should attack
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                StartCoroutine(NextTurn());
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool canFlee)
    {
        if (!isBattleActive)
        {
            canFleeFromBattle = canFlee;
            isBattleActive = true;
            PlayerController.playerInstance.CancelMovement();
            GameManager.instance.SetIsBattleActive(true);
            transform.position = new Vector3(Camera.main.transform.position.x
                                             , Camera.main.transform.position.y
                                             , transform.position.z);
            battleScene.SetActive(true);

            AudioController.instance.PlayBackgroundMusic(0);

            //Setting up player party
            BattleChar newPlayer = Instantiate(playerParty[0], playerPartyPosition[0]);
            newPlayer.transform.parent = playerPartyPosition[0];
            activeBattlers.Add(newPlayer);

            Stats[] partyStats = GameManager.instance.GetStats();
            Stats player = partyStats[0];
            activeBattlers[0].isPlayer = true;
            activeBattlers[0].charName = "Player :)";
            activeBattlers[0].currentHP = player.currentHP;
            activeBattlers[0].maxHP = player.maxHP;
            activeBattlers[0].currentMP = player.currentMP;
            activeBattlers[0].maxMP = player.maxHP;
            activeBattlers[0].strength = player.strength;
            activeBattlers[0].defense = player.defense;

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (!string.IsNullOrEmpty(enemiesToSpawn[i]))
                {
                    for(int j = 0; j < enemyParty.Length; j++)
                    {
                        if(enemyParty[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyParty[j], enemyPartyPosition[i].position, enemyPartyPosition[i].rotation);
                            newEnemy.transform.parent = enemyPartyPosition[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }
            turnWaiting = true;
            currentTurn = 0;
            UpdateUIStats();
        }
    }

    public IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(1.5f);

        if (isGameOver == false)
        {
            if (!isBattleActive == false)
            {
                for (int i = 0; i < activeBattlers.Count; i++)
                {
                    currentTurn++;

                    if (currentTurn >= activeBattlers.Count)
                    {
                        currentTurn = 0;
                    }

                    if (activeBattlers[currentTurn].currentHP > 0)
                    {
                        i = activeBattlers.Count;
                    }
                }

                turnWaiting = true;
                Instantiate(currentAttacker, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                UpdateBattle();
            }
        }
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = false;

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if(activeBattlers[i].currentHP == 0)
            {
                activeBattlers[i].isDead = true;
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if(allPlayersDead || allEnemiesDead)
        {
            if (allEnemiesDead)
            {
                //END BATTLE IN VICTORY
            }
            else
            {
                isGameOver = true;
            }

            StartCoroutine(EndBattle());
        }
        else
        {
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    private IEnumerator EndBattle()
    {
        if(isGameOver == false)
        {
            isBattleActive = false;
            menuTargetSelection.SetActive(false);
            magicMenuPanel.SetActive(false);
            inventoryPanel.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            Fader.instance.SetFadeTo();

            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].isPlayer)
                {
                    for (int j = 0; j < GameManager.instance.GetStats().Length; j++)
                    {
                        if (activeBattlers[i].charName == GameManager.instance.GetStats()[j].charName)
                        {
                            Stats stat = new Stats();
                            stat = GameManager.instance.GetStats()[j];
                            stat.currentHP = activeBattlers[j].currentHP;
                            stat.currentMP = activeBattlers[j].currentMP;

                            GameManager.instance.SetStats(stat, i);
                            j = GameManager.instance.GetStats().Length;
                        }
                    }
                }
                Destroy(activeBattlers[i].gameObject);
            }

            Fader.instance.SetFadeToFrom();


            battleScene.SetActive(false);
            GameManager.instance.SetIsBattleActive(false);
            activeBattlers.Clear();
            currentTurn = 0;
            if (fleeing)
            {
                GameManager.instance.SetIsBattleActive(false);
                fleeing = false;
            }
            else
            {
                BattleReward.instance.OpenRewardPanel(rewardXP, rewardItems);
            }
        }
        else
        {
            isGameOver = false;
            isBattleActive = false;
            Fader.instance.SetFadeTo();

            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene(6);//GameOver
        }
        
    }

    public IEnumerator EnemyMoveCall()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(2f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        StartCoroutine(NextTurn());
    }
    public void EnemyAttack()
    {
        List<int> players = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        if(players.Count == 0)
        {
            isGameOver = true;
            StartCoroutine(EndBattle());
            return;
        }

        int selectedTarget = players[Random.Range(0, players.Count)];
        int selectedAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for(int i = 0; i< movesList.Length; i++)
        {
            if(movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectedAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
                AudioController.instance.PlaySFX(3);
            }
        }
        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float attackPower = activeBattlers[currentTurn].strength;
        float defensePower = activeBattlers[target].defense;
        float damageCalculated = 0;

        if(defensePower == 0)
        {
            defensePower = 1;
        }
        damageCalculated = (attackPower / defensePower) * movePower * Random.Range(0.9f, 1.1f);
        Instantiate(displayerHP, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).DisplayHPChange(-Mathf.FloorToInt(damageCalculated));
        activeBattlers[target].currentHP -= Mathf.FloorToInt(damageCalculated);
        if(activeBattlers[target].currentHP <= 0)
        {
            activeBattlers[target].currentHP = 0;
        }
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < partyNames.Length; i++)
        {
            if(activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];
                    partyNames[i].gameObject.SetActive(true);
                    partyNames[i].text = playerData.charName;
                    partyHP[i].text = playerData.currentHP + "/" + playerData.maxHP;
                    partyMP[i].text = playerData.currentMP + "/" + playerData.maxMP;
                } else
                {
                    partyNames[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        turnWaiting = false;
        menuTargetSelection.SetActive(false);
        battlePanel.gameObject.SetActive(false);

        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
                AudioController.instance.PlaySFX(3);
            }
        }

        DealDamage(selectedTarget, movePower);
        StartCoroutine(NextTurn());
    }

    public void OpenTargetMenu(string moveName)
    {
        menuTargetSelection.SetActive(true);

        List<int> enemies = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer && !activeBattlers[i].isDead)
            {
                enemies.Add(i);
            }
        }

        for(int i = 0; i < battleTargetButtton.Length; i++)
        {
            if(enemies.Count > i && activeBattlers[enemies[i]].currentHP > 0)
            {
                battleTargetButtton[i].gameObject.SetActive(true);
                battleTargetButtton[i].moveName = moveName;
                battleTargetButtton[i].activeBattleTarget = enemies[i];
                battleTargetButtton[i].targetName.text = activeBattlers[enemies[i]].charName;
            } else
            {
                battleTargetButtton[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenuPanel.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].magicName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].textMagicName.text = magicButtons[i].magicName;

                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].magicName)
                    {
                        magicButtons[i].mpCost = movesList[j].moveCost;
                        magicButtons[i].textMPCost.text = magicButtons[i].mpCost + "MP";
                        j = movesList.Length;
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseMagicMenu()
    {
        magicMenuPanel.SetActive(false);
    }

    public void Flee()
    {

        if (!canFleeFromBattle)
        {
            battleNotice.Activate("Cannot flee this battle!");
        }
        else
        {
            int fleeSucess = Random.Range(0, 100);

            if (fleeSucess <= chanceToFlee)
            {
                fleeing = true;
                StartCoroutine(EndBattle());
            }
            else
            {
                StartCoroutine(NextTurn());
                battleNotice.Activate("You Can't Flee");
            }
        }
        
    }

    public void OpenItemMenu()
    {
        inventoryPanel.SetActive(true);
        ShowItems();
    }

    public void CloseItemMenu()
    {
        inventoryPanel.SetActive(false);
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < slots.Length; i++)
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

    /*public void SelectItem(Item selected)
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

    public void OpenPartyChoicePanel()
    {
        partyPanel.SetActive(true);
        for (int i = 0; i < charChoiceNames.Length; i++)
        {
            charChoiceNames[i].text = GameManager.instance.partyMembers[i];
            charChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.gameObject.activeInHierarchy);
        }
    }

    public void ClosePartyChoicePanel()
    {
        partyPanel.SetActive(false);
    }

    public void UseOrEquipe()
    {
        StartCoroutine(NextTurn());
        activeItem.Use(0);
        ResetPlayerStats();
        ClosePartyChoicePanel();
        CloseItemMenu();
        battlePanel.SetActive(false);
    }

    private void ResetPlayerStats()
    {
        Stats[] partyStats = GameManager.instance.GetStats();
        Stats player = partyStats[0];

        activeBattlers[0].currentHP = player.currentHP;
        activeBattlers[0].maxHP = player.maxHP;
        activeBattlers[0].currentMP = player.currentMP;
        activeBattlers[0].maxMP = player.maxMP;
        activeBattlers[0].strength = player.strength;
        activeBattlers[0].defense = player.defense;

        UpdateUIStats();
    }
}
