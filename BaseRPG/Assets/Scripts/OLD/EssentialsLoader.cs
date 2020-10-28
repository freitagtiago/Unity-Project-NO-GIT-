using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject audioController;
    [SerializeField] GameObject battleManager;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.playerInstance == null)
        {
            Instantiate(player);
        } else if (!PlayerController.playerInstance.gameObject.activeInHierarchy)
        {
            PlayerController.playerInstance.gameObject.SetActive(true);
        }
        
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
        if (AudioController.instance == null)
        {
            Instantiate(audioController);
        }
        if (BattleManager.instance == null)
        {
            Instantiate(battleManager);
        }
    }

}
