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
            ap.QueuePlay(audioClips[0], 3);
            ap.QueuePlay(audioClips[1], 5);
            ap.QueuePlay(audioClips[2], 2);
            ap.QueuePlay(audioClips[3], 0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ap.Play(audioClips[4]);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ap.QueuePlay(audioClips[5], 3);
            ap.QueuePlay(audioClips[6], 0);
        }
	}
}
