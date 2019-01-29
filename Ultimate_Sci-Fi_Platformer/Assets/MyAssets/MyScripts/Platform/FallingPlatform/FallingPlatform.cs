using UnityEngine;

public class FallingPlatform : GenericFallingPlatform
{
   
    void OnTriggerEnter (Collider other) {
        if (other.tag.Equals ("Player")) {
            canCountDown = true;
        }
    }

}
