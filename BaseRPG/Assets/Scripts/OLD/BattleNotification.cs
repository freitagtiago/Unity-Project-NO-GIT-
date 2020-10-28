using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleNotification : MonoBehaviour
{
    public float awakeTime;
    float awakeCounter;
    public TMP_Text notificationText;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(awakeCounter);
        if(awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;
            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Activate(string textToShow)
    {
        gameObject.SetActive(true);
        notificationText.text = textToShow;
        awakeCounter = awakeTime;

    }
}
