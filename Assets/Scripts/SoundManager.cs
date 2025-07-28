using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] soundEffect;
    [SerializeField] private AudioSource[] music;
    [SerializeField] private int musicIndex = -1;
    private readonly HashSet<AudioSource> suppressingEffects = new();

    private void Awake()
    {
        var list = music.ToList();
        list = list.OrderBy(x => UnityEngine.Random.value).ToList();
        music = list.ToArray();
    }

    private void FixedUpdate()
    {
        PlaySequentially();
    }
    
    public void PlaySoundEffect(Sound sound, bool suppressMusic = false)
    {

        var sfx = Instantiate(soundEffect[(int)sound]);
        sfx.Play();
        StartCoroutine(DestroyAudioSource(sfx));
        
        if (suppressMusic)
        {
            music[musicIndex].Stop();
            suppressingEffects.Add(sfx);
        }
        Debug.Log(soundEffect[(int)sound].name);
    }

    IEnumerator DestroyAudioSource(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        suppressingEffects.Remove(source);
        Destroy(source.gameObject);
    }

    private void PlayMusic(int index)
    {
        music[index].Play();
        Debug.Log(music[index].name);
    }

    public void Play(int index, bool force)
    {
        if (force)
        {
            music[musicIndex].Stop();
        }

        musicIndex = index - 1;
    }

    private bool IsPLaying()
    {
        try
        {   
            // Debug.Log(music[0].isPlaying + " " + music[1].isPlaying);
            return music.ToList().Exists(m => m.isPlaying);
        }
        catch
        {
            // Debug.Log("error");
            return false;
        }
    }

    private void PlaySequentially()
    {
        if (IsPLaying() || suppressingEffects.Count > 0)
            return;

        musicIndex = (musicIndex == music.Length - 1) ? 0 : musicIndex + 1;
        PlayMusic(musicIndex);
    }
}

public enum Sound
{
    BasicAttack = 0,
    Coin = 4,
    Success = 5,
    Construction = 6,
    Bleed = 7
}