using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    public static Fader instance;
    bool shouldFadeTo;
    bool shouldFadeFrom;

    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeTo)
        {
            FadeIn();
        }

        if (shouldFadeFrom)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, 1f * Time.deltaTime));
        
        if(fadeScreen.color.a == 1)
        {
            shouldFadeTo = false;
        }
    }

    private void FadeOut()
    {
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, 1f * Time.deltaTime));
        if (fadeScreen.color.a == 0)
        {
            shouldFadeFrom = false;
        }
    }

    public void SetFadeTo()
    {
        shouldFadeTo = true;
        shouldFadeFrom = false;
    }

    public void SetFadeToFrom()
    {
        shouldFadeFrom = true;
        shouldFadeTo = false;
    }
}
