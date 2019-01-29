using System.Collections;
using UnityEngine;

public class FirstMovement : MonoBehaviour
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

/* Per debug    void Start () {
        ResetMe ();
        StartCoroutine (WaitForStart ());
    }

    private IEnumerator WaitForStart () {
        yield return new WaitForSeconds (4f);
        StartMe ();
    } */

    void Update () {
        if (canMove) {
            if (linearMovement) {
                LinearMovement ();
            } else {
                NonLinearMovement ();
            }
            canMove = !transform.position.Equals (endTransform.position);
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
    }

    //Movimento non lineare. La posizione iniziale del lerp cambia ogni frame (parte veloce e arriva piano).
    private void NonLinearMovement () {
        transform.position = Vector3.Lerp (transform.position , endTransform.position , Time.deltaTime * speed);
        if ((transform.position - endTransform.position).sqrMagnitude < 0.0001f) {
            transform.position = endTransform.position;
        }
    }

    //Movimento lineare. L'oggetto parte dalla posizione iniziale e arriva nella posizione finale in un tempo fisso.
    private void LinearMovement () {
        transform.position = Vector3.Lerp (startTransform.position , endTransform.position , (Time.time - startTime) * speed);
    }
}
