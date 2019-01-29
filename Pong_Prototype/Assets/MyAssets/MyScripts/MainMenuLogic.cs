using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    
    void Update () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            GlobalObject.instance.LoadScene ((int) SceneByIndex.GameScene);
        }
    }

}
