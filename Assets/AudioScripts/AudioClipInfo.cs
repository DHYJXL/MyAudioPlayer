using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipInfo
{
    public AudioClip audioClip;
    public Action callBack;
    public AudioClipInfo(AudioClip audioClip, Action callBack = null)
    {
        this.audioClip = audioClip;
        this.callBack = callBack;
    }

}
