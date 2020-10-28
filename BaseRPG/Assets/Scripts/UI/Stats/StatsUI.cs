using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;
using RPG.Combat;
using RPG.Inventories;

namespace RPG.UI
{
    public class StatsUI : MonoBehaviour
    {
        [Header("Party Members Info")]
        [SerializeField] PlayerTeam playerTeam;
        [SerializeField] CharacterSheet characterSelected;

        [Header("UI Elements - Party Panels")]
        [SerializeField] private GameObject[] partyPanels;
        [SerializeField] private AllPartyMemberUI[] partyMemberInfo;
        [SerializeField] private GameObject specificMemberInfo;
        [SerializeField] GameObject skillPanel;

        [Header("UI Elements - Selected Party Member")]
        [SerializeField] private PartyMemberUISelected partyMember;
        [SerializeField] SkillSlotUI skillPrefab;
        

        int selectedindex;

        #region STRUCTS
        [System.SerializableAttribute]
        public struct AllPartyMemberUI
        {
            public TMP_Text name;
            public TMP_Text type;
            public TMP_Text level;
            public TMP_Text hp;
            public Slider hpSlider;
            public TMP_Text sp;
            public Slider spSlider;
            public TMP_Text statusCondition;
            public Image charImage;
        }

        [System.SerializableAttribute]
        public struct EquippedItemSlots
        {
            public TMP_Text itemName;
            public TMP_Text itemInfo;
            public ItemConfig item;
            public GameObject unequipButton;
        }

        [System.SerializableAttribute]
        public struct PartyMemberUISelected
        {
            public TMP_Text charName;
            public Image charSprite;
            public TMP_Text level;
            public TMP_Text charType;
            public TMP_Text statusCondition;
            public TMP_Text hp;
            public TMP_Text sp;
            public TMP_Text atack;
            public TMP_Text defense;
            public TMP_Text magicAttack;
            public TMP_Text magicDefense;
            public TMP_Text lucky;
            public TMP_Text speed;
            public TMP_Text xpToLevelUp;
            public Slider xpToLevelUpSlider;
            public EquippedItemSlots[] equippedItemSlot;
        }
        #endregion
        private void Start()
        {
            if(playerTeam == null)
            {
                playerTeam = GameObject.FindWithTag("Player").GetComponent<PlayerTeam>();
            }
        }

        private void LoadStatsInfo(BaseStats charStats)
        {
            specificMemberInfo.SetActive(true);
            partyMember.charName.text = characterSelected.GetCharName().ToUpper();
            partyMember.level.text = "LVL " + characterSelected.GetLevel();
            partyMember.charSprite.sprite = characterSelected.GetSprite();
            partyMember.charType.text = charStats.GetStatType().ToString().ToUpper();
            if (charStats.GetStatusCondition() == StatusCondition.None)
            {
                partyMember.statusCondition.gameObject.SetActive(false);
            }
            else
            {
                partyMember.statusCondition.text = charStats.GetStatusCondition().ToString().ToUpper();
            }
            partyMember.hp.text = charStats.GetStat(StatsAtributte.Health) + "/" + charStats.GetStat(StatsAtributte.MaxHealth);
            partyMember.sp.text = charStats.GetStat(StatsAtributte.SkillPoints) + "/" + charStats.GetStat(StatsAtributte.MaxSkillPoints);
            partyMember.atack.text = charStats.GetStat(StatsAtributte.Attack).ToString();
            partyMember.defense.text = charStats.GetStat(StatsAtributte.Defense).ToString();
            partyMember.magicAttack.text = charStats.GetStat(StatsAtributte.MagicAttack).ToString();
            partyMember.magicDefense.text = charStats.GetStat(StatsAtributte.MagicDefense).ToString();
            partyMember.lucky.text = charStats.GetStat(StatsAtributte.Lucky).ToString();
            partyMember.speed.text = charStats.GetStat(StatsAtributte.Speed).ToString();
            partyMember.xpToLevelUp.text = charStats.GetStat(StatsAtributte.ExperienceReward) + "/" + charStats.GetStat(StatsAtributte.ExperienceToLevelUp).ToString();
            partyMember.xpToLevelUpSlider.maxValue = charStats.GetStat(StatsAtributte.ExperienceToLevelUp);
            partyMember.xpToLevelUpSlider.value = charStats.GetStat(StatsAtributte.ExperienceReward);
        }

