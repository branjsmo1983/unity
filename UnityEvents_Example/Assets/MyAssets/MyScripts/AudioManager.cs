using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioSource myAudioSource;

    [SerializeField]
    private AudioClip powerUpClip;

    void Start () {
        MyEventManager.instance.AddListener (MyIndexEvent.powerUpEvent , OnPowerUpTaked);
    }

    public void OnPowerUpTaked (MyEventArgs e) {
        myAudioSource.PlayOneShot (powerUpClip);
    }

}
