using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource[] bgmSources;
    AudioHighPassFilter bgmHighPassFilter;

    [Header("EFFECT")]
    public AudioClip[] effClips;
    public float effVolume;
    AudioSource[] effSources;
    public int channels;
    int channel;                        // ȿ���� ������ ����ġ
    public enum Effect { Die, Hit, LvUp=3, GameOver, Melee, Range=7, Select, Win, Click, Notice, Coin, Bounce }

    private void Awake()
    {
        instance = this;
        Init();
    }
    void Init()         // �Ҹ� �ʱ�ȭ
    {
        GameObject bgmObject = new GameObject("BgmSource");
        bgmObject.transform.parent = transform;     // �ڽ����� ��ġ
        bgmSources = new AudioSource[2];        // ����ȭ���, ������ ��� 2��
        for (int i = 0; i < bgmSources.Length; i++)
        {
            bgmSources[i] = bgmObject.AddComponent<AudioSource>();
            bgmSources[i].playOnAwake = false;
            bgmSources[i].loop = true;
            bgmSources[i].volume = bgmVolume;
            bgmSources[i].clip = bgmClips[i];
        }
        bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject effObject = new GameObject("EffSource");
        effObject.transform.parent = transform;
        effSources = new AudioSource[channels];
        for (int i = 0; i < effSources.Length; i++)
        {
            effSources[i] = effObject.AddComponent<AudioSource>();
            effSources[i].playOnAwake = false;
            effSources[i].bypassListenerEffects = true; // ����������н����� ����
            effSources[i].volume = effVolume;
        }
    }
    public void PlayEffect(Effect effect)
    {
        for (int i = 0; i < effSources.Length; ++i)
        {
            int loopChannel = (i + channel) % effSources.Length;
            if (effSources[loopChannel].isPlaying)      // ���� �����������Ʈ ��� ����
                continue;
            int randSound = 0;      // ��������
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
    public void PlayBgm(bool isPlaying)     // ��� ����
    {
        if (isPlaying)
        {
            bgmSources[1].Stop();
            bgmSources[0].Play();
        }else
        {
            bgmSources[0].Stop();
            bgmSources[1].Play();
        }
    }
    public void StopBgm(bool isPlaying)
    {
        bgmHighPassFilter.enabled = isPlaying;
    }
    public void ClickBtn()
    {
        SoundManager.instance.PlayEffect(SoundManager.Effect.Click);
    }
}
