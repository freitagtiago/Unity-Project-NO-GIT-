using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour
    {
        private int currentXPPoints = 0;
        public Action OnExperienceGained;

        #region PUBLIC METHODS
        public void GainExperience(int experience)
        {
            currentXPPoints += experience;
            OnExperienceGained();
        }
        public int GetExperience()
        {
            return currentXPPoints;
        }
        #endregion
    }
}

