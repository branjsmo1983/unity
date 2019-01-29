using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformOnlyOver : FallingPlatform
{

    void OnTriggerExit (Collider other) {
        if (other.tag.Equals("Player")) {
            canCountDown = false;
        }
    }

}
