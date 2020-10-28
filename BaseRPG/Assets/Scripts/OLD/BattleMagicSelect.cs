using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleMagicSelect : MonoBehaviour
{
    public string magicName;
    public int mpCost;
    public TMP_Text textMagicName;
    public TMP_Text textMPCost;

    public void Pressed()
    {
        if(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= mpCost)
        {
            BattleManager.instance.CloseMagicMenu();
            BattleManager.instance.OpenTargetMenu(magicName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= mpCost;
        }
        else
        {
            BattleManager.instance.battleNotice.Activate("Can't use "+ magicName +"\nNot enough MP");
        }
    }
}
