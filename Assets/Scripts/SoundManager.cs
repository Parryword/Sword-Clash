using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sound;

    public void playSound(Sound sound)
    {
        switch (sound)
        {
            case Sound.BASIC_ATTACK: this.sound[0].GetComponent<AudioSource>().Play(); break;
            case Sound.BLEED: this.sound[1].GetComponent<AudioSource>().Play(); break;
            case Sound.COIN: this.sound[3].GetComponent<AudioSource>().Play(); break;
        }
    }
}

public enum Sound
{
    BASIC_ATTACK,
    BLEED,
    COIN
}
