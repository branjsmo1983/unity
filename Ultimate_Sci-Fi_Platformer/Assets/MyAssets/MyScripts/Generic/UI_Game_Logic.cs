using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Game_Logic : MonoBehaviour
{

    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject endScreen;

    [SerializeField]
    private GameObject victoryWrite;
    [SerializeField]
    private GameObject looseWrite;

    public void ActivatePause (bool value) {
        pauseScreen.SetActive (value);
        endScreen.SetActive (false);
        gameObject.SetActive (value);
        AudioListener.pause = value;
    }

    public void ActivateEndScreen (bool victory) {
        pauseScreen.SetActive (false);
        endScreen.SetActive (true);
        victoryWrite.SetActive (victory);
        looseWrite.SetActive (!victory);
        gameObject.SetActive (true);
    }

    public void OnEnable () {
        MyEventManager.instance.CastEvent (MyIndexEvent.unLockMouse , new MyEventArgs ());
    }

    public void OnDisable () {
        MyEventManager.instance.CastEvent (MyIndexEvent.lockMouse , new MyEventArgs ());
    }



}
