using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class GlobalObject : MonoBehaviour
{
    public const int mainMenuIndex = 0;

    public static GlobalObject instance;

    [SerializeField]
    private GameObject loadingSchermate;
    [SerializeField]
    private int[] indexAudioClipLevel;

    private WaitForSeconds waitingTimeForLoading = new WaitForSeconds (4f);

    private int _modeSelected;
    public int ModeSelected
    {
        get { return _modeSelected; }
        set { _modeSelected = value; }
    }
    
    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (this);
        } else {
            Destroy (this);
        }
    }

    void Start () {
        MyEventManager.instance.AddListener (MyIndexEvent.lockMouse , OnLockMouse);
        MyEventManager.instance.AddListener (MyIndexEvent.unLockMouse , OnUnlockMouse);
    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.lockMouse , OnLockMouse);
            MyEventManager.instance.RemoveListener (MyIndexEvent.unLockMouse , OnUnlockMouse);
        }
    }

    public void OnLockMouse (MyEventArgs e) {
        SetMouse (false);
    }

    public void OnUnlockMouse (MyEventArgs e) {
        SetMouse (true);
    }

    private void SetMouse (bool visibility) {
        Cursor.visible = visibility;
        if (visibility) {
            Cursor.lockState = CursorLockMode.Confined;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LoadScene (int indexScene , bool async) {
        if (async) {
            StartCoroutine (LoadYourAsyncScene (indexScene));
        } else {
            SceneManager.LoadScene (indexScene);
            RiseAudioEvent (indexScene);
        }
    }

    public int IndexCurrentScene () {
        return SceneManager.GetActiveScene ().buildIndex;
    }

    private IEnumerator LoadYourAsyncScene (int indexScene) {
        loadingSchermate.SetActive (true);
        yield return waitingTimeForLoading; //falsa attesa
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (indexScene);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        loadingSchermate.SetActive (false);
        RiseAudioEvent (indexScene);
    }

    private void RiseAudioEvent (int indexScene) {
        MyEventManager.instance.CastEvent (MyIndexEvent.changeClip , new MyEventArgs (this.gameObject , indexAudioClipLevel[indexScene]));
    }




}
