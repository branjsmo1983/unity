using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{

    public const int menuIndex = 0;

    [SerializeField]
    private GameObject[] fathers;

    private int currentIndexScreen;

    private int levelIndex;

    void Start () {
        MyEventManager.instance.CastEvent (MyIndexEvent.unLockMouse , new MyEventArgs ());
        currentIndexScreen = 0;
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            Application.Quit ();
        }
    }

    public void StartButton () {
        GoToNextScreen ();
    }

    public void LevelButton (int levelIndex) {
        this.levelIndex = levelIndex;
        GoToNextScreen ();
    }

    public void ModeButton (int modeSelected) {
        GlobalObject.instance.ModeSelected = modeSelected;
        GoToNextScreen ();
    }

    private void GoToNextScreen () {
        fathers[currentIndexScreen].SetActive (false);
        currentIndexScreen++;
        if (currentIndexScreen < fathers.Length) {
            fathers[currentIndexScreen].SetActive (true);
        } else {
            GlobalObject.instance.LoadScene (levelIndex , true);
        }
    }

}
