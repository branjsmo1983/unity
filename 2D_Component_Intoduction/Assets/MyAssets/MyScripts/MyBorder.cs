using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBorder : MonoBehaviour
{

    void OnCollisionEnter2D (Collision2D collision) {
        
        if (collision.gameObject.tag.Equals ("Player")) {
           //Debug.Log ("Oggetto: " + name + " a avuto una collisione con la palla");
        }
    }


    void OnCollisionStay2D (Collision2D collision) {
        if (collision.gameObject.tag.Equals ("Player")) {
            //Debug.Log ("Oggetto: " + name + " continua ad avere una collisione con la palla");
        }
    }

    void OnCollisionExit2D (Collision2D collision) {
        if (collision.gameObject.tag.Equals ("Player")) {
            //Debug.Log ("Oggetto: " + name + " non ha più una collisione con la palla");
        }
    }

}
