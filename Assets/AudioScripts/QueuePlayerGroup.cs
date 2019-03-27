using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class QueuePlayerGroup
{
    private List<QueuePlayer> queuePlayers;
    private List<int> weights;

    private List<QueuePlayer> hideQueuePlayers;
    private List<int> hideWeights;

    public AudioPlayer audioPlayer
    {
        get;
        private set;
    }
    private QueuePlayer playingQueue;


    public QueuePlayerGroup(AudioPlayer audioPlayer)
    {
        queuePlayers = new List<QueuePlayer>();
        hideQueuePlayers = new List<QueuePlayer>();
        weights = new List<int>();
        hideWeights = new List<int>();
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
            hideWeights.Add(queuePlayer.weight);
            weights.Remove(queuePlayer.weight);
            hideQueuePlayers.Add(queuePlayer);
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
    private QueuePlayer FindQueuePlayer(int weight)
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
        return null;
    }

    /// <summary>
    /// 根据级别查找隐藏队列
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    private QueuePlayer FindHideQueuePlayer(int weight)
    {
        if (!hideWeights.Contains(weight))
        {
            Debug.Log("查找失败，不存在该级别队列：" + weight);
            return null;
        }
        foreach (QueuePlayer queuePlayer in hideQueuePlayers)
        {
            if (queuePlayer.weight == weight)
            {
                return queuePlayer;
            }
        }
        return null;
    }

    /// <summary>
    /// 添加音频
    /// </summary>
    /// <param name="audioClip">音频</param>
    /// <param name="weight">设置的级别</param>
    public void AddAudioClipInfo(AudioClipInfo audioClipInfo, int weight)
    {
        Debug.Log("添加");
        if (weights.Contains(weight))
        {
            QueuePlayer queuePlayer = FindQueuePlayer(weight);
            queuePlayer.AddClip(audioClipInfo);
        }
        else if (hideWeights.Contains(weight))
        {
            Debug.Log("移除列表中存在该级别");
            QueuePlayer queuePlayer = FindHideQueuePlayer(weight);
            
            queuePlayer.AddClip(audioClipInfo);
            AddQueue(queuePlayer);

            hideQueuePlayers.Remove(queuePlayer);
            hideWeights.Remove(weight);
        }
        else
        {
            QueuePlayer queuePlayer = new QueuePlayer(audioPlayer, weight);//可优化
            queuePlayer.AddClip(audioClipInfo);
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
