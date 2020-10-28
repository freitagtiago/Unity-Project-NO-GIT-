using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Inventories
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] Sprite sprite;
        [SerializeField] ItemConfig item;
        [SerializeField] int amount;
        [SerializeField] GameObject tooltip;
        [SerializeField] float rangeToShowTooltip = 2.5f;
        bool isInRange = false;
        bool canCollect = false;


        private void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rangeToShowTooltip);
            if(colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Player"))
                    {
                        isInRange = true;
                        i = colliders.Length;
                    }
                    else
                    {
                        isInRange = false;
                    }
                }
            }
            else
            {
                isInRange = false;
            }
            if (isInRange)
            {
                if (!tooltip.activeInHierarchy)
                {
                    ShowToolTip();
                }
            }
            else
            {
                if (tooltip.activeInHierarchy)
                {
                    HideTooltip();
                }
            }

            if (canCollect && (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
            {
                InteractWith();
            }

        }

        private void InteractWith()
        {
            FindObjectOfType<Inventory>().AddItem(item, amount);
            gameObject.SetActive(false);
        }

        private void ShowToolTip()
        {
            isInRange = true;
            tooltip.SetActive(true);
        }

        private void HideTooltip()
        {
            isInRange = false;
            tooltip.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canCollect = true;   
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            canCollect = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rangeToShowTooltip);
        }
    }

}
