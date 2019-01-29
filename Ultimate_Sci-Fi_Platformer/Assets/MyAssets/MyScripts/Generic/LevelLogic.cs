using System.Collections;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{

    [SerializeField]
    private LevelData[] myModeData;
    [SerializeField]
    private UI_Game_Logic myUI;

    private bool inGame;
    private int numberOfCollectable;
    private int collectableTaked;

    void Awake () {
        MyEventManager.instance.AddListener (MyIndexEvent.defeat , OnDefeat);
        MyEventManager.instance.AddListener (MyIndexEvent.victory , OnVictory);
        MyEventManager.instance.AddListener (MyIndexEvent.collectableTaked , OnCollectableTaked);
    }

    void Start () {
        MyEventManager.instance.CastEvent (MyIndexEvent.lockMouse , new MyEventArgs ());
        InitializeParameter ();
        numberOfCollectable = GameObject.FindGameObjectsWithTag ("Collectable").Length;
        collectableTaked = 0;
    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.defeat , OnDefeat);
            MyEventManager.instance.RemoveListener (MyIndexEvent.victory , OnVictory);
            MyEventManager.instance.RemoveListener (MyIndexEvent.collectableTaked , OnCollectableTaked);
        }
    }

    public void OnDefeat (MyEventArgs e) {
        StartCoroutine (ActivateEndScreen (false,0));
    }

    public void OnVictory (MyEventArgs e) {
        if (collectableTaked < numberOfCollectable) {
            return;
        }
        //Tutti i feedback che vogliamo
        StartCoroutine (ActivateEndScreen (true, 1));
    }

    public void OnCollectableTaked (MyEventArgs e) {
        collectableTaked++;
    }

    private IEnumerator ActivateEndScreen (bool value, float waitingTime) {
        inGame = false;
        yield return new WaitForSeconds (waitingTime);
        myUI.ActivateEndScreen (value);
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            PauseButton ();
        }
    }

    public void PauseButton () { 
        if (!inGame) {
            return;
        }
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
            myUI.ActivatePause (false);
        } else {
            Time.timeScale = 0;
            myUI.ActivatePause (true);
        }
    }

    public void RetryButton () {
        GlobalObject.instance.LoadScene (GlobalObject.instance.IndexCurrentScene (), false);
    }

    public void MainMenuButton () {
        GlobalObject.instance.LoadScene (GlobalObject.mainMenuIndex , true);
    }

    private void InitializeParameter () {
        LevelData currentModeData = myModeData[GlobalObject.instance.ModeSelected];
        MyEventManager.instance.CastEvent (MyIndexEvent.initializeScene , new MyEventArgs (gameObject , currentModeData));
        inGame = true;
    }



}
