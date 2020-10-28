using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource[] backgroundMusic;
    [SerializeField] AudioSource[] sfx;
    public static AudioController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int soundToPlay)
    {
        if(soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void PlayBackgroundMusic(int musicToPlay)
    {
        if(musicToPlay < backgroundMusic.Length)
        {
            if (backgroundMusic[musicToPlay].isPlaying) return;

            StopAllMusic();
            backgroundMusic[musicToPlay].Play();
        }
    }

    public void StopAllMusic()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            backgroundMusic[i].Stop();
        }
    }
}
