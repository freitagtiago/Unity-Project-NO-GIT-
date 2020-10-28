using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;
    public TMP_Text expText;
    public TMP_Text itemText;
    public GameObject rewardPanel;

    public string[] rewardItems;
    public int xpEarned;

    public bool markQuestComplete = false;
    public string questToMark;

    private void Awake()
    {
        if(instance == null)
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
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardPanel(100, new string[] {"HealthPotion", "Iron Sword" });
        }
    }

    public void OpenRewardPanel(int xp, string[] rewards)
    {
        xpEarned = xp;
        rewardItems = rewards;
        expText.text = "Each one received " + xpEarned +" exp. points";
        itemText.text = "";

        for(int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewardItems[i] + "\n";        
        }

        rewardPanel.SetActive(true);

    }

    public void CloseRewardPanel()
    {
        for (int i = 0; i < GameManager.instance.GetStats().Length; i++)
        {
            if (GameManager.instance.GetStats()[i] != null && GameManager.instance.GetStats()[i].gameObject.activeInHierarchy)
            { 
                Stats stat = GameManager.instance.GetStats()[i];
                stat.currentXp += xpEarned;

                GameManager.instance.SetStats(stat, i);
            }
        }

        for(int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i],1);
        }

        GameManager.instance.SetIsBattleActive(false);
        if (markQuestComplete)
        {
            QuestController.instance.MarkQuestAsComplete(questToMark);
        }
        rewardPanel.SetActive(false);
    }
}
