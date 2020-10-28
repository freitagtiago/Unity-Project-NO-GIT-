using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Core
{
    public class TimeEffectController : MonoBehaviour
    {
        bool isGainXPActive = false;
        bool isIncreaseMoneyActive = false;
        bool isRepelEnemyActive = false;

       public IEnumerator GainXPRoutine()
        {
            isGainXPActive = true;
            yield return new WaitForSeconds(260f);
            isGainXPActive = false;
        }

        public IEnumerator IncreaseMoneyRoutine()
        {
            isIncreaseMoneyActive = true;
            yield return new WaitForSeconds(260f);
            isIncreaseMoneyActive = false;
        }

        public IEnumerator RepelEnemyRoutine()
        {
            isRepelEnemyActive = true;
            yield return new WaitForSeconds(260f);
            isRepelEnemyActive = false;
        }
    }
}

