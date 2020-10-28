using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoopKeeper : MonoBehaviour
{
    bool canOpenMenu = false;
    public string[] itemsForSale = new string[40];

    private void FixedUpdate()
    {
        if(canOpenMenu && Input.GetButton("Fire1") && PlayerController.playerInstance.GetCanMove() && !Shop.instance.IsShopMenuOpen())
        {
            Shop.instance.itemsForSale = itemsForSale;
            PlayerController.playerInstance.CancelMovement();
            Shop.instance.OpenShopMenu();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpenMenu = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpenMenu = true;
        }
    }
}
