using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BattleTargetSelection : MonoBehaviour
{
    public string moveName;
    public int activeBattleTarget;
    public TMP_Text targetName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressed()
    {
        BattleManager.instance.PlayerAttack(moveName, activeBattleTarget);
    }
}
