using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Combat
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] List<CharacterSheet> combatants;
        [SerializeField] Transform[] positionToSpawn;
        [SerializeField] StateTurns currentTurn;

        public void Setup()
        {
            //definir posições
            //pegar party e adicionar a lista de combatantes
        }



    }
}

