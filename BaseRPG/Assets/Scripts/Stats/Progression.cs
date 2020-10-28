using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacter[] characterTypes;
        Dictionary<CharacterTypes, Dictionary<StatsAtributte, int[]>> lookupTable = null;

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterTypes, Dictionary<StatsAtributte, int[]>>();

            foreach (ProgressionCharacter progressionClass in characterTypes)
            {
                var statLookupTable = new Dictionary<StatsAtributte, int[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterTypes] = statLookupTable;
            }
        }

        #region PUBLIC METHODS
        public float GetStat(StatsAtributte stat, CharacterTypes charType, int level)
        {
            BuildLookup();
            int[] levels = lookupTable[charType][stat];

            if (levels.Length < level)
            {
                return 0;
            }
            return levels[level - 1];
        }

        public int GetLevels(StatsAtributte stat, CharacterTypes charTypes)
        {
            BuildLookup();

            int[] levels = lookupTable[charTypes][stat];
            return levels.Length;
        }
        #endregion


        [System.Serializable]
        class ProgressionCharacter
        {
            public CharacterTypes characterTypes;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public StatsAtributte stat;
            public int[] levels;
        }
    }
}

