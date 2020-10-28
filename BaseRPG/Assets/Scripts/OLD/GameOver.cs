using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] int mainMenu = 3;
    [SerializeField] int lastScene = 4;

    // Start is called before the first frame update
    void Start()
    {
        AudioController.instance.PlayBackgroundMusic(4);
        PlayerController.playerInstance.gameObject.SetActive(false);
        MenuController.instance.gameObject.SetActive(false);
        BattleManager.instance.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.playerInstance.gameObject);
        Destroy(MenuController.instance.gameObject);
        Destroy(AudioController.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(mainMenu);
    }

    public void LoadLastSave()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.playerInstance.gameObject);
        Destroy(MenuController.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(lastScene);
    }
}
