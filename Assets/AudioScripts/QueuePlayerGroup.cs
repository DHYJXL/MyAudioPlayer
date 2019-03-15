using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class QueuePlayerGroup
{
    private List<QueuePlayer> queuePlayers;
    private List<int> weights;
    public AudioPlayer audioPlayer
    {
        get;
        private set;
    }
    private QueuePlayer playingQueue;


    public QueuePlayerGroup(AudioPlayer audioPlayer)
    {
        queuePlayers = new List<QueuePlayer>();
        weights = new List<int>();
        this.audioPlayer = audioPlayer;
    }
    /// <summary>
    /// 队列是否在播放
    /// </summary>
    public bool isPlaying
    {
        get;
        private set;
    }


    /// <summary>
    /// 添加新的队列
    /// </summary>
    /// <param name="queuePlayer"></param>
    public void AddQueue(QueuePlayer queuePlayer)//可优化
    {
        if (weights.Contains(queuePlayer.weight))
        {
            Debug.Log("添加队列失败，已有该级别队列：" + queuePlayer.weight);
            return;
        }
        queuePlayers.Add(queuePlayer);
        weights.Add(queuePlayer.weight);
        queuePlayer.SetHolderGroup(this);
        queuePlayers.Sort((x, y) => x.weight.CompareTo(y.weight));//由小到大排序
        Debug.Log("添加成功：队列数量为" + queuePlayers.Count);
    }
    /// <summary>
    /// 移除队列
    /// </summary>
    /// <param name="queuePlayer"></param>
    public void RemoveQueue(QueuePlayer queuePlayer)
    {
        if (weights.Contains(queuePlayer.weight))
        {
            weights.Remove(queuePlayer.weight);
            queuePlayers.Remove(queuePlayer);
        }
        else
        {
            Debug.Log("移除队列失败，不存在该级别队列：" + queuePlayer.weight);
        }
    }

    /// <summary>
    /// 根据级别查找队列
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    public QueuePlayer FindQueuePlayer(int weight)
    {
        if (!weights.Contains(weight))
        {
            Debug.Log("查找失败，不存在该级别队列：" + weight);
            return null;
        }
        foreach (QueuePlayer queuePlayer in queuePlayers)
        {
            if (queuePlayer.weight == weight)
            {
                return queuePlayer;
            }
        }
        Debug.Log("查找失败，见鬼了");
        return null;
    }

    /// <summary>
    /// 添加音频
    /// </summary>
    /// <param name="audioClip">音频</param>
    /// <param name="weight">设置的级别</param>
    public void AddAudioClip(AudioClip audioClip, int weight)
    {
        Debug.Log("添加");
        if (weights.Contains(weight))
        {
            QueuePlayer queuePlayer = FindQueuePlayer(weight);
            queuePlayer.AddClip(audioClip);
        }
        else
        {
            QueuePlayer queuePlayer = new QueuePlayer(audioPlayer, weight);//可优化
            queuePlayer.AddClip(audioClip);
            AddQueue(queuePlayer);
        }
    }

    /// <summary>
    /// 尝试播放最高级别的音频队列
    /// </summary>
    public void TryPlay()
    {
        if (queuePlayers.Count == 0)
        {
            Debug.Log("无队列:" + queuePlayers.Count);
            isPlaying = false;
            return;
        }
        if (audioPlayer.isPlaying)
        {
            Debug.Log("播放中");
            return;
        }
        isPlaying = true;
        Debug.Log("开始播放");
        queuePlayers[0].TryPlayQueue(() => RemoveQueue(queuePlayers[0]));

    }

    /// <summary>
    /// 打断当前播放音频
    /// </summary>
    public void Interrupt()
    {
        //for (int i = 0; i < weights.Count; i++)
        //{
        //    if (queuePlayers[i].isPlaying == false)
        //    {
        //        continue;
        //    }
        //    queuePlayers[i].Interrupt();
        //}
        if (playingQueue != null)
        {
            playingQueue.Interrupt();
            isPlaying = false;
        }
    }

    public void SetPlayingQueue(QueuePlayer queuePlayer)
    {
        playingQueue = queuePlayer;
    }
    public void ClearPlayingQueue()
    {
        playingQueue = null;
    }
}
