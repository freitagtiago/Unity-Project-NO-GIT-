using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int newGameSceneIndex;
    [SerializeField] int loadGameScene;
    [SerializeField] GameObject continueButton;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(false);
        }else
        {
            continueButton.SetActive(true);
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(loadGameScene);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameSceneIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
