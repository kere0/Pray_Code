using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance;
    private PlayableDirector playableDirector;
    [SerializeField] private TimelineAsset IntroTimelineAsset;
    [SerializeField] private TimelineAsset DeathKnightTimelineAsset;
    [SerializeField] private TimelineAsset FlameBossTimelineAsset;
    
    public Func<Task> introAction;
    public Func<Task> deathKnightAction;
    public Func<Task> flameBossAction;

    private Dictionary<TimelineAsset, Func<Task>> timelineDic;
    
    public IntroSequenceController introSequenceController;
    private void Awake()
    {
        Instance = this;
        TryGetComponent(out playableDirector);
        playableDirector.played -= OnTimelinePlayed;
        playableDirector.played += OnTimelinePlayed;
    }
    public void PlayIntro()
    {
        playableDirector.playableAsset = IntroTimelineAsset;
        playableDirector.Play();
    }
    public void KnightBossCutScene()
    {
        playableDirector.playableAsset = DeathKnightTimelineAsset;
        SoundManager.Instance.PlayBgm(Bgm.DeathKnightBgm);
        playableDirector.Play();
    }
    public void PlayFlameBossCutScene()
    {
        playableDirector.playableAsset = FlameBossTimelineAsset;
        SoundManager.Instance.PlayBgm(Bgm.FlameBossBgm);
        playableDirector.Play();
    }
    private async void OnTimelinePlayed(PlayableDirector director)
    {
        if (director.playableAsset == IntroTimelineAsset)
        {
            await introAction?.Invoke();
        }
        else if (director.playableAsset == DeathKnightTimelineAsset)
        {
            await deathKnightAction?.Invoke();
        }
        else if (director.playableAsset == FlameBossTimelineAsset)
        {
            await flameBossAction?.Invoke();
        }
    }
}
