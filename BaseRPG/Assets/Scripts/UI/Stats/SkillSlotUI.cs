using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;
using RPG.Core;
using TMPro;

namespace RPG.UI
{
    public class SkillSlotUI : MonoBehaviour
    {
        public TMP_Text skillName;
        public TMP_Text skillType;
        public TMP_Text skillTypeDamage;
        public TMP_Text skillCost;
        public TMP_Text skillInfo;
        public TMP_Text skillBasePower;

        public void Setup(SkillConfig skill)
        {
            skillName.text = skill.GetSkillName().ToUpper();
            skillTypeDamage.text = skill.GetSkillTypeDamage().ToString().ToUpper();
            skillCost.text = skill.GetCost().ToString().ToUpper();
            skillInfo.text = skill.GetDescription().ToUpper();
            skillType.text = skill.GetSkillType().ToString().ToUpper();
            skillBasePower.text = skill.GetDamage().ToString();
        }

    }

}