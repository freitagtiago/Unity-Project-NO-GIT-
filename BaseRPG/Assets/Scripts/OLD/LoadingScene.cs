using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] float waitToLoad;
    // Start is called before the first frame update
    void Update()
    {
        if(waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Current_Scene"));
                GameManager.instance.LoadData();
                QuestController.instance.LoafQuestData();
            }
        }
    }
}
