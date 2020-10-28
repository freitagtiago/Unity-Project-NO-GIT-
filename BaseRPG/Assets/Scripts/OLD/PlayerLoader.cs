using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.playerInstance == null)
        {
            Instantiate(player,Vector3.zero,Quaternion.identity);
        }
    }
}
