using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField]
    private GameObject[] sound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public void playSound(Sound sound)
    {
        switch (sound)
        {
            case Sound.BASIC_ATTACK: this.sound[0].GetComponent<AudioSource>().Play(); break;
            case Sound.BLEED: this.sound[1].GetComponent<AudioSource>().Play(); break;
        }
    }
}

public enum Sound
{
    BASIC_ATTACK,
    BLEED
}
