using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject[] sound;
    [SerializeField] private GameObject[] music;
    [SerializeField] private int musicIndex = -1;

    private void FixedUpdate()
    {
        PlaySequentially();
    }

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.BASIC_ATTACK: this.sound[0].GetComponent<AudioSource>().Play(); break;
            case Sound.BLEED: this.sound[1].GetComponent<AudioSource>().Play(); break;
            case Sound.COIN: this.sound[3].GetComponent<AudioSource>().Play(); break;
        }
    }

    private void PlayMusic(int index)
    {
        music[index].GetComponent<AudioSource>().Play();
    }

    public void Play(int index, bool force)
    {
        if (force) {
            music[musicIndex].GetComponent<AudioSource>().Stop();
        }
        musicIndex = index - 1;
    }
 
    private bool HasEnded(int index)
    {
        try
        {
            return !music[index].GetComponent<AudioSource>().isPlaying;
        } catch {
            return true;
        }
    }

    private void PlaySequentially()
    {
        if (HasEnded(musicIndex))
        {
            if (musicIndex == music.Length - 1)
            {
                musicIndex = 0;
            }
            else
            {
                musicIndex++;
            }
            PlayMusic(musicIndex);
        }
    }
}

public enum Sound
{
    BASIC_ATTACK,
    BLEED,
    COIN
}
