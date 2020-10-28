using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "SkillConfig")]
    public class SkillConfig : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private int id;
        [SerializeField] private string skillName;
        [SerializeField][TextArea] private string description;
        [SerializeField] private int cost = 0;
        [SerializeField] private int damage = 0;
        [SerializeField] private CharacterTypes skillType;
        [SerializeField] private SkillTypeDamage skillTypeDamage;
        [SerializeField] private StatusCondition statusCondition;
        [SerializeField] private StatsAtributte statAffected;
        [SerializeField] private int chanceOfStatus = 0;
        [SerializeField] private bool usePercentage;
        [SerializeField] private float percentage = 0;

        #region PUBLIC METHODS
        public string GetSkillName()
        {
            return skillName;
        }

        public string GetDescription()
        {
            return description;
        }

        public int GetCost()
        {
            return cost;
        }

        public int GetDamage()
        {
            return damage;
        }

        public CharacterTypes GetSkillType()
        {
            return skillType;
        }

        public SkillTypeDamage GetSkillTypeDamage()
        {
            return skillTypeDamage;
        }

        public StatusCondition GetStatusCondition()
        {
            return statusCondition;
        }

        public StatsAtributte GetStatAffected()
        {
            return statAffected;
        }

        public int GetChanceOfStatus()
        {
            return chanceOfStatus;
        }
        public bool GetUsePercentage()
        {
            return usePercentage;
        }
        public float GetPercentage()
        {
            return percentage;
        }



        #endregion
    }
}

