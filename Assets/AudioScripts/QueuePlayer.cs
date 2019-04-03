using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class QueuePlayer
{
    private AudioPlayer ap;
    private List<AudioClipInfo> audioQueue;
    private QueuePlayerGroup holderGroup;

    public int weight
    {
        get;
        private set;
    }

    public QueuePlayer(AudioPlayer audioPlayer, int weight)
    {
        this.ap = audioPlayer;
        audioQueue = new List<AudioClipInfo>();
        this.weight = weight;
    }


    public bool isPlaying
    {
        get;
        private set;
    }


    public void AddClip(AudioClipInfo audioClip, int insertIndex = -1)
    {
        if (insertIndex >= 0)
        {
            audioQueue.Insert(insertIndex, audioClip);
        }
        else
        {
            audioQueue.Add(audioClip);
        }
    }
    public void Interrupt()
    {
        if (isPlaying)
        {
            Debug.Log(audioQueue[0].audioClip.name + "被打断了");
            isPlaying = false;
            ap.Stop();
            audioQueue.RemoveAt(0);
           
        }
    }
    public void TryPlayQueue(Action callback = null)
    {
        if (audioQueue == null || audioQueue.Count == 0)
        {
            if (callback != null)
            {
                holderGroup.ClearPlayingQueue();
                callback();
            }
            return;
        }
        if (ap.isPlaying == false && isPlaying == false)
        {
            ap.Play(audioQueue[0].audioClip, true, audioQueue[0].callBack);
            holderGroup.SetPlayingQueue(this);
            isPlaying = true;
        }
        if (isPlaying && ap.remainingTime <= 0)
        {
            isPlaying = false;
            audioQueue.RemoveAt(0);
        }
    }


    public void SetHolderGroup(QueuePlayerGroup holderGroup)
    {
        this.holderGroup = holderGroup;
        this.ap = holderGroup.audioPlayer;
    }
}


