using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool isPlayer;
    public string charName;
    public int currentHP, maxHP, currentMP, maxMP, strength, defense;
    public bool isDead;
    public string[] movesAvailable;

    public SpriteRenderer spriteRenderer;
    public Sprite deadSprite;
    public Sprite aliveSprite;

    bool shouldFade = false;
    public float fadeSpeed = 1.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {
            spriteRenderer.color = new Color(Mathf.MoveTowards(spriteRenderer.color.r,1f, fadeSpeed * Time.deltaTime),
                                                Mathf.MoveTowards(spriteRenderer.color.g, 0f, fadeSpeed * Time.deltaTime),
                                                Mathf.MoveTowards(spriteRenderer.color.b, 0f, fadeSpeed * Time.deltaTime),
                                                Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (spriteRenderer.color.a == 0)
            {
                gameObject.SetActive(false);
            }

        }
        

    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
}
