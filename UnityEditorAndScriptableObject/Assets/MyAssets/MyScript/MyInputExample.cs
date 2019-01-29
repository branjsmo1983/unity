using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputExample : MonoBehaviour
{
    


    // Update is called once per frame
    void Update()
    {
        //Debug.Log (Input.GetAxis ("Horizontal"));
        //Debug.Log (Input.GetAxis ("Vertical"));
        if (Input.GetButtonDown ("Jump")) {
            Debug.Log ("Rilevato down di Jump");
        } else if (Input.GetButton ("Jump")) { 
            Debug.Log ("Jump in over");
        } else if (Input.GetButtonUp ("Jump")) {
            Debug.Log ("Rilevato up di Jump");
        }
    }
}
