using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] soundEffect;
    [SerializeField] private AudioSource[] music;
    [SerializeField] private int musicIndex = -1;
    private readonly HashSet<Sound> suppressingEffects = new();
    private bool musicSuppressed;

    private void FixedUpdate()
    {
        PlaySequentially();
        RevokeSuppression();
    }

    public void PlaySoundEffect(Sound sound, bool suppressMusic = false)
    {
        if (suppressMusic)
        {
            music[musicIndex].Stop();
            suppressingEffects.Add(sound);
            musicSuppressed = true;
        }
        soundEffect[(int)sound].Play();
    }

    private void RevokeSuppression()
    {
        suppressingEffects.RemoveWhere(s => !soundEffect[(int)s].isPlaying);
        if (suppressingEffects.Count == 0)
        {
            musicSuppressed = false;
        }
    }

    private void PlayMusic(int index)
    {
        music[index].Play();
    }

    public void Play(int index, bool force)
    {
        if (force) {
            music[musicIndex].Stop();
        }
        musicIndex = index - 1;
    }
 
    private bool HasEnded(int index)
    {
        try
        {
            return !music[index].isPlaying;
        } catch {
            return true;
        }
    }

    private void PlaySequentially()
    {
        if (!HasEnded(musicIndex) || musicSuppressed)
            return;

        musicIndex = (musicIndex == music.Length - 1) ? 0 : musicIndex + 1;
        PlayMusic(musicIndex);
    }
}

public enum Sound
{
    BASIC_ATTACK = 0,
    BLEED = 1,
    COIN = 4,
    SUCCESS = 5,
}
