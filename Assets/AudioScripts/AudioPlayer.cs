using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private QueuePlayerGroup queuePlayerGroup;

    private Action callBack;

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool isPlaying
    {
        get
        {
            return audioSource.isPlaying;
        }
    }

    public AudioClip currentAudioClip
    {
        get
        {
            return audioSource.clip;
        }
    }
    /// <summary>
    /// 当前音频剩余时间
    /// </summary>
    public float remainingTime
    {
        get;
        private set;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        queuePlayerGroup = new QueuePlayerGroup(this);
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        if (remainingTime < 0)
        {
            remainingTime = 0;
            if (callBack != null)
            {
                callBack();
            }
        }

        queuePlayerGroup.TryPlay();
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="audioClip">音频</param>
    /// <param name="interrupt">是否打断正在播的音频，如果正在播且为false则不作操作</param>
    public void Play(AudioClip audioClip, bool interrupt = true, Action callBack = null)
    {
        if (audioSource.isPlaying && interrupt == false)
        {
            return;
        }
        if (callBack != null)
        {
            this.callBack = callBack;
        }
        remainingTime = audioClip.length;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// 延迟播放
    /// </summary>
    /// <param name="audioClip">音频</param>
    /// <param name="delay">延迟时间s</param>
    /// <param name="interrupt">是否打断正在播的音频，如果正在播且为false则不作操作/param>
    public void Play(AudioClip audioClip, float delay, bool interrupt = true, Action callBack = null)
    {
        DOVirtual.DelayedCall(delay, () => Play(audioClip, interrupt, callBack));
    }

    /// <summary>
    /// 加入播放队列
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="weight"></param>
    /// <param name="interrupt"></param>
    /// <param name="callBack"></param>
    public void QueuePlay(AudioClip audioClip, int weight, bool interrupt = true, Action callBack = null)
    {
        if (audioSource.isPlaying && interrupt == false)
        {
            Debug.Log("播放中");
            return;
        }
        AudioClipInfo audioClipInfo = new AudioClipInfo(audioClip, callBack);
        queuePlayerGroup.AddAudioClipInfo(audioClipInfo, weight);
    }


    public void Stop()
    {
        remainingTime = 0;
        audioSource.Stop();
    }
}


