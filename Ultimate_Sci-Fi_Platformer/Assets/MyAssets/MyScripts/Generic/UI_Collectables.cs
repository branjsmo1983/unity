using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Collectables : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI text;

    private int totalCollectable;
    private int collectableTaked;

    // Start is called before the first frame update
    void Start()
    {
        totalCollectable = GameObject.FindGameObjectsWithTag ("Collectable").Length;
        collectableTaked = 0;
        UpdateCounter ();
        MyEventManager.instance.AddListener (MyIndexEvent.collectableTaked , OnCollectableTaked);

    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.collectableTaked , OnCollectableTaked);
        }
    }

    public void OnCollectableTaked (MyEventArgs e) {
        collectableTaked++;
        UpdateCounter ();
    }


    private void UpdateCounter () {
        text.text = collectableTaked.ToString () + "/" + totalCollectable.ToString ();
    }
}
