using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{
    [SerializeField] Fruit fruit;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Backpack backpack;
    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = fruit.GetSprite();
        gameManager = FindObjectOfType<GameManager>();
        backpack = FindObjectOfType<Backpack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            backpack.AddToBackpack(fruit);
            gameManager.ColectFruit(fruit);
            Destroy(gameObject);
        }
    }

    public Fruit GetFruit()
    {
        return fruit;
    }

    public void SetupFruit(Fruit fruit)
    {
        this.fruit = fruit;
    }
}
