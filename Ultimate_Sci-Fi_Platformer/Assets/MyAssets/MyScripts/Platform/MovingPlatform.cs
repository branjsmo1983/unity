using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    private Transform[] checkPoints;

    private float speed;
    private float waitingTime;

    private bool isMoving;
    private float currentWaitingTime;

    private float startMovingTime;

    private int lastIndexCheckPoint;
    private int currentIndexCheckPoint;



    void Awake () {
        MyEventManager.instance.AddListener (MyIndexEvent.initializeScene , OnInitializeScene);
    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.initializeScene , OnInitializeScene);
        }
    }

    void OnInitializeScene (MyEventArgs e) {
        InitializeMe (e.myLevelData.TimeToReachCheckPoint_MovingPlatform , e.myLevelData.TimeOfWaiting_MovingPlatform);
    }

    void FixedUpdate () {
        if (isMoving) {
            MovingBehaviour ();
        } else {
            NotMovingBehaviour ();
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            other.gameObject.transform.parent = transform;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.tag.Equals ("Player")) {
            other.gameObject.transform.parent = null;
        }
    }

    public void InitializeMe (float speed , float waitingTime) {
        currentIndexCheckPoint = 0;
        transform.position = checkPoints[currentIndexCheckPoint].transform.position;
        this.speed = 1 / speed;
        this.waitingTime = waitingTime;
        StopMe ();
    }

    private void MovingBehaviour () {
        transform.position = Vector3.Lerp (checkPoints[lastIndexCheckPoint].transform.position , checkPoints[currentIndexCheckPoint].transform.position , (Time.time - startMovingTime) * speed);
        if (transform.position.Equals (checkPoints[currentIndexCheckPoint].transform.position)) {
            StopMe ();
        }
    }

    private void NotMovingBehaviour () {
        currentWaitingTime -= Time.fixedDeltaTime;
        if (currentWaitingTime <= 0) {
            ReachNextCheckPoint ();
        }
    }

    private void ReachNextCheckPoint () {
        lastIndexCheckPoint = currentIndexCheckPoint;
        currentIndexCheckPoint++;
        currentIndexCheckPoint = currentIndexCheckPoint % checkPoints.Length;
        startMovingTime = Time.time;
        isMoving = true;
    }

    private void StopMe () {
        isMoving = false;
        currentWaitingTime = waitingTime;
    }

}
