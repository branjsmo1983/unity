using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    [SerializeField]
    private Transform startTransform;
    [SerializeField]
    private Transform endTransform;


    [SerializeField]
    private bool linearMovement;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float timeToReachEndPoint;

    private float startTime;
    private bool canMove;

    //Variabili di supporto
    private Vector3 a, b;

    /*    void Start () {
            ResetMe ();
            StartCoroutine (WaitForStart ());
        }

        private IEnumerator WaitForStart () {
            yield return new WaitForSeconds (4f);
            StartMe ();
        }*/

    void Update () {
        if (canMove) {
            if (linearMovement) {
                LinearMovement ();
            } else {
                NonLinearMovement ();
            }
            if (transform.position.Equals(b)) {
                SwapStartEndPosition ();
            }
        } else {
            if (Input.GetMouseButtonDown (0)) {
                ResetMe ();
                StartMe ();
            }
        }
    }

    private void ResetMe () {
        transform.position = startTransform.position;
        canMove = false;
    }

    private void StartMe () {
        canMove = true;
        if (linearMovement) {
            startTime = Time.time;
            speed = 1 / timeToReachEndPoint;
        }
        a = startTransform.position;
        b = endTransform.position;
    }

    private void SwapStartEndPosition () {
        Vector3 supportPosition;
        supportPosition = a;
        a = b;
        b = supportPosition;
        if (linearMovement) {
            startTime = Time.time;
        }
    }

    //Movimento non lineare. La posizione iniziale del lerp cambia ogni frame (parte veloce e arriva piano).
    private void NonLinearMovement () {
        transform.position = Vector3.Lerp (transform.position , b , Time.deltaTime * speed);
        if ((transform.position - b).sqrMagnitude < 0.0001f) {
            transform.position = b;
        }
    }

    //Movimento lineare. L'oggetto parte dalla posizione iniziale e arriva nella posizione finale in un tempo fisso.
    private void LinearMovement () {
        transform.position = Vector3.Lerp (a , b , (Time.time - startTime) * speed);
    }
}
