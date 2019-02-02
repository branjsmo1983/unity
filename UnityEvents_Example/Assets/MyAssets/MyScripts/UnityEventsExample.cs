using UnityEngine;
using UnityEngine.Events;

public class UnityEventsExample : MonoBehaviour {

    //Eventi unity tradizionale
    /*[SerializeField]
    private UnityEvent PowerUpTaked;

    void OnTriggerEnter (Collider other) {
        Debug.Log ("OnTriggerEnterChiamato");
        if (other.tag == "PowerUp") {
            other.gameObject.SetActive (false);
            PowerUpTaked.Invoke ();
        }
    }*/
    

    //Event Manager
    void OnTriggerEnter (Collider other) {
        if (other.tag.Equals ("PowerUp")) {
            other.gameObject.SetActive (false);
            MyEventManager.instance.CastEvent (MyIndexEvent.powerUpEvent , new MyEventArgs { sender = gameObject, myFloat = other.gameObject.GetComponent<MyPowerUp>().MyIncrementValue });
        }
    }

}
