using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    
    void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Player")) {
            MyEventManager.instance.CastEvent (MyIndexEvent.collectableTaked , new MyEventArgs ());
            gameObject.SetActive (false);
        }
    }

}
