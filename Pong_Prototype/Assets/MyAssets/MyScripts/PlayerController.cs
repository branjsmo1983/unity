using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //public KeyCode moveUp;
    //public KeyCode moveDown;

    public string axisName;

    public float speed = 10f;

    [SerializeField]
    private float boundY = 2.25f;

    private Rigidbody2D rb2d;


    void Start () {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    /*UPDATE VECCHIA CON KEYCODE
    // Update is called once per frame
    void Update () {
        Vector2 velocity = rb2d.velocity;
        if (Input.GetKey (moveUp)) {
            velocity.y = speed;
        } else if (Input.GetKey (moveDown)) {
            velocity.y = -speed;
        } else if (!Input.anyKey) {
            velocity.y = 0;
        }
        rb2d.velocity = velocity;
        Vector2 position = transform.position;
        if (position.y > boundY) {
            position.y = boundY;
        } else if (position.y < - boundY) {
            position.y = -boundY;
        }
        transform.position = position;

    }*/

    void Update () {
        Vector2 velocity = rb2d.velocity;
        velocity.y = Input.GetAxis (axisName) * speed;
        rb2d.velocity = velocity;
        Vector2 position = transform.position;
        if (position.y > boundY) {
            position.y = boundY;
        } else if (position.y < -boundY) {
            position.y = -boundY;
        }
        transform.position = position;
    }
}
