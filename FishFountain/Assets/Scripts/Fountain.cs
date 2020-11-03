using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    bool inRange = false;
    FishController controller;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<FishController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetKeyDown(KeyCode.Space)) && inRange)
        {
            InteractWithPlayer();
        }
        
    }

    private void InteractWithPlayer()
    {
        controller.RestoreBreath();
        gameManager.InteractWithFountain();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

}
