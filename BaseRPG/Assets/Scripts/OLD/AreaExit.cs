using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int sceneToLoad;
    [SerializeField] string areaToTransition;

    [SerializeField] AreaEntrance areaEntrance;

    float waitToLoad = 1f;
    bool shouldLoadAfterFade;

    private void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") 
        {
            PlayerController.playerInstance.CancelMovement();
            GameManager.instance.SetIsBetweenArea(true);
            Fader.instance.SetFadeTo();
            shouldLoadAfterFade = true;
            PlayerController.playerInstance.areaTransition = areaToTransition;

        }
        
    }
}
