using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource auSource;
    private AudioSource AuSource
    {
        get
        {
            if (auSource == null)
            {
                auSource = GetComponent<AudioSource>();
            }
            return auSource;
        }
    }
    private QueuePlayerGroup queuePg;
    private QueuePlayerGroup QueuePg
    {
        get
        {
            if (queuePg == null)
            {
                queuePg = new QueuePlayerGroup(this);
            }
            return queuePg;
        }
    }

    private Action callBack;

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool isPlaying
    {
        get
        {
            return AuSource.isPlaying;
        }
    }

    public AudioClip currentAudioClip
    {
        get
        {
            return AuSource.clip;
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
                callBack = null;
            }
        }

        QueuePg.TryPlay();
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="audioClip">音频</param>
    /// <param name="interrupt">是否打断正在播的音频，如果正在播且为false则不作操作</param>
    public void Play(AudioClip audioClip, bool interrupt = true, Action callBack = null)
    {
        if (AuSource.isPlaying && interrupt == false)
        {
            return;
        }
        if (callBack != null)
        {
            this.callBack = callBack;
        }
        if(QueuePg.isPlaying)
        {
            QueuePg.Interrupt();
        }
        remainingTime = audioClip.length;
        AuSource.clip = audioClip;
        AuSource.Play();
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
        if (AuSource.isPlaying && interrupt == false)
        {
            Debug.Log("播放中");
            return;
        }
        AudioClipInfo audioClipInfo = new AudioClipInfo(audioClip, callBack);
        QueuePg.AddAudioClipInfo(audioClipInfo, weight);
    }


    public void Stop()
    {
        remainingTime = 0;
        AuSource.Stop();
        if (QueuePg.isPlaying)
        {
            QueuePg.Interrupt();
        }
    }
}


