using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDefeat : MonoBehaviour
{
    
    void OnTriggerEnter (Collider other) {
        if (other.tag.Equals ("Player")) {
            MyEventManager.instance.CastEvent (MyIndexEvent.defeat , new MyEventArgs ());
        }
    }

}
