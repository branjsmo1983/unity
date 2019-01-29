using UnityEngine;

public class AudioSourceManager : MonoBehaviour {

    public static AudioSourceManager instance;

    [SerializeField]
    private int startClip = 0;
    [SerializeField]
    private float minVolume = 0;
    [SerializeField]
    private float maxVolume = 1;
    [SerializeField]
    private float fadeInOutDuration = 5f;

    [SerializeField]
    private AudioClip[] backgroundMusics;
    [SerializeField]
    private AudioSource primaryAudioSource;
    [SerializeField]
    private AudioSource secondaryAudioSource;
    [SerializeField]
    private AudioSource sfxAudioSource;

    [SerializeField]
    private AudioClip collectableTaked;
    [SerializeField]
    private AudioClip victoryJingle;
    [SerializeField]
    private AudioClip defeatJingle;

    private float fadeInOutSpeed;
    private float startVolumeSecondaryAudioSource;
    private int currentClipIndex = -1;
    private float startTime = 0;
    private bool inTransition;

    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (this);
            fadeInOutSpeed = 1 / fadeInOutDuration;
            LoadClip (startClip);
        } else {
            Destroy (this);
        }
    }

    void Start () {
        MyEventManager.instance.AddListener (MyIndexEvent.changeClip , OnChangeClip);
        MyEventManager.instance.AddListener (MyIndexEvent.collectableTaked , OnCollectableTaked);
        MyEventManager.instance.AddListener (MyIndexEvent.victory , OnVictory);
        MyEventManager.instance.AddListener (MyIndexEvent.defeat , OnDefeat);
    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.changeClip , OnChangeClip);
            MyEventManager.instance.RemoveListener (MyIndexEvent.collectableTaked , OnCollectableTaked);
            MyEventManager.instance.RemoveListener (MyIndexEvent.victory , OnVictory);
            MyEventManager.instance.RemoveListener (MyIndexEvent.defeat , OnDefeat);
        }
    }

    void Update () {
        if (inTransition) {
            primaryAudioSource.volume = Mathf.Lerp (minVolume , maxVolume , (Time.time - startTime) * fadeInOutSpeed);
            secondaryAudioSource.volume = Mathf.Lerp (startVolumeSecondaryAudioSource , minVolume , (Time.time - startTime) * fadeInOutSpeed);
            if ((Time.time - startTime) >= fadeInOutDuration) {
                primaryAudioSource.volume = maxVolume;
                secondaryAudioSource.volume = minVolume;
                inTransition = false;
            }
        }
    }

    public void OnCollectableTaked (MyEventArgs e) {
        sfxAudioSource.PlayOneShot (collectableTaked);
    }

    public void OnVictory (MyEventArgs e) {
        StopClip ();
        sfxAudioSource.PlayOneShot (victoryJingle);
    }

    public void OnDefeat (MyEventArgs e) {
        StopClip ();
        sfxAudioSource.PlayOneShot (defeatJingle);
    }

    public void OnChangeClip (MyEventArgs e) {
        LoadClip (e.myInt);
    }

    private void LoadClip (int index) {
        if (index == currentClipIndex && primaryAudioSource.isPlaying) {
            return;
        }
        currentClipIndex = index;
        if (primaryAudioSource.isPlaying) {
            secondaryAudioSource.clip = primaryAudioSource.clip;
            secondaryAudioSource.volume = primaryAudioSource.volume;
            startVolumeSecondaryAudioSource = secondaryAudioSource.volume;
            secondaryAudioSource.timeSamples = primaryAudioSource.timeSamples;
            secondaryAudioSource.Play ();
        }
        primaryAudioSource.Stop ();
        primaryAudioSource.volume = 0;
        primaryAudioSource.clip = backgroundMusics[currentClipIndex];
        primaryAudioSource.Play ();
        inTransition = true;
        startTime = Time.time;
    }

    private void StopClip () {
        primaryAudioSource.Stop ();
    }

}
