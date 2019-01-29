using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource myAudioSource;

    public static AudioSourceManager instance;

    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (this);
        } else {
            Destroy (this);
        }
    }

    public void PlayOneShot (AudioClip clipToPlay) {
        myAudioSource.PlayOneShot (clipToPlay);
    }

}
