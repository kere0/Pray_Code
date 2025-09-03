 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bgm
{
    TitleBgm,
    Chapter1Bgm,
    Chapter2Bgm,
    DeathKnightBgm,
    FlameBossBgm
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    public AudioClip[] audioClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayBgm(Bgm.TitleBgm);
    }
    public void PlayBgm(Bgm bgm)
    {
        StartCoroutine(PlayBgmCoroutine(bgm));
    }
    IEnumerator PlayBgmCoroutine(Bgm bgm)
    {
        AudioClip clip = audioClip[(int)bgm];
        clip.LoadAudioData();
        while (clip.loadState != AudioDataLoadState.Loaded)
        {
            yield return null;
        }
        audioSource.clip = clip;
        audioSource.Play();
    }
}
