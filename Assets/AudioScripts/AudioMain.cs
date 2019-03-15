using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMain : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioPlayer ap;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("start");
            ap.QueuePlay(audioClips[0], 3, true, CallBack);
            ap.QueuePlay(audioClips[1], 5, true, CallBack);
            ap.QueuePlay(audioClips[2], 2, true, CallBack);
            ap.QueuePlay(audioClips[3], 0, true, CallBack);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ap.Play(audioClips[4], true, CallBack);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ap.QueuePlay(audioClips[5], 3, true, CallBack);
            ap.QueuePlay(audioClips[6], 0, true, CallBack);
        }
	}
    public void CallBack()
    {
        Debug.Log("播放完成" + ap.currentAudioClip.name);
    }
}
