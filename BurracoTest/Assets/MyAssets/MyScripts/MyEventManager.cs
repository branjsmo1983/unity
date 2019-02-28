using System;
using System.Collections.Generic;
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
	public string playerStart;
	public string player1;
	public string player2;
	public Vector3 lastCardPosition;
	public List<Card> deck;
	public Card cardSelected;
	public Card cardFished;
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

	public MyEventArgs(GameObject sender, Card cardSelected)
	{
		this.sender = sender;
		this.cardSelected = cardSelected;
	}

	public MyEventArgs(GameObject sender, string playerStart)
	{
		this.sender = sender;
		this.playerStart = playerStart;
	}

	public MyEventArgs(GameObject sender, string player1, string player2)
	{
		this.player1 = player1;
		this.player2 = player2;
	}

	public MyEventArgs(GameObject sender, Vector3 lastCardPosition, List<Card> deck, Card cardFished)
	{
		this.lastCardPosition = lastCardPosition;
		this.deck = deck;
		this.cardFished = cardFished;
	}


	//devo ancora creare il LevelData 
	//public MyEventArgs(GameObject sender, LevelData myLevelData)
	//{
	//	this.sender = sender;
	//	this.myLevelData = myLevelData;
	//}
}


//Enumerazione degli eventi
public enum MyIndexEvent
{
	cardSelect = 0,
	cardsHang = 1,
	deckDraw = 2,
	scrapsCollect = 3,
	cockpitTake = 4,
	burracoMake = 5,
	gameEnd = 6,
	gameStart = 7,
	cardsAddToCanasta = 8,

}
