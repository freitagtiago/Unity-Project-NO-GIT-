using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    public string questToMark;
    public bool markComplete;
    public bool markOnEnter;
    bool canMark = false;
    bool deactivateOnMarking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMark && Input.GetButton("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canMark = false;
        }
    }

    void MarkQuest()
    {
        if (markComplete)
        {
            QuestController.instance.MarkQuestAsComplete(questToMark);
        }
        else
        {
            QuestController.instance.MarkQuestAsIncomplete(questToMark);
        }
        gameObject.SetActive(!deactivateOnMarking);
    }
}
