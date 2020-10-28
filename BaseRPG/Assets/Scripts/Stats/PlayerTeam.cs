using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class PlayerTeam : MonoBehaviour
    {
        [SerializeField] CharacterSheet[] playerTeam = new CharacterSheet[4];
        [SerializeField] List<CharacterSheet> onlyCharactersAvailable;
        [SerializeField] List<CharacterSheet> allCharacters;


        private void Awake()
        {
            playerTeam[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSheet>();
        }

        private void SortTeam()
        {
            for(int i = 0; i < playerTeam.Length - 1; i++)
            if (playerTeam[i] == null)
            {
                    playerTeam[i] = playerTeam[i + 1];
                    playerTeam[i+1] = null;
            }
        }
        #region PUBLIC METHODS
        public void AddToPlayerTeam(CharacterSheet characterToAdd)
        {
            bool foundEmptySlot = false;
            for (int i = 0; i < playerTeam.Length; i++)
            {
                if(playerTeam[i].GetCharId() == characterToAdd.GetCharId())
                {
                    foundEmptySlot = false;
                    i = playerTeam.Length;
                    Debug.LogError(characterToAdd.GetCharName() + " is already in team");
                }

                if(playerTeam[i] == null)
                {
                    playerTeam[i] = characterToAdd;
                    foundEmptySlot = true;
                    i = playerTeam.Length;
                }
            }

            if (!foundEmptySlot)
            {
                Debug.LogError("There's no slot available");
            }
        }

        public void RemoveFromPlayerTeam(CharacterSheet characterToRemove)
        {
            for(int i = 0; i < playerTeam.Length; i++)
            {
                if(playerTeam[i].GetCharId() == characterToRemove.GetCharId())
                {
                    playerTeam[i] = null;
                }
            }
            SortTeam();
        }

        public CharacterSheet[] GetPlayerTeam()
        {
            return playerTeam;
        }

        public CharacterSheet GetSpecificMember(int index)
        {
            return playerTeam[index];
        }

        #endregion
    }
}

