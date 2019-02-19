﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Burraco : MonoBehaviour
{
	private const string ME = "mePlayer";
	private const string MYMATE = "myMatePlayer";
	private const string LEFTOPPONENT = "leftOpponentPlayer";
	private const string RIGHTOPPONENT = "rightOpponentPlayer";

	[SerializeField]
	private Deck deck;

	[SerializeField]
	private DeckForStartGame deckForStartGame;

	[SerializeField]
	GameObject cockpitPosition;

	[SerializeField]
	GameObject deckFake;

	[SerializeField]
	GameObject playingDeckPosition;

	[SerializeField]
	GameObject refusePosition;

	[SerializeField]
	internal GameObject[] hands = new GameObject[4];										//per sapere la transform

	[SerializeField]
	internal GameObject[] cards = new GameObject[4];                                        //per sapere la transform

	internal List<Card> firstCockpit = new List<Card>();
	internal List<Card> secondCockpit = new List<Card>();
	internal List<Card> refuseCards = new List<Card>();
	internal List<Card> cardsSelected = new List<Card>();
	private List<CardForStartGame> myCardsForStart = new List<CardForStartGame>();
	internal string playerstart;
	internal bool alreadyGetBurraco = false;
	internal bool isCanGetInput = false;
	internal bool alreadyFished = false;
	internal bool alreadyCollected = false;
	[SerializeField]
	internal Player me;
	[SerializeField]
	internal Player myMate;
	[SerializeField]
	internal Player leftOpponent;
	[SerializeField]
	internal Player rightOpponent;


	void Awake()
	{
		MyEventManager.instance.AddListener(MyIndexEvent.cardSelect, OnCardSelect);
		MyEventManager.instance.AddListener(MyIndexEvent.cardsHang, OnCardsHang);
		MyEventManager.instance.AddListener(MyIndexEvent.deckDraw, OnDeckDraw);
		MyEventManager.instance.AddListener(MyIndexEvent.scrapsCollect, OnScrapsCollect);
		MyEventManager.instance.AddListener(MyIndexEvent.cockpitTake, OnCockpitTake);
		MyEventManager.instance.AddListener(MyIndexEvent.burracoMake, OnBurracoMake);
		MyEventManager.instance.AddListener(MyIndexEvent.gameEnd, OnGameEnd);
		MyEventManager.instance.AddListener(MyIndexEvent.gameStart, OnGameStart);
	}



	void OnDestroy()
	{
		if (MyEventManager.instance != null)
		{
			MyEventManager.instance.RemoveListener(MyIndexEvent.cardSelect, OnCardSelect);
			MyEventManager.instance.RemoveListener(MyIndexEvent.cardsHang, OnCardsHang);
			MyEventManager.instance.RemoveListener(MyIndexEvent.deckDraw, OnDeckDraw);
			MyEventManager.instance.RemoveListener(MyIndexEvent.scrapsCollect, OnScrapsCollect);
			MyEventManager.instance.RemoveListener(MyIndexEvent.cockpitTake, OnCockpitTake);
			MyEventManager.instance.RemoveListener(MyIndexEvent.burracoMake, OnBurracoMake);
			MyEventManager.instance.RemoveListener(MyIndexEvent.gameEnd, OnGameEnd);
			MyEventManager.instance.RemoveListener(MyIndexEvent.gameStart, OnGameStart);
		}
	}
	

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(PlayCards());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	
	IEnumerator PlayCards()
	{

		Shuffle(deck.myDeck);
		yield return StartCoroutine(SelectPlayerStartGame());
		yield return StartCoroutine(RemovePreStartCards());
		yield return StartCoroutine(InitialBunches());
		yield return StartCoroutine(ShowMeCards());


	}

	void Shuffle<T>(List<T> list)
	{
		System.Random random = new System.Random();
		int n = list.Count;
		while (n > 1)
		{
			int k = random.Next(n);
			n--;
			T temp = list[k];
			list[k] = list[n];
			list[n] = temp;
		}
	}


	IEnumerator SelectPlayerStartGame()
	{
		List<int> randomNumbers = new List<int>();
		System.Random random = new System.Random();
		for(int i = 0; i<4 ; i++)
		{
			int index;
			yield return new WaitForSeconds(0.5f);
			do index = random.Next(deckForStartGame.deck.Count);
			while (randomNumbers.Contains(index));
			randomNumbers.Add(index);
			deckForStartGame.deck[index].transform.position = new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z);
			deckForStartGame.deck[index].AbsoluteValue = index;
			deckForStartGame.deck[index].tag = cards[i].tag;
			myCardsForStart.Add(deckForStartGame.deck[index]);
		}
		int highestValue = myCardsForStart.Max(x => x.AbsoluteValue);
		
		CardForStartGame myCard = myCardsForStart.OrderByDescending(i => i.AbsoluteValue).FirstOrDefault();
		playerstart = myCard.gameObject.tag + "Player";
		print("il giocatore che inizia è : " + playerstart);
		

	}

	IEnumerator RemovePreStartCards()
	{
		yield return new WaitForSeconds(1.5f);

		//	TO DO  : una scritta con chi inizia

		foreach (CardForStartGame cardToDisable in myCardsForStart)
		{
			cardToDisable.gameObject.SetActive(false);
		}
		deckForStartGame.gameObject.SetActive(false);
		deckFake.SetActive(false);

	}

	IEnumerator InitialBunches()
	{
		int newIndex;
		float xOffset = 0;
		float zOffset = 0.03f;
		float yOffset = 0;
		float z = 0;
		foreach (Card card in deck.myDeck)
		{
			card.transform.position = new Vector3(playingDeckPosition.transform.position.x , playingDeckPosition.transform.position.y, playingDeckPosition.transform.position.z - z);
			card.IsVisible = false;
			z += 0.02f;
		}
		for (int i = 0; i < 11; i++)
		{


			for (int indexPlayer = 0; indexPlayer < 4; indexPlayer++)
			{
				newIndex = (indexPlayer + GetOffset()) % 4;

				yield return new WaitForSeconds(0.7f);
				Card newCard = deck.myDeck[deck.myDeck.Count -1];
				if (hands[newIndex].tag == "me")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x + xOffset, hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.identity;
					newCard.tag = "myCard";
					me.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);

				}
				else if (hands[newIndex].tag == "myMate")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x + xOffset, hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.identity;
					newCard.tag = "myMateCard";
					myMate.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}
				else if (hands[newIndex].tag == "leftOpponent")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x , hands[newIndex].transform.position.y - yOffset, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					newCard.tag = "leftOpponentCard";
					leftOpponent.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}
				else
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x , hands[newIndex].transform.position.y -yOffset, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					newCard.tag = "rightOpponentCard";
					rightOpponent.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}

				//adesso faccio un test, le nascondo e le faccio vedere solo con una coroutine successiva
				//newCard.IsVisible = hands[newIndex].tag == "me" ? true : false;				//vedo solo le mie
				
			}
			xOffset += 0.9f;
			zOffset += 0.03f;
			yOffset += 0.7f;
		}
		zOffset = 0;
		float zOffsetCockpit = 0.24f;
		float xOffsetCockpit = 0.05f;
		float yOffsetCockpit = 0.05f;
		for (int i = 0; i < 11; i++)
		{


			for (int index = 0; index < 2; index++)
			{
				yield return new WaitForSeconds(0.3f);
				Card newCard = deck.myDeck[0];

				if (index == 0)
				{

					newCard.transform.position = new Vector3(cockpitPosition.transform.position.x, cockpitPosition.transform.position.y, cockpitPosition.transform.position.z -zOffset);
					newCard.transform.rotation = Quaternion.identity;
					newCard.tag = "firstCockpitCard";
					firstCockpit.Add(newCard);
					deck.myDeck.RemoveAt(0);
				}
				else
				{

					newCard.transform.position = new Vector3(cockpitPosition.transform.position.x + xOffsetCockpit, cockpitPosition.transform.position.y + yOffsetCockpit, cockpitPosition.transform.position.z + zOffsetCockpit - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					newCard.tag = "secondCockpitCard";
					secondCockpit.Add(newCard);
					deck.myDeck.RemoveAt(0);
				}


			}
			zOffset += 0.02f;
			
		}

		Card initCard = deck.myDeck[deck.myDeck.Count - 1];
		initCard.transform.position = new Vector3(refusePosition.transform.position.x, refusePosition.transform.position.y, refusePosition.transform.position.z - zOffset);
		initCard.transform.rotation = Quaternion.identity;
		initCard.IsVisible = true;
		initCard.tag = "refuse";
		refuseCards.Add(initCard);
		deck.myDeck.RemoveAt(deck.myDeck.Count - 1);


		//------------------------- TEST sulle carte date ------------------------------------------------------

		//me.CountJolly();
		//me.CountPins();
		//myMate.CountJolly();
		//myMate.CountPins();
		//leftOpponent.CountJolly();
		//leftOpponent.CountPins();
		//rightOpponent.CountJolly();
		//rightOpponent.CountPins();
		//print("Il numero di Jolly che ho io è : " + me.NumberOfJolly + " Il numero di Pinelle è : " + me.NumberOfPins);
		//print("Il numero di Jolly che ha Left è : " + leftOpponent.NumberOfJolly + " Il numero di Pinelle  è : " + leftOpponent.NumberOfPins);
		//print("Il numero di Jolly che ha myMate : " + myMate.NumberOfJolly + " Il numero di Pinelle è : " + myMate.NumberOfPins);
		//print("Il numero di Jolly che ha Right è : " + rightOpponent.NumberOfJolly + " Il numero di Pinelle  è : " + rightOpponent.NumberOfPins);
		//int numOfPins = 0, numOfJolly = 0, numOfRemainingCards = 0;
		//foreach(Card card in firstCockpit)
		//{
		//	if (card.CanBePin)
		//	{
		//		numOfPins++;
		//	}
		//	if (card.CanBeJolly && !card.CanBePin)
		//	{
		//		numOfJolly++;
		//	}
		//}
		//print("Il numero di Jolly primo pozzetto è : " + numOfJolly + " Il numero di Pinelle  è : " + numOfPins);
		//numOfJolly = 0;
		//numOfPins = 0;

		//foreach (Card card in secondCockpit)
		//{
		//	if (card.CanBePin)
		//	{
		//		numOfPins++;
		//	}
		//	if (card.CanBeJolly && !card.CanBePin)
		//	{
		//		numOfJolly++;
		//	}
		//}
		//print("Il numero di Jolly secondo pozzetto è : " + numOfJolly + " Il numero di Pinelle  è : " + numOfPins);
		//numOfJolly = 0;
		//numOfPins = 0;

		//foreach (Card card in deck.myDeck)
		//{
		//	numOfRemainingCards++;
		//	if (card.CanBePin)
		//	{
		//		numOfPins++;
		//	}
		//	if (card.CanBeJolly && !card.CanBePin)
		//	{
		//		numOfJolly++;
		//	}

		//}
		//print("il numero delle carte rimasto è : " + numOfRemainingCards);
		//print("Il numero di Jolly rimasto è : " + numOfJolly + " Il numero di Pinelle rimaste  è : " + numOfPins);
		//print("L'indice della prima carta jolly o pinella è :" + Deck.IndexOfFirstJollyOrPin(me.myHand));
		//print("la carta del mazzo è " + deck.myDeck[0].Value +" "+deck.myDeck[0].Suit+" "+deck.myDeck[0].Color);
		//print("la prima carta della mia mano è " + me.myHand[0].Value + " " + me.myHand[0].Suit + " " + me.myHand[0].Color);

		// +++++++++++++++++++++++++ TESTATO E FUNZIONA ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//Deck.Swap2CardOf2list(deck.myDeck, me.myHand, 0, 0);

		//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//print("adesso la carta del mazzo è " + deck.myDeck[0].Value + " " + deck.myDeck[0].Suit + " " + deck.myDeck[0].Color);
		//print("adesso la prima carta della mia mano è " + me.myHand[0].Value + " " + me.myHand[0].Suit + " " + me.myHand[0].Color);

		//------------------------ fine TEST --------------------------------------------------------------------

	}

	IEnumerator ShowMeCards()
	{


		foreach(Card card in me.myHand)
		{
			yield return new WaitForSeconds(0.4f);
			card.IsVisible = true;
		}
		MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, playerstart));
	}

	int GetOffset()
	{
		int offset;
		if (playerstart == ME)
		{
			offset = 0;
		}
		else if (playerstart == LEFTOPPONENT)
		{
			offset = 1;
		}
		else if (playerstart == MYMATE)
		{
			offset = 2;
		}
		else if (playerstart == RIGHTOPPONENT)
		{
			offset = 3;
		}
		else
		{
			offset = -1;
			print("Errore!!! non ha preso nessun nome corretto");
		}
		return offset;
	}

	void CheckGamefinished()
	{
		if((me.myHand.Count == 0 && (me.CockpitAlreadyBeenTaken || myMate.CockpitAlreadyBeenTaken))|| (myMate.myHand.Count == 0 && (me.CockpitAlreadyBeenTaken || myMate.CockpitAlreadyBeenTaken)))
		{
			MyEventManager.instance.CastEvent(MyIndexEvent.gameEnd, new MyEventArgs(this.gameObject, me.tag, myMate.tag));
		}else if ((leftOpponent.myHand.Count == 0 && (leftOpponent.CockpitAlreadyBeenTaken || rightOpponent.CockpitAlreadyBeenTaken)) || (rightOpponent.myHand.Count == 0 && (leftOpponent.CockpitAlreadyBeenTaken || rightOpponent.CockpitAlreadyBeenTaken)))
		{
			MyEventManager.instance.CastEvent(MyIndexEvent.gameEnd, new MyEventArgs(this.gameObject, leftOpponent.tag, rightOpponent.tag));
		}

	}


	public void OnCardSelect(MyEventArgs e)
	{
		
		print("Sono entrato nell'evento OnCardSelect");
		e.cardSelected.IsSelected = !e.cardSelected.IsSelected;
		if (e.cardSelected.IsSelected)
		{
			e.cardSelected.transform.position = new Vector3(e.cardSelected.transform.position.x, e.cardSelected.transform.position.y + 0.2f, e.cardSelected.transform.position.z);
			cardsSelected.Add(e.cardSelected);
		}
		else
		{
			e.cardSelected.transform.position = new Vector3(e.cardSelected.transform.position.x, e.cardSelected.transform.position.y - 0.2f, e.cardSelected.transform.position.z);
			cardsSelected.Remove(e.cardSelected);
		}
		

	}

	public void OnCardsHang(MyEventArgs e)					//attaccare al tavolo
	{
		print("Sono entrato nell'evento OnCardsHang");
	}

	public void OnDeckDraw(MyEventArgs e)					//pescare dal mazzo di carte
	{
		print("Sono entrato nell'evento OnDeckDraw");
		Vector3 lastCardPosition = e.lastCardPosition;
		List<Card> currentDeck = e.deck;
		deck.myDeck[deck.myDeck.Count - 1].transform.position = new Vector3(lastCardPosition.x + 0.9f, lastCardPosition.y, lastCardPosition.z);
		deck.myDeck[deck.myDeck.Count - 1].IsVisible = true;
		deck.myDeck[currentDeck.Count - 1].tag = currentDeck[0].tag;
		currentDeck.Add(deck.myDeck[currentDeck.Count - 1]);
		deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
		print(" ho pescato la carta : " + currentDeck[currentDeck.Count - 1].Value + " "+ currentDeck[currentDeck.Count - 1].Suit + " " + currentDeck[currentDeck.Count - 1].Color);
		print(" la carta ha tag : " + currentDeck[currentDeck.Count - 1].tag);


	}

	public void OnScrapsCollect(MyEventArgs e)				//raccogliere
	{
		print("Sono entrato nell'evento OnScrapsCollect");
		if(!alreadyCollected && !alreadyFished)
		{
			alreadyCollected = true;
			Vector3 myLastCardPosition = new Vector3(me.myHand[me.myHand.Count - 1].transform.position.x, me.myHand[me.myHand.Count - 1].transform.position.y, me.myHand[me.myHand.Count - 1].transform.position.z);
			float xOffset = 0.9f;
			for(int index = 0;index < refuseCards.Count(); index++)
			{
				refuseCards[index].transform.position = new Vector3(myLastCardPosition.x + xOffset, myLastCardPosition.y, myLastCardPosition.z);
				refuseCards[index].tag = "myCard";
				me.myHand.Add(refuseCards[index]);
				refuseCards.Remove(refuseCards[index]);
				xOffset += 0.9f;
			}
		}
	}

	public void OnCockpitTake(MyEventArgs e)				//prendere i pozzetti
	{
		print("Sono entrato nell'evento OnCockpitTake");
	}

	public void OnBurracoMake(MyEventArgs e)				//fare burraco
	{
		print("Sono entrato nell'evento OnBurracoMake");
	}

	public void OnGameEnd(MyEventArgs e)
	{
		print("Sono entrato nell'evento OnGameEnd");
		string player1 = e.player1;		//non so se servono
		string player2 = e.player2;

		// TO DO -> metodo che conta i punti + coroutine che faccia qualche effetto

	}

	public void OnGameStart(MyEventArgs e)
	{
		string name = e.playerStart;
		Player player =  GameObject.FindGameObjectWithTag(name).GetComponent<Player>();
		print("Sono entrato nell'evento OnGameStart il giocatore che inizia è : " + player.tag);
		if(name == ME)
		{
			print(" tocca a me ");
			OrderHand(me.myHand, hands[0].transform.position);
			isCanGetInput = true;           //abilito l'input utente
			

			//alla fine scateno l'evento del prossimo giocatore
			//MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, LEFTOPPONENT));
		}else if(name == LEFTOPPONENT)
		{
			print(" tocca al giocatore di sinistra ");
			isCanGetInput = false;

			//alla fine
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, MYMATE));
		}
		else if (name == MYMATE)
		{
			print(" tocca al mio compagna ");
			isCanGetInput = false;

			//alla fine
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, RIGHTOPPONENT));
		}
		else if (name == RIGHTOPPONENT)
		{
			print(" tocca al giocatore di destra ");
			isCanGetInput = false;

			//alla fine
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, ME));
		}
		else
		{
			print(" ERRORE! non mi ha preso il giocatore ");
		}
	}

	private void OrderHand(List<Card> hand, Vector3 position)
	{

		IEnumerable<Card> query = hand.OrderBy(card => card.Suit).OrderBy(card => card.Value);
		hand = query.ToList();
		float xOffSet = 0;
		float zOffset = 0;
		foreach(Card card in hand)
		{
			card.transform.position = new Vector3(position.x + xOffSet, position.y, position.z - zOffset);
			xOffSet += 0.9f;
			zOffset += 0.2f;
		}

		foreach (Card card in hand)
		{
			print("  " + card.Value + "  "+ card.Suit + "  "+ card.Color);
		}

	}
}

