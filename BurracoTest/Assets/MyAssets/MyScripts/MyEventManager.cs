using System;
using UnityEngine;
using UnityEngine.Events;

public class MyEventManager : MonoBehaviour
{

	public static MyEventManager instance;

	private MyEvent[] myEvents;


	//Istanza singleton
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
			InitializeEvents();
		}
		else
		{
			Destroy(this);
		}
	}

	//Metodo per inizializzzare gli eventi
	private void InitializeEvents()
	{
		myEvents = new MyEvent[Enum.GetValues(typeof(MyIndexEvent)).Length];
		for (int i = 0; i < myEvents.Length; i++)
		{
			myEvents[i] = new MyEvent();
		}
	}


	//Metodo per lanciare gli eventi.
	public void CastEvent(MyIndexEvent eventToCast, MyEventArgs e)
	{
		myEvents[(int)eventToCast].Invoke(e);
	}

	//Metodo per aggiungere metodi a eventi
	public void AddListener(MyIndexEvent eventListener, UnityAction<MyEventArgs> call)
	{
		myEvents[(int)eventListener].AddListener(call);
	}

	//Metodo per rimuovere metodi a eventi
	public void RemoveListener(MyIndexEvent eventListener, UnityAction<MyEventArgs> call)
	{
		myEvents[(int)eventListener].RemoveListener(call);
	}

}


//La classe evento personalizzata
[System.Serializable]
public class MyEvent : UnityEvent<MyEventArgs>
{

}

//I parametri dell'evento personalizzati
public class MyEventArgs
{

	public GameObject sender;
	public int myInt;
	//public LevelData myLevelData;

	public MyEventArgs()
	{
		sender = null;
	}

	public MyEventArgs(GameObject sender)
	{
		this.sender = sender;
	}

	public MyEventArgs(GameObject sender, int myInt)
	{
		this.sender = sender;
		this.myInt = myInt;
	}

	//public MyEventArgs(GameObject sender, LevelData myLevelData)
	//{
	//	this.sender = sender;
	//	this.myLevelData = myLevelData;
	//}
}


//Enumerazione degli eventi
public enum MyIndexEvent
{
	//changeClip = 0,
	//initializeScene = 1,
	//defeat = 2,
	//victory = 3,
	//lockMouse = 4,
	//unLockMouse = 5,
	//collectableTaked = 6,
}
