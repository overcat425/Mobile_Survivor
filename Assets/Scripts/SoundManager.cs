using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmSource;
    AudioSource mainSource;
    AudioHighPassFilter bgmHighPassFilter;

    [Header("EFFECT")]
    public AudioClip[] effClips;
    public float effVolume;
    AudioSource[] effSources;
    public int channels;
    int channel;
    public enum Effect { Die, Hit, LvUp=3, GameOver, Melee, Range=7, Select, Win, Click, Notice }

    private void Awake()
    {
        instance = this;
        Init();
    }
    void Init()         // 소리 초기화
    {
        GameObject bgmObject = new GameObject("BgmSource");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.clip = bgmClip;
        bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject effObject = new GameObject("EffSource");
        effObject.transform.parent = transform;
        effSources = new AudioSource[channels];

        for (int i = 0; i < effSources.Length; i++)
        {
            effSources[i] = effObject.AddComponent<AudioSource>();
            effSources[i].playOnAwake = false;
            effSources[i].bypassListenerEffects = true; // 오디오하이패스필터 적용
            effSources[i].volume = effVolume;
        }
    }
    public void PlayEffect(Effect effect)
    {
        for (int i = 0; i < effSources.Length; ++i)
        {
            int loopChannel = (i + channel) % effSources.Length;
            if (effSources[loopChannel].isPlaying)
                continue;
            int randSound = 0;      // 랜덤사운드
            if (effect == Effect.Hit || effect == Effect.Melee)
            {
                randSound = Random.Range(0, 2);
            }
            channel = loopChannel;
            effSources[loopChannel].clip = effClips[(int)effect];
            effSources[loopChannel].Play();
                break;
        }
    }
    public void PlayBgm(bool isPlaying)
    {
        if (isPlaying)
        {
            bgmSource.Play();
        }else
        {
            bgmSource.Stop();
        }
    }
    public void StopBgm(bool isPlaying)
    {
        bgmHighPassFilter.enabled = isPlaying;
    }
}
