using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] TMP_Text charName;
    [SerializeField] TMP_Text dialogTextBox;
    [SerializeField] GameObject dialogPanel;
    [SerializeField] GameObject nameCharPanel;

    [SerializeField] string[] dialogLines;
    [SerializeField] int currentLine;

    [SerializeField] string questToMark;
    bool markQuestComplete = false;
    bool shouldMarkQuest = false;

    bool beginDialog = false;

    public static DialogSystem instance;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (dialogPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(beginDialog == false)
                {
                    if (currentLine < (dialogLines.Length - 1))
                    {
                        currentLine++;
                        dialogTextBox.text = dialogLines[currentLine];
                    }
                    else
                    {
                        GameManager.instance.SetIsDialogActive(false);
                        dialogPanel.SetActive(false);

                        if (shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            if (markQuestComplete)
                            {
                                QuestController.instance.MarkQuestAsComplete(questToMark);
                            }
                            else
                            {
                                QuestController.instance.MarkQuestAsIncomplete(questToMark);
                            }
                        }
                        return;
                        
                    }
                }
                else
                {
                    beginDialog = false;
                }  
            }
        }
    }

    public void ShowDialog(string characterName, string[] dialogText)
    {
        GameManager.instance.SetIsDialogActive(true);

        if(String.IsNullOrEmpty(characterName))
        {
            nameCharPanel.SetActive(false);
        }
        else
        {
            nameCharPanel.SetActive(true);
            charName.text = characterName;
        }
        
        dialogLines = dialogText;
        currentLine = 0;

        dialogPanel.SetActive(true);
        dialogTextBox.text = dialogLines[currentLine];
        beginDialog = true;
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
