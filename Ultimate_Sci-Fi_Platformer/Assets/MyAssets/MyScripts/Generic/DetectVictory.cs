using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectVictory : MonoBehaviour
{

    void OnTriggerEnter (Collider other) {
        if (other.tag.Equals ("Player")) {
            MyEventManager.instance.CastEvent (MyIndexEvent.victory , new MyEventArgs ());
        }
    }

}
