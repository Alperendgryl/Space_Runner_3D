using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource gameMusic, gameOver, coin, hit, meteorHit, rocket, explosion;
    public bool SFX_Muted;
    public GameObject mutedIMG;
    public GameObject sfxManager;

    public PlayerController playerController;

    private void Start()
    {
        if (PlayerPrefs.GetInt("SFX") == 1)
        {
            mute();
            mutedIMG.SetActive(true);
            SFX_Muted = true;
        }
        else
        {
            unmute();
            mutedIMG.SetActive(false);
            SFX_Muted = false;
        }
    }

    public void SFX_On_Off()
    {
        if (SFX_Muted)
        {
            mutedIMG.SetActive(false);
            SFX_Muted = false;
            unmute();
        }
        else
        {
            mutedIMG.SetActive(true);
            SFX_Muted = true;
            mute();
        }
    }

    private void mute()
    {
        sfxManager.SetActive(false);
        PlayerPrefs.SetInt("SFX", 1);
    }

    private void unmute()
    {
        sfxManager.SetActive(true);
        PlayerPrefs.SetInt("SFX", 0);
    }
}
