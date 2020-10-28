using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour 
{
    [Header("General")]
    [SerializeField] Sprite charImage;
    public string charName;

    [Header("Current Stats")]
    public int currentHP;
    public int currentMP;
    public int strength;
    public int defense;

    [Header("Max Stats")]
    public int maxHP;
    public int maxMP;

    [Header("XP Stats")]
    public int level = 1;
    public int currentXp;
    public int[] xpForLevel;
    [SerializeField] int maxLevel = 100;
    [SerializeField] int baseXp = 150;


    private void Awake()
    {
        SetupLevels();
    }

    private void Update()
    {
        //Test purpose
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GainXP(150);
        }
    }

    private void SetupLevels()
    {
        xpForLevel = new int[maxLevel];

        for (int i = 0; i < xpForLevel.Length; i++)
        {
            if (i >= 1)
            {
                xpForLevel[i] = Mathf.FloorToInt(xpForLevel[i - 1] * 1.05f);
            }
            else
            {
                xpForLevel[i] = baseXp;
            }

        }
    }

    public void GainXP(int amount)
    {
        if(level == maxLevel)
        {
            currentXp = 0;
            return;
        }
        currentXp += amount;

        if(currentXp > xpForLevel[level])
        {
            currentXp -= xpForLevel[level];

            level++;

            IncreaseStats();
        }
    }


    private void IncreaseStats()
    {
        if(level%2 == 0)
        {
            defense++;

            int maxmPbefore = maxMP;

            maxMP = Mathf.FloorToInt(maxMP * 1.12f);
            currentMP = Mathf.Clamp((maxMP - maxmPbefore), currentMP, maxMP);

        }
        else
        {
            strength++;
        }
        int maxHPbefore = maxHP;

        maxHP = Mathf.FloorToInt(maxHP * 1.12f);
        currentHP = Mathf.Clamp((maxHP - maxHPbefore),currentHP, maxHP);

    }

    public int GetCurrentHp()
    {
        return currentHP;
    }

}
