using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    [SerializeField]
    private Vector3 positionOffsetOnPaddle;
    [SerializeField]
    private float startSpeed;

    [SerializeField]
    private AudioClip sfxWall, sfxPaddle, sfxScore;

    public GameController gameController;

    private Rigidbody2D rb2d;

    private Transform currentTransformToFollow;

    private bool readyToStart;

    void Awake () {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    public void InitializeBall () {
        readyToStart = false;
        rb2d.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        float randomNumber = Random.Range (0 , 2);
        StartCoroutine (PerformStartForce (randomNumber < 1 ? new Vector2 (20 , -15) : new Vector2 (-20 , -15)));
    }

    public void InitializeBall (Transform paddleTransform) {
        rb2d.velocity = Vector2.zero;
        currentTransformToFollow = paddleTransform;
        readyToStart = true;
    }

    void Update () {
        if (readyToStart) {
            if (Input.GetKeyDown (KeyCode.Space)) {
                Shoot ();
            } else {
                transform.position =  currentTransformToFollow.position + (transform.position.x < 0 ?positionOffsetOnPaddle : -positionOffsetOnPaddle);
            }
        }
    }

    private void Shoot () {
        Debug.Log (transform.position - Vector3.zero);
        rb2d.velocity = (Vector3.zero - transform.position) * startSpeed;
        readyToStart = false;
    }

    private IEnumerator PerformStartForce (Vector2 force) {
        yield return new WaitForSeconds (3f);
        rb2d.AddForce (force);
    }

    void OnCollisionEnter2D (Collision2D coll) {
        if (coll.collider.CompareTag ("Player")) {
            Vector2 velocity;
            velocity.x = rb2d.velocity.x;
            velocity.y = (rb2d.velocity.y / 2.0f) + (coll.collider.attachedRigidbody.velocity.y / 3.0f);
            AudioSourceManager.instance.PlayOneShot (sfxPaddle);
            rb2d.velocity = velocity;
        } else if (coll.collider.CompareTag ("Y_Wall")) {
            AudioSourceManager.instance.PlayOneShot (sfxWall);
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag.Equals ("X_Wall")) {
            gameController.PlayerScore (transform.position.x > 0);
            AudioSourceManager.instance.PlayOneShot (sfxScore);
        }
    }

}
