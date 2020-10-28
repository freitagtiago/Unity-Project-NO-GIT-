using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Fruit")]
public class Fruit : ScriptableObject
{
    [Header("Info")]
    [SerializeField] string fruitName;
    [SerializeField] int weight;
    [SerializeField] Sprite sprite;
    [SerializeField] float spawnChance = 75f;

    public int GetWeight()
    {
        return weight;
    }
    public string GetFruitName()
    {
        return fruitName;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }

    public float GetSpawnChance()
    {
        return spawnChance;
    }
}
