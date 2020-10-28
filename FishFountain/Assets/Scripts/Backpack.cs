using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public static Backpack instance;
    [SerializeField] List<Fruit> backpackContent = new List<Fruit>();
    [SerializeField] ControlBy controlBy = ControlBy.Unit;
    [SerializeField] int unitContentLimit = 10;
    [SerializeField] int weightContentLimit = 10;
    [SerializeField] int currentWeight = 0;


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    private bool CanAdd(Fruit fruit)
    {
        if(controlBy == ControlBy.Weight)
        {
            if ((currentWeight + fruit.GetWeight()) <= weightContentLimit)
            {
                return true;
            }
            return false;
        }
        else
        {
            if ((backpackContent.Count + 1) <= unitContentLimit)
            {
                return true;
            }
            return false;
        }
        
    }

    #region PUBLIC METHODS
    public bool IsFull()
    {
        if(controlBy == ControlBy.Weight)
        {
            if (currentWeight <= weightContentLimit)
            {
                return true;
            }

            return false;
        }
        else
        {
            if (backpackContent.Count >= unitContentLimit)
            {
                return true;
            }

            return false;
        }
    }

    public void AddToBackpack(Fruit fruit)
    {
        if (fruit == null || IsFull()) return;

        if (CanAdd(fruit))
        {
            if (controlBy == ControlBy.Weight)
            {
                currentWeight += fruit.GetWeight();
                //Atualizar UI com a fruta
                backpackContent.Add(fruit);
                currentWeight += fruit.GetWeight();
            }
            else
            {
                //Atualizar UI com a fruta
                backpackContent.Add(fruit);
                currentWeight += fruit.GetWeight();
            }
        }
    }

    public void RemoveFromBackpack()
    {
        if (backpackContent.Count == 0) return;

        int lastIndex = backpackContent.Count - 1;
        Fruit fruitToDrop = backpackContent[lastIndex];
        currentWeight -= fruitToDrop.GetWeight();
        //Atualizar UI sem a fruta
        //Spawnar fruta no mapa
        backpackContent.RemoveAt(lastIndex);
    }

    public void ResetBackpack()
    {
        backpackContent.Clear();
    }

    public List<Fruit> GetBackpackContent()
    {
        return backpackContent;
    }

    public int GetWeight()
    {
        return currentWeight;
    }
    #endregion
}
