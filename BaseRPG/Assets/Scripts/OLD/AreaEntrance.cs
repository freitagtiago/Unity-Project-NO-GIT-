using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;

    private void Start()
    {
        if (String.Equals(transitionName, PlayerController.playerInstance.areaTransition))
        {
            PlayerController.playerInstance.TransportTo(transform.position);
        }

        Fader.instance.SetFadeToFrom();
        GameManager.instance.SetIsBetweenArea(false);
    }
}
