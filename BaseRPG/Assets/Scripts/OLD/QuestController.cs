using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public string[] questMarkerNames;
    public bool[] questMarkersComplete;

    public static QuestController instance;

    // Start is called before the first frame update
    void Awake()
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoafQuestData();
        }
    }

    public int GetQuestId(string questToFind)
    {
        for(int i =0; i< questMarkerNames.Length; i++)
        {
            if (string.Equals(questMarkerNames[i], questToFind))
            {
                return i;
            }
        }
        Debug.LogError("Quest "+ questToFind + " not found");
        return 0;
    }

    public bool IsQuestComplete(string questToCheck)
    {
        if(GetQuestId(questToCheck) != 0)
        {
            return questMarkersComplete[GetQuestId(questToCheck)];
        }

        return false;
    }

    public void MarkQuestAsComplete(string completeQuest)
    {
        questMarkersComplete[GetQuestId(completeQuest)] = true;
        UpdateLocalQuestObjects();
    }

    public void MarkQuestAsIncomplete(string incompleteQuest)
    {
        questMarkersComplete[GetQuestId(incompleteQuest)] = false;
        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();
        if (questObjects.Length > 0)
        {
            for (int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    public void SaveQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_"+questMarkerNames[i],1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }

    public void LoafQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0;

            if(PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            if(valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            }
            else
            {
                questMarkersComplete[i] = true;
            }
        }
    }
}
