using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityEngine.UI;
using System;

namespace RPG.Stats
{
    public class CharacterSheet : MonoBehaviour
    {
        [Header("General Info")]
        [SerializeField] int charId;
        [SerializeField] Sprite sprite;
        [SerializeField] string charName;
        [SerializeField] Characters charType;
        [SerializeField] Dictionary<int, SkillConfig> allSkillsByLevel;

        [Header("Combat Info")]
        [SerializeField] bool isAlive = true;
        [SerializeField] BaseStats stats;
        //Depois qm sabe lista com quests ou algo do tipo

        private void Awake()
        {
            if(stats == null)
            {
                stats = GetComponent<BaseStats>();
            }
        }

        #region PUBLIC METHODS
        public int GetCharId()
        {
            return charId;
        }
        public Sprite GetSprite()
        {
            return sprite;
        }
        public string GetCharName()
        {
            return charName;
        }
        public Characters GetCharType()
        {
            return charType;
        }
        public BaseStats GetStats()
        {
            return stats;
        }
        public SkillConfig GetSkillInLevel(int level)
        {
            if (allSkillsByLevel.ContainsKey(level))
            {
                return allSkillsByLevel[level];
            }
            else
            {
                return null;
            }
        }

        public Dictionary<int,SkillConfig> GetAllSkillsByLevel()
        {
            return allSkillsByLevel;
        }

        public int GetLevel()
        {
            return stats.GetLevel();
        }
        #endregion
    }
}

