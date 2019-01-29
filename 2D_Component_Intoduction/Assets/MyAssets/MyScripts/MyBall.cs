using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBall : MonoBehaviour
{

    public Vector2 myForceValue;

    private Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody2D> ();
        myRigidBody.isKinematic = true;
        
        StartCoroutine (StartUsingPhysics ());
    }


    private IEnumerator StartUsingPhysics () {
        yield return new WaitForSeconds (1f);
        myRigidBody.isKinematic = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0)) {
            myRigidBody.AddTorque (0.1f);
        } else if (Input.GetMouseButton (1)) {
            myRigidBody.AddRelativeForce (myForceValue , ForceMode2D.Force);
        } else if (Input.GetKeyDown (KeyCode.A)) {
            myRigidBody.velocity = Vector2.right;
        }
    }
}