        private void LoadSkillsInfo(BaseStats charInfo)
        {
            foreach (Transform child in skillPanel.transform)
            {
                Destroy(child.gameObject);
            }

            foreach(SkillConfig skill in charInfo.GetAllAvailableSkills())
            {
                var skillUI = Instantiate(skillPrefab, transform);
                skillUI.transform.parent = skillPanel.transform;
                skillUI.Setup(skill);
            }
        }
        private void SetupEquippedItemsPanel(BaseStats charStats)
        {
            ItemConfig item;
            for(int i = 0; i < partyMember.equippedItemSlot.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        item = charStats.GetEquippedItem(CharEquipmentSlot.LeftHand);
                        if(item != null)
                        {
                            partyMember.equippedItemSlot[i].itemName.text = item.GetDisplayName().ToUpper();
                            partyMember.equippedItemSlot[i].itemInfo.text = item.GetDescription().ToUpper();
                            partyMember.equippedItemSlot[i].item = item;
                            partyMember.equippedItemSlot[i].unequipButton.SetActive(true);
                        }
                        else
                        {
                            partyMember.equippedItemSlot[i].itemName.text = "NONE";
                            partyMember.equippedItemSlot[i].itemInfo.text = "NO ITEM EQUIPPED";
                            partyMember.equippedItemSlot[i].item = null;
                            partyMember.equippedItemSlot[i].unequipButton.SetActive(false);
                        }
                        break;
                    case 1:
                        item = charStats.GetEquippedItem(CharEquipmentSlot.Book);
                        if (item != null)
                        {
                            partyMember.equippedItemSlot[i].itemName.text = item.GetDisplayName().ToUpper();
                            partyMember.equippedItemSlot[i].itemInfo.text = item.GetDescription().ToUpper();
                            partyMember.equippedItemSlot[i].item = item;
                            partyMember.equippedItemSlot[i].unequipButton.SetActive(true);
                        }
                        else
                        {
                            partyMember.equippedItemSlot[i].itemName.text = "NONE";
                            partyMember.equippedItemSlot[i].itemInfo.text = "NO BOOK EQUIPPED";
                            partyMember.equippedItemSlot[i].item = null;
                            partyMember.equippedItemSlot[i].unequipButton.SetActive(false);
                        }
                        break;
                    case 2:
                        item = charStats.GetEquippedItem(CharEquipmentSlot.RightHand);
                        if (item != null)
                        {
                            partyMember.equippedItemSlot[i].itemName.text = item.GetDisplayName().ToUpper();
                            partyMember.equippedItemSlot[i].itemInfo.text = item.GetDescription().ToUpper();
                            partyMember.equippedItemSlot[i].item = item;
                            partyMember.equippedItemSlot[i].unequipButton.SetActive(true);
                        }
                        else
                        {
                            partyMember.equippedItemSlot[i].itemName.text = "NONE";
                            partyMember.equippedItemSlot[i].itemInfo.text = "NO ITEM EQUIPPED";
                            partyMember.equippedItemSlot[i].item = null;
                            partyMember.equippedItemSlot[i].unequipButton.SetActive(false);
                        }
                        break;
                    default: return;
                }
            }
        }

        #region PUBLIC METHODS
        public void LoadPartyPanels()
        {
            if (playerTeam == null)
            {
                playerTeam = GameObject.FindWithTag("Player").GetComponent<PlayerTeam>();
            }
            if (playerTeam != null)
            {
                CharacterSheet[] team = playerTeam.GetPlayerTeam();
                if (team != null)
                {
                    for (int i = 0; i < team.Length; i++)
                    {
                        if (team[i] != null)
                        {
                            partyPanels[i].SetActive(true);
                            BaseStats currentStats = team[i].GetStats();
                            partyMemberInfo[i].name.text = team[i].GetCharName().ToUpper();
                            partyMemberInfo[i].type.text = currentStats.GetStatType().ToString().ToUpper();
                            partyMemberInfo[i].level.text = "LVL " + currentStats.GetLevel();
                            partyMemberInfo[i].hp.text = currentStats.GetStat(StatsAtributte.Health) + "/" + currentStats.GetStat(StatsAtributte.MaxHealth);
                            partyMemberInfo[i].hpSlider.maxValue = currentStats.GetStat(StatsAtributte.MaxHealth);
                            partyMemberInfo[i].hpSlider.value = currentStats.GetStat(StatsAtributte.Health);
                            partyMemberInfo[i].sp.text = currentStats.GetStat(StatsAtributte.SkillPoints) + "/" + currentStats.GetStat(StatsAtributte.MaxSkillPoints); ;
                            partyMemberInfo[i].spSlider.maxValue = currentStats.GetStat(StatsAtributte.MaxSkillPoints);
                            partyMemberInfo[i].spSlider.value = currentStats.GetStat(StatsAtributte.SkillPoints);
                            partyMemberInfo[i].charImage.sprite = team[i].GetSprite();
                            if (currentStats.GetStatusCondition() != StatusCondition.None)
                            { 
                                partyMemberInfo[i].statusCondition.gameObject.SetActive(true);
                                partyMemberInfo[i].statusCondition.text = currentStats.GetStatusCondition().ToString().ToUpper();
                            }
                            else
                            {
                                partyMemberInfo[i].statusCondition.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            partyPanels[i].SetActive(false);
                        }
                    }
                }
            }
        }
        
        public void LoadSpecificMemberInfo(int index)
        {
            selectedindex = index;
            characterSelected = playerTeam.GetSpecificMember(index);
            BaseStats charStats = characterSelected.GetStats();

            if(characterSelected != null)
            {
                LoadStatsInfo(charStats);
                LoadSkillsInfo(charStats);
                SetupEquippedItemsPanel(charStats);
            }
        }

        public void Unnequip(int equippedOn)
        {
            ItemConfig itemToUnnequip = partyMember.equippedItemSlot[equippedOn].item;
            StatsAtributte stat = itemToUnnequip.GetAttributeToModify();
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            switch (equippedOn)
            {
                case 0:
                    characterSelected.GetStats().ReturnAfterUnnequip(stat, itemToUnnequip, CharEquipmentSlot.LeftHand);
                    break;
                case 1:
                    characterSelected.GetStats().ReturnAfterUnnequip(stat, itemToUnnequip, CharEquipmentSlot.Book);
                    break;
                case 2:
                    characterSelected.GetStats().ReturnAfterUnnequip(stat, itemToUnnequip, CharEquipmentSlot.RightHand);
                    break;
                default: return;
            }
            inventory.AddItem(itemToUnnequip, 1);
            LoadSpecificMemberInfo(selectedindex);
        }

        public void Close()
        {
            selectedindex = 0;
            specificMemberInfo.SetActive(false);
        }
        #endregion
    }
}


