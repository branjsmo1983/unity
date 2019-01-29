using UnityEngine;

public class MyBallExcercise : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private bool isKinematic;
    [SerializeField]
    private bool isForce;
    [SerializeField]
    private Vector2 rightDownVector;
    [SerializeField]
    private Vector2 leftUpVector;

    void Start () {
        myRigidbody.isKinematic = isKinematic;
    }

    void Update () {
        if (Input.GetMouseButton (0)) {
            if (isForce) {
                myRigidbody.AddForce (rightDownVector);
            } else {
                myRigidbody.velocity = rightDownVector;
            }
        } else if (Input.GetMouseButton (1)) {
            if (isForce) {
                myRigidbody.AddForce (leftUpVector);
            } else {
                myRigidbody.velocity = leftUpVector;
            }
        }
    }


}
