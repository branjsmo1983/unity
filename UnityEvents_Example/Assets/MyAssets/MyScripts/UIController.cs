using UnityEngine.UI;
using UnityEngine;


public class UIController : MonoBehaviour {

    [SerializeField]
    private Text powerUpText;

    private int powerUpCount = 0;

    void Start () {
        MyEventManager.instance.AddListener (MyIndexEvent.powerUpEvent , OnPowerUpTaked);
    }

    public void OnPowerUpTaked (MyEventArgs e) {
        powerUpCount++;
        powerUpText.text = powerUpCount.ToString ();
    }

}
