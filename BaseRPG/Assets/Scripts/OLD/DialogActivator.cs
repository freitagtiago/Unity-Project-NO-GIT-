using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    [SerializeField] string charName;
    [SerializeField] string[] dialogLines;
    bool canActivate;

    [SerializeField] bool shouldActivateQuest = false;
    [SerializeField] bool markComplete = false;
    [SerializeField] string questToMark;

    private void Update()
    {
        if(canActivate && Input.GetKeyDown(KeyCode.Space) && !GameManager.instance.GetIsDialogActive())
        {
            PlayerController.playerInstance.CancelMovement();
            DialogSystem.instance.ShowDialog(charName, dialogLines);
            DialogSystem.instance.ShouldActivateQuestAtEnd(questToMark,markComplete);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canActivate = false;
        }
    }
}
