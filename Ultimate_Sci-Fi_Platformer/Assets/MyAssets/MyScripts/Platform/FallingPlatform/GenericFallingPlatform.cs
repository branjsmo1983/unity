using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericFallingPlatform : MonoBehaviour
{

    [SerializeField]
    protected Rigidbody myRigidbody;

    protected float timeToVanish;

    protected Vector3 startPosition;

    protected float currentTimeToVanish;
    protected bool canCountDown;

    void Awake () {
        startPosition = transform.position;
        MyEventManager.instance.AddListener (MyIndexEvent.initializeScene , OnInitializeScene);
    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.initializeScene , OnInitializeScene);
        }
    }

    void Update () {
        if (canCountDown) {
            currentTimeToVanish -= Time.deltaTime;
        }
        if (currentTimeToVanish < 0) {
            Fall ();
        }
    }

    public void OnInitializeScene (MyEventArgs e) {
        InitializeMe (e.myLevelData.TimeToVanish_FallingPlatform);
    }

    public virtual void InitializeMe (float timeToVanish) {
        this.timeToVanish = timeToVanish;
        ResetMe ();
    }

    public virtual void ResetMe () {
        myRigidbody.isKinematic = true;
        transform.position = startPosition;
        currentTimeToVanish = timeToVanish;
    }

    protected virtual void Fall () {
        myRigidbody.isKinematic = false;
    }


}
