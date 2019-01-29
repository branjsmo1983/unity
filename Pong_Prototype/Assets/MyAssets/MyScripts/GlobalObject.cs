using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalObject : MonoBehaviour
{
    public static GlobalObject instance;
    
    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (this);
        } else {
            Destroy (this);
        }
    }

    public void LoadScene (int indexScene) {
        SceneManager.LoadScene (indexScene);
    }

}


public enum SceneByIndex {
    MainMenu = 0,
    GameScene = 1,
}
