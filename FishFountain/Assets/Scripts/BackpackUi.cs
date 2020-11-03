using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUi : MonoBehaviour
{
    [SerializeField] Image[] slots = new Image[4];
    [SerializeField] TMP_Text countContent;
    [SerializeField] Backpack backpack;

    private void Awake()
    {
        backpack.UpdateUi += UpdatePanel;
    }

    public void UpdatePanel()
    {
        List<Fruit> fruits = backpack.GetBackpackContent();
        int fruitCount = fruits.Count;
        int index = 0;

        if (fruitCount < 1)
        {
            countContent.gameObject.SetActive(false);
            while (index < 4)
            {
                slots[index].gameObject.SetActive(false);
                index++;
            }
            return;
        }
        
        
        if(fruitCount < 4)
        {
            countContent.gameObject.SetActive(false);
            while (fruitCount > 0)
            {
                slots[index].gameObject.SetActive(true);
                slots[index].sprite = fruits[fruitCount - 1].GetSprite();
                fruitCount--;
                index++;
            }
            
            while(index < 4)
            {
                slots[index].gameObject.SetActive(false);
                index++;
            }
        }
        else
        {
            countContent.gameObject.SetActive(true);
            countContent.text = "+" + (fruitCount - 4);
            for (int i = 0; i < 4; i++)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].sprite = fruits[fruitCount - (i + 1)].GetSprite();
            }
        }
    }
}
