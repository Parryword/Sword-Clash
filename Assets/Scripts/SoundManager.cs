using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource sfxSource;
    private AudioSource musicSource;
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] music;
    [SerializeField] private int musicIndex = -1;
    private readonly HashSet<AudioSource> suppressingEffects = new();

    private void Start()
    {
        sfxSource = GameObject.Find("SFX").GetComponent<AudioSource>();
        musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
        
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
        sfxSource.clip = sfx[(int)sound];
        var source = Instantiate(sfxSource);
        source.Play();
        StartCoroutine(DestroyAudioSource(source));
        
        if (suppressMusic)
        {
            musicSource.Stop();
            suppressingEffects.Add(source);
        }
        
        // Debug.Log(this.sfx[(int)sound].name);
    }

    IEnumerator DestroyAudioSource(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        suppressingEffects.Remove(source);
        Destroy(source.gameObject);
    }

    private void PlayMusic(int index)
    {
        musicSource.clip = music[index];
        musicSource.Play();
        // Debug.Log(music[index].name);
    }

    private bool IsPLaying()
    {
        try
        {   
            return musicSource.isPlaying;
        }
        catch
        {
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
    Coin = 1,
    Construction = 2,
    Bleed = 3
}