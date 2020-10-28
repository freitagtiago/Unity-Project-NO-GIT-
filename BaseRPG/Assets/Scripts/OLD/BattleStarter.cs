using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    public BattleType[] potentialBattles;
    private bool isInArea;
    public bool activateOnEnter;
    public bool activateOnStay;
    public bool activateOnExit;
    public float timeBetweenBattles = 5f;
    private float timeCounter;
    public bool deactivateAfterStart = false;
    public bool canFlee = false;

    public bool shouldCompleteQuest = false;
    public string questToComplete;


    private void Start()
    {
        timeCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }

    private void Update()
    {
        if(isInArea && PlayerController.playerInstance.GetCanMove())
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                timeCounter -= Time.deltaTime;
            }

            if(timeCounter <= 0)
            {
                timeCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
                StartCoroutine(StartBattle());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(activateOnEnter == true)
            {
                StartCoroutine(StartBattle());
            }
            else
            {
                isInArea = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            isInArea = false;
            if (activateOnExit== true)
            {
                StartCoroutine(StartBattle());
            }
            else
            {
                isInArea = false;
            }
        }
    }

    private IEnumerator StartBattle()
    {

        Fader.instance.SetFadeTo();
        GameManager.instance.SetIsBattleActive(true);

        int selectedBattle = Random.Range(0, potentialBattles.Length);
        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies,canFlee);
        Fader.instance.SetFadeToFrom();
        
        if(deactivateAfterStart == true)
        {
            gameObject.SetActive(false);
        }

        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = questToComplete;
    }

}
