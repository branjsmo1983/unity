using System;
using UnityEngine;
using UnityEngine.Events;

public class MyEventManager : MonoBehaviour
{

    public static MyEventManager instance;

    public MyEvent[] myEvents;


    //Istanza singleton
    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (this);
            InitializeEvents ();
        } else {
            Destroy (this);
        }
    }

    //Metodo per inizializzzare gli eventi
    private void InitializeEvents () {
        myEvents = new MyEvent [Enum.GetValues (typeof (MyIndexEvent)).Length];
        for (int i = 0; i < myEvents.Length; i++) {
            myEvents[i] = new MyEvent();
        }
    }


    //Metodo per lanciare gli eventi.
    public void CastEvent (MyIndexEvent eventToCast, MyEventArgs e) {
        myEvents[(int) eventToCast].Invoke (e);
    }

    //Metodo per aggiungere metodi a eventi
    public void AddListener (MyIndexEvent eventListener, UnityAction<MyEventArgs> call) {
        myEvents[(int) eventListener].AddListener (call);
    }

    //Metodo per rimuovere metodi a eventi
    public void RemoveListener (MyIndexEvent eventListener , UnityAction<MyEventArgs> call) {
        myEvents[(int) eventListener].RemoveListener (call);
    }

}


//La classe evento personalizzata
[System.Serializable]
public class MyEvent : UnityEvent<MyEventArgs> {

}

//I parametri dell'evento personalizzati
public class MyEventArgs {

    public GameObject sender;
    public float myFloat;

    public MyEventArgs () {
        sender = null;
    }

    public MyEventArgs (GameObject sender) {
        this.sender = sender;
    }

    public MyEventArgs (GameObject sender, float myFloat) {
        this.sender = sender;
        this.myFloat = myFloat;
    }
}


//Enumerazione degli eventi
public enum MyIndexEvent {
    powerUpEvent = 0,
    jumpCasted = 1,
    playerHitted = 2,
}