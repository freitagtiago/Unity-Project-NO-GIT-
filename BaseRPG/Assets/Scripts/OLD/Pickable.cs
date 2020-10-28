using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private bool canPickup = false;


    // Update is called once per frame
    void Update()
    {
        if(canPickup && Input.GetButtonDown("Fire1") && PlayerController.playerInstance.GetCanMove())
        {
            GameManager.instance.AddItem(GetComponent<Item>().GetName(),1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}
