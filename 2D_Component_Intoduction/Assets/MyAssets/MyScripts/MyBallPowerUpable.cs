using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBallPowerUpable : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D myRigidBody;
    [SerializeField]
    private float forceValueX;
    [SerializeField]
    private float forceValueY;
    [SerializeField]
    private AudioSource myAudioSource;
    [SerializeField]
    private AudioClip powerUpAudioClip;
    [SerializeField]
    private AudioClip wallClip;
    
    void Update () {
        if (Input.GetKey (KeyCode.DownArrow)) {
            myRigidBody.AddForce (Vector2.down * forceValueY);
        }
        if (Input.GetKey (KeyCode.UpArrow)) {
            myRigidBody.AddForce (Vector2.up * forceValueY);
        }
        if (Input.GetKey (KeyCode.LeftArrow)) {
            myRigidBody.AddForce (Vector2.left * forceValueX);
        }
        if (Input.GetKey (KeyCode.RightArrow)) {
            myRigidBody.AddForce (Vector2.right * forceValueX);
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag.Equals ("PowerUp")) {
            MyPowerUp otherMyPowerUp = other.GetComponent<MyPowerUp> ();
            ApplyPowerUp (otherMyPowerUp.MyVector , otherMyPowerUp.IsForce);
            myAudioSource.PlayOneShot (powerUpAudioClip);
            other.gameObject.SetActive (false);
        }
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag.Equals ("Border")) {
            myAudioSource.PlayOneShot (wallClip);
        }
    }

    private void ApplyPowerUp (Vector2 vector, bool isForce) {
        if (isForce) {
            myRigidBody.AddForce (vector , ForceMode2D.Impulse);
        } else {
            myRigidBody.velocity = vector;
        }
    }

}
