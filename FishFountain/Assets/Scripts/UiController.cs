using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    [SerializeField] GameObject resultsPanel;
    [SerializeField] GameObject gameOver;
    [SerializeField] TMP_Text[] fruitText = new TMP_Text[5];
    [SerializeField] TMP_Text totalWeight;

    [SerializeField] int[] fruitCounter = new int[5];

    private void CountFruits(List<Fruit> fruitList)
    {
        foreach(Fruit fruit in fruitList)
        {
            switch (fruit.GetFruitName())
            {
                case "Apple":
                    fruitCounter[0]++;
                    break;
                case "Pear":
                    fruitCounter[1]++;
                    break;
                case "Peach":
                    fruitCounter[2]++;
                    break;
                case "Cherry":
                    fruitCounter[3]++;
                    break;
                case "Orange":
                    fruitCounter[4]++;
                    break;
            }
        }
    }
    
    private void ShowValues(int weight)
    {
        for(int i = 0; i < fruitText.Length; i++)
        {
            fruitText[i].text = "x" + fruitCounter[i].ToString();
        }

        totalWeight.text = weight.ToString();
    }
    public void SetupResultsPanel(List<Fruit> fruits, int weight)
    {
        CountFruits(fruits);
        ShowValues(weight);
        resultsPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
