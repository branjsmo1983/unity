using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class Burraco : MonoBehaviour
{
	private const string ME = "mePlayer";
	private const string MYCARD = "myCard";
	private const string MYMATE = "myMatePlayer";
	private const string MYMATECARD = "myMateCard";
	private const string LEFTOPPONENT = "leftOpponentPlayer";
	private const string LEFTOPPONENTCARD = "leftOpponentCard";
	private const string RIGHTOPPONENT = "rightOpponentPlayer";
	private const string RIGHTOPPONENTCARD = "rightOpponentCard";
	private const string OURCANASTA = "ourCanasta";
	private const string THEIRCANASTA = "theirCanasta";
	private const int MAXCARDSFULLFACE = 16;
	private const int MAXCARDHALFFECE = 25;
	private const float Z_OFFSET = 0.2f;
	private const float X_OFFSET = 0.9f;
	private const float X_OFFSET_MID = 0.6f;
	private const float X_OFFSET_SMALL = 0.3f;

	[SerializeField]
	private TextMeshProUGUI text;															//testo durante il gioco

	[SerializeField]
	private Deck deck;																		//mazzo di gioco

	[SerializeField]
	private DeckForStartGame deckForStartGame;												//mazzo iniziale per estrarre le 4 carte

	[SerializeField]
	GameObject cockpitPosition;																// per sapere la transform dei pozzetti

	[SerializeField]
	GameObject deckFake;																	//posizione della sprite cardback che uso come finto mazzo iniziale

	[SerializeField]
	GameObject playingDeckPosition;															//per sapere la transform del mazzo di carte

	[SerializeField]
	GameObject refusePosition;                                                              // per sapere la transform degli scarti
	[SerializeField]
	GameObject nextRefusePosition;

	[SerializeField]
	internal GameObject[] hands = new GameObject[4];										//per sapere la transform delle 4 mani dei 4  giocatori

	[SerializeField]
	internal GameObject[] cards = new GameObject[4];                                        //per sapere la transform delle 4 carte iniziali per sapere chi inizia il gioco

	[SerializeField]
	GameObject firstOurCanastaPosition;

	[SerializeField]
	GameObject firsTheirCanastaPosition;

	[SerializeField]
	internal Table ourTable;

	[SerializeField]
	internal Table theirTable;

	internal List<Card> firstCockpit = new List<Card>();									// primo pozzetto
	internal List<Card> secondCockpit = new List<Card>();									// secondo pozzetto
	internal List<Card> refuseCards = new List<Card>();										// lista delle carte negli scarti
	private List<CardForStartGame> myCardsForStart = new List<CardForStartGame>();			// mi serve per ordinare le 4 carte e prendere la maggiore con linq
	internal string playerstart;                                                            // stringa del giocatore iniziale		
	internal bool firstcockpitAlreadyToken = false;                                         // se è stato già preso il primo pozzetto 
	internal bool secondcokpitAlreadyToken = false;
	internal bool isCanGetInput = false;													// per abilitare l'input all'utente o meno												
	internal bool orderbyValue = false;														//ordino solo per valore o anche per suit
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
		MyEventManager.instance.AddListener(MyIndexEvent.cardsAddToCanasta, OnAddCardsToCanasta);
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
			MyEventManager.instance.RemoveListener(MyIndexEvent.cardsAddToCanasta, OnAddCardsToCanasta);
		} 
	}
	

	// Start is called before the first frame update
	void Start()
    {
		//ourTable.canaste.Add(GetComponent<Canasta>());
		ourTable.canaste = new List<Canasta>();
		StartCoroutine(PlayCards());
    }

    // Update is called once per frame
    void Update()
    {
		CheckDistancePlayerCards(me.myHand,hands[0]);
		CheckDistance(refuseCards, refusePosition);

	}

	
	IEnumerator PlayCards()
	{

		Shuffle(deck.myDeck);
		yield return StartCoroutine(SelectPlayerStartGame());
		yield return StartCoroutine(RemovePreStartCards());
		yield return StartCoroutine(InitialBunches());
		yield return StartCoroutine(ShowMeCards());


	}

	public void ChangeOrder()
	{
		print("ho cliccato il bottone per cambiare l'ordine");
		orderbyValue = !orderbyValue;
		OrderHand(me.myHand, hands[0].transform.position);
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
		//print("il giocatore che inizia è : " + myCard.gameObject.tag);
		string namePlayer;
		if(playerstart == ME)
		{
			namePlayer = "io";
	
		}else if(playerstart == LEFTOPPONENT)
		{
			namePlayer = "l'avversario di sinistra";
		}else if(playerstart == MYMATE)
		{
			namePlayer = "il mio compagno";
		}else if(playerstart == RIGHTOPPONENT)
		{
			namePlayer = "l'avversario di destra";
		}
		else
		{
			namePlayer = "";
			print("non ha preso il controllo sul giocatore");
		}

		text.text = "Inizia : \r\n" + namePlayer;
		yield return new WaitForSeconds(1.2f);
		text.text = "";
	}

	IEnumerator RemovePreStartCards()
	{
		yield return new WaitForSeconds(1.5f);


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
					newCard.tag = MYCARD;
					me.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);

				}
				else if (hands[newIndex].tag == "myMate")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x + (xOffset * 2/3), hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.identity;
					newCard.tag = MYMATECARD;
					myMate.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}
				else if (hands[newIndex].tag == "leftOpponent")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x , hands[newIndex].transform.position.y - (yOffset *2/3), hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					newCard.tag = LEFTOPPONENTCARD;
					leftOpponent.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}
				else
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x , hands[newIndex].transform.position.y - (yOffset * 2/3), hands[newIndex].transform.position.z - zOffset);
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
		initCard.transform.position = new Vector3(refusePosition.transform.position.x, refusePosition.transform.position.y, refusePosition.transform.position.z);
		initCard.transform.rotation = Quaternion.identity;
		initCard.IsVisible = true;
		initCard.tag = "refuse";
		refuseCards.Add(initCard);
		deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
		nextRefusePosition.transform.position = new Vector3(refusePosition.transform.position.x + X_OFFSET, refusePosition.transform.position.y, refusePosition.transform.position.z - Z_OFFSET);


		//------------------------- TEST sulle carte date (lascio commentato per futuri controlli su pinelle)------------------------------------------------------

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

	void CheckGameFinished()
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
			me.cardsSelected.Add(e.cardSelected);
		}
		else
		{
			e.cardSelected.transform.position = new Vector3(e.cardSelected.transform.position.x, e.cardSelected.transform.position.y - 0.2f, e.cardSelected.transform.position.z);
			me.cardsSelected.Remove(e.cardSelected);
		}
		

	}



	public void OnDeckDraw(MyEventArgs e)					//pescare dal mazzo di carte
	{
		print("Sono entrato nell'evento OnDeckDraw");
		List<Card> currentDeck = e.deck;
		Card cardFished = e.cardFished;
		cardFished.transform.position = new Vector3(hands[0].transform.position.x + (X_OFFSET * me.myHand.Count), hands[0].transform.position.y, hands[0].transform.position.z - (Z_OFFSET * me.myHand.Count));
		cardFished.IsVisible = true;
		currentDeck.Add(cardFished);
		deck.myDeck.Remove(cardFished);


	}

	public void OnAddCardsToCanasta(MyEventArgs e)			//aggiunge le carte selezionate alla canasta
	{
		Vector3 initialPosition = e.canastaSelected.cards.OrderByDescending(c=>c.CurrentValue).ElementAt(0).transform.position;							//salvo la posizione iniziale della canasta
		// ------ per test
		print(" Sono entrato nel metodo per aggiungere carte alla canasta ");
		foreach(Card c in e.canastaSelected.cards)
		{
			print("" + c.name);
		}
		//------ fine test

		List<Card> canastaCards = new List<Card>();
		canastaCards.AddRange(e.canastaSelected.cards.ToArray().OrderByDescending(c=>c.CurrentValue));
		List<Card> cardsSelected = new List<Card>();
		cardsSelected.AddRange(me.cardsSelected.ToArray().OrderByDescending(c => c.CurrentValue));
		Canasta canasta = new Canasta
		{
			cards = canastaCards
			
		};
		canasta.GetTrisNumber();
		print(" il valore della canasta è : " + canasta.TrisValue);
		if(canasta.AreAddables(ref cardsSelected))
		{
			print(" le carte selezionate sono aggiungibili alla canasta ");
			foreach(Card card in cardsSelected)
			{
				card.tag = OURCANASTA;
				card.IsSelected = false;
				e.canastaSelected.cards.Add(card);
				me.myHand.Remove(card);
			}

			// chiamo il metodo che mostra le cards nella canasta
			ShowCanasta(e.canastaSelected.cards.OrderByDescending(C => C.CurrentValue).ToList(), initialPosition);
			//ordino la mia mano senza le carte aggiunte alla canasta
			OrderHand(me.myHand, hands[0].transform.position);
			//pulisco la lista di carte selezionate
			me.cardsSelected.Clear();

		}
		else
		{
			cardsSelected.Clear();
			print(" le carte selezionate NON sono aggiungibili alla cansta!!! ");
		}
		if(me.myHand.Count == 0 && !me.CockpitAlreadyBeenTaken && !myMate.CockpitAlreadyBeenTaken)
		{
			MyEventManager.instance.CastEvent(MyIndexEvent.cockpitTake, new MyEventArgs(this.gameObject,me));
		}

	}

	private void ShowCanasta(List<Card> cards, Vector3 initialPosition)
	{
		float yOffset = 0;
		float zOffset = 0.2f;
		foreach(Card card in cards)
		{
			card.transform.position = new Vector3(initialPosition.x, initialPosition.y - yOffset, initialPosition.z - zOffset);
			yOffset += 0.4f;
			zOffset += 0.2f;
		}

	}

	public void OnCardsHang(MyEventArgs e)                  //attaccare al tavolo
	{
		print("Sono entrato nell'evento OnCardsHang");
													// if(!me.HasCollected && !me.HasFished)
		if (me.cardsSelected.Count >= 3 && (me.HasCollected || me.HasFished))			//se non ho ancora nessuna canasta devo controllare di aver selezionato almeno 3 carte
		{
			if (Canasta.IsCanasta(ref me.cardsSelected))
			{
				int checkTris = Canasta.GetTrisNumber(me.cardsSelected);
				if (ourTable.canaste.Count > 0 && ourTable.canaste.Exists(c => c.TrisValue == checkTris && c.TrisValue != -1))
				{
					print("esiste già una canasta-tris di questo valore : " + checkTris);
					return;
				}
				print(" il valore dell'ipotetico tris è : " + Canasta.GetTrisNumber(me.cardsSelected));
	
				float yOffset = 0;
				float zOffset = 0.2f;
				float xOffset = 0.9f;

				//Canasta firstCanasta = GetComponent<Canasta>();			DA ERRORE, perchè?

				Canasta firstCanasta = new Canasta
				{
					cards = new List<Card>()
				};
				foreach (Card card in me.myHand.FindAll(c => c.IsSelected).OrderByDescending(c=> c.CurrentValue))
				{
					print("sto attacanado la carta : "+ card.Name);
					print("con nome : " + card.name);
					card.transform.position = new Vector3(firstOurCanastaPosition.transform.position.x + (xOffset * ourTable.canaste.Count), firstOurCanastaPosition.transform.position.y - yOffset,firstOurCanastaPosition.transform.position.z - zOffset -(zOffset * ourTable.canaste.Count));
					yOffset += 0.4f;
					zOffset += 0.2f;
					card.IsSelected = false;
					card.tag = OURCANASTA;
					firstCanasta.cards.Add(card);
					me.myHand.Remove(card);
				}
				OrderHand(me.myHand, hands[0].transform.position);
				firstCanasta.cards.OrderByDescending(c => c.CurrentValue);
				firstCanasta.GetTrisNumber();
				ourTable.canaste.Add(firstCanasta);
				me.cardsSelected.Clear();
				if (me.myHand.Count == 0 && !me.CockpitAlreadyBeenTaken && !myMate.CockpitAlreadyBeenTaken)
				{
					MyEventManager.instance.CastEvent(MyIndexEvent.cockpitTake, new MyEventArgs(this.gameObject, me));
				}
			}
			else
			{
				print("le carte scelte non formano una canasta");
				foreach(Card card in me.cardsSelected)
				{
					if(card.Value == Card.MyValues.due)
					{
						//rimetto a true perchè nei vari controlli precedenti alcune le avrei potute mettere a false
						card.CurrentValue = 15;
						card.CanBeJolly = true;
						card.CanBePin = true;
					}
					else if(card.Value == Card.MyValues.A)
					{
						card.CurrentValue = 14;
					}

				}
			}

		}
		else
		{
			print(" numero di carte selezionate : " + me.cardsSelected.Count);
			print(" ho pescato? : " + me.HasFished);
			print(" ho reccolto? : " + me.HasCollected);
			print("entro nel ramo in cui nel tavolo ho almeno una canasta oppure non ho selezionato almeno 3 carte dalla mia mano");
		}

	}

	public void OnScrapsCollect(MyEventArgs e)				//raccogliere o scartare
	{
		
		print("Sono entrato nell'evento OnScrapsCollect");
		
		if(!me.HasCollected && !me.HasFished)
		{
			print("Sono nel ramo in cui sto raccogliendo");
			if(refuseCards.Count == 1)
			{
				print(" rendo non scartabile l'unica carta degli scarti");
				refuseCards[0].IsDiscardable = false;
			}
			nextRefusePosition.transform.position = new Vector3(refusePosition.transform.position.x, refusePosition.transform.position.y, refusePosition.transform.position.z);
			me.HasCollected = true;
		
			float xOffset = 0.9f;
			float zOffset = 0.2f;
			Vector3 myLastCardPosition = new Vector3(hands[0].transform.position.x + (xOffset * me.myHand.Count), hands[0].transform.position.y, hands[0].transform.position.z - (zOffset * me.myHand.Count));

			
			xOffset = 0;
			zOffset = 0;
			int length = refuseCards.Count;
			for (int index = 0;index < length; index++)
			{
				print(refuseCards[index].name);
				refuseCards[index].transform.position = new Vector3(myLastCardPosition.x + xOffset, myLastCardPosition.y, myLastCardPosition.z - zOffset);
				refuseCards[index].tag = "myCard";
				me.myHand.Add(refuseCards[index]);
				xOffset += 0.9f;
				zOffset += 0.2f;
			}
			refuseCards.Clear();
			
		}
		else if ((me.HasCollected && me.cardsSelected.Count() == 1)|| (me.HasFished && me.cardsSelected.Count() == 1))
		{
			print("Sono nel ramo in cui devo scartare");
			if(!me.myHand.Find(c=>c.IsSelected == true).IsDiscardable)
			{
				print("sto cercando di scartare una carta che ho appena raccolto");
				return;
			}
			me.myHand.Find(c => c.IsSelected).transform.position = new Vector3(nextRefusePosition.transform.position.x, nextRefusePosition.transform.position.y, nextRefusePosition.transform.position.z);
			me.myHand.Find(c => c.IsSelected).tag = "refuse";
			me.cardsSelected.ElementAt(0).tag = "refuse";
			me.myHand.Find(c => c.IsSelected).IsSelected = false;
			refuseCards.Add( me.cardsSelected.ElementAt(0));
			print("Ho scartato la carta : " + me.cardsSelected.ElementAt(0));
			me.myHand.Remove(me.cardsSelected.ElementAt(0));
			me.cardsSelected.Clear();
			print("la carta scartata ha tag :" + refuseCards.ElementAt(0).tag);
			OrderHand(me.myHand, hands[0].transform.position);
			nextRefusePosition.transform.position = new Vector3(nextRefusePosition.transform.position.x + 0.9f, nextRefusePosition.transform.position.y, nextRefusePosition.transform.position.z - 0.2f);
			foreach(Card card in me.myHand)
			{
				card.IsDiscardable = true;
			}
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, LEFTOPPONENT));
			me.HasCollected = false;
			me.HasFished = false;
		}
		else
		{
			print("ho già raccolto o pescato ma il numero di carte selezionate è diverso da 1");
			if(me.cardsSelected.Count() > 0)
			{
				foreach(Card c in me.cardsSelected)
				{
					print("carta che mi vede ancora selezionata: " + c.name);
				}
			}
		}
	}

	public void OnCockpitTake(MyEventArgs e)				//prendere i pozzetti
	{
		print("Sono entrato nell'evento OnCockpitTake");
		if (secondcokpitAlreadyToken)
		{
			print("sono stati raccolti già entrambi i pozzetti");
		}
		
		float xOffset = 0;
		float yOffset = 0;
		float zOffset = 0.2f;
		bool isvisible = false;
		string tag = "";
		Vector3 initialPosition = new Vector3();
		List<Card> cockpit = new List<Card>();
		if (firstcockpitAlreadyToken)
		{
			cockpit = firstCockpit;
			firstcockpitAlreadyToken = true;
		}
		else
		{
			cockpit = secondCockpit;
			secondcokpitAlreadyToken = true;
		}

		if (e.playerTookCockpit.tag == ME)
		{
			print("ho preso io il pozzetto");
			initialPosition = hands[0].transform.position;
			xOffset = 0.9f;
			isvisible = true;
			tag = MYCARD;
		
		}
		else if(e.playerTookCockpit.tag == MYMATE)
		{
			print(" il pozzetto lo ha preso il mio compagno");
			initialPosition = hands[2].transform.position;
			xOffset = 0.6f;
			tag = MYMATECARD;

		}
		else if(e.playerTookCockpit.tag == LEFTOPPONENT)
		{
			print(" il pozzetto lo ha preso l'avversario di sinistra");
			initialPosition = hands[1].transform.position;
			yOffset = 0.47f;
			tag = LEFTOPPONENTCARD;

		}
		else if(e.playerTookCockpit.tag == RIGHTOPPONENT)
		{
			print(" il pozzetto lo ha preso l'avversario di destra");
			initialPosition = hands[3].transform.position;
			yOffset = 0.47f;
			tag = RIGHTOPPONENTCARD;
	
		}
		else
		{
			print(" Errore, non mi ha preso il tag del giocatore giusto ");
		}
		PlaceCockpit(cockpit, e.playerTookCockpit.myHand, initialPosition, xOffset, yOffset, zOffset, isvisible,tag);
	}

	private void PlaceCockpit(List<Card> cockpit,List<Card> hand, Vector3 initialposition, float x,float y, float z,bool isVisible,string tag )
	{
		float xOffset = 0;
		float yOffset = 0;
		float zOffset = 0;
		for(int index = 0; index < cockpit.Count; index++)
		{
			cockpit[index].transform.position = new Vector3(initialposition.x + xOffset, initialposition.y - yOffset, initialposition.z - z);
			if(tag == MYCARD || tag == MYMATECARD)
			{
				cockpit[index].transform.rotation = Quaternion.identity;
			}
			else
			{
				cockpit[index].transform.rotation = Quaternion.Euler(0, 0, 90);
			}
			cockpit[index].tag = tag;
			cockpit[index].IsVisible = isVisible;
			hand.Add(cockpit[index]);
			xOffset += x;
			yOffset += y;
			zOffset += z;

		}
		cockpit.Clear();
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
			
		}else if(name == LEFTOPPONENT)
		{
			print(" tocca al giocatore di sinistra ");
			isCanGetInput = false;

			// TO DO 1) Metodo per decidere se pescare o raccogliere -> booleano 
			//		 2) Coroutine per mostare che raccoglie o scarta
			//		 3) Metodo per cercare di attaccare
			//		 4) Metodo per cercare di calare canasta
			//		 5) Metodo per scartare una carta

			//da rimuovere, mi serve per adesso per giocare sempre io
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, MYMATE));
		}
		else if (name == MYMATE)
		{
			print(" tocca al mio compagna ");
			isCanGetInput = false;

			//da rimuovere, mi serve per adesso per giocare sempre io
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, RIGHTOPPONENT));
		}
		else if (name == RIGHTOPPONENT)
		{
			print(" tocca al giocatore di destra ");
			isCanGetInput = false;

			//da rimuovere, mi serve per adesso per giocare sempre io
			MyEventManager.instance.CastEvent(MyIndexEvent.gameStart, new MyEventArgs(this.gameObject, ME));
		}
		else
		{
			print(" ERRORE! non mi ha preso il giocatore ");
		}
	}

	private void OrderHand(List<Card> hand, Vector3 position)
	{
		
		IEnumerable<Card> query = orderbyValue? hand.OrderBy(card => card.Suit).OrderBy(card => card.Value) : hand.OrderBy(card => card.Value).OrderBy(card => card.Suit);
		hand = query.ToList();
		float xOffSet = 0;
		float zOffset = 0;
		foreach(Card card in hand)
		{
			card.transform.position = new Vector3(position.x + xOffSet, position.y, position.z - zOffset);
			card.IsVisible = true;
			xOffSet = hand.Count < MAXCARDSFULLFACE ? xOffSet += X_OFFSET : xOffSet += X_OFFSET_MID;
			zOffset += Z_OFFSET;
		}


	}

	private void CheckDistancePlayerCards(List<Card> hand, GameObject startPosition)
	{
		if ((hand.Count > MAXCARDSFULLFACE) && (hand.Count <= MAXCARDHALFFECE))
		{
			ResizeHand(hand, startPosition.transform.position, X_OFFSET_MID);
		}
		else if (me.myHand.Count > MAXCARDHALFFECE)
		{
			ResizeHand(hand, startPosition.transform.position, X_OFFSET_SMALL);
		}
	}

	private void CheckDistance(List<Card> hand, GameObject startPosition)
	{
		if ((hand.Count > MAXCARDSFULLFACE) && (hand.Count <= MAXCARDHALFFECE))
		{
			Resize(hand, startPosition.transform.position, X_OFFSET_MID);
		}
		else if (me.myHand.Count > MAXCARDHALFFECE)
		{
			Resize(hand, startPosition.transform.position, X_OFFSET_SMALL);
		}
	}

	private void ResizeHand(List<Card> hand, Vector3 position,float gap)
	{
		IEnumerable<Card> query = orderbyValue ? hand.OrderBy(card => card.Suit).OrderBy(card => card.Value) : hand.OrderBy(card => card.Value).OrderBy(card => card.Suit);
		hand = query.ToList();
		float xOffSet = 0;
		float zOffset = 0;
		foreach (Card card in hand)
		{
			card.transform.position = new Vector3(position.x + xOffSet, position.y, position.z - zOffset);
			card.IsVisible = true;
			xOffSet += gap;
			zOffset += 0.2f;
		}
		
	}

	private void Resize(List<Card> hand, Vector3 position, float gap)
	{
		float xOffSet = 0;
		float zOffset = 0;
		foreach (Card card in hand)
		{
			card.transform.position = new Vector3(position.x + xOffSet, position.y, position.z - zOffset);
			card.IsVisible = true;
			xOffSet += gap;
			zOffset += 0.2f;
		}

	}

	private bool ShouldCollect(List<Card> hand,List<Card> refuse,Table table,Player player)
	{
		if(hand.Count == 1 && (hand[0].Value == Card.MyValues.jolly|| hand[0].Value == Card.MyValues.due))
		{
			print("sono nel ramo in cui ho solo una carta, ed è un jolly o una pinella");
			if(refuse.Count == 1 && (refuse[0].Value == Card.MyValues.jolly || refuse[0].Value == Card.MyValues.due))
			{
				print("sono nel caso in cui in mano ho una pinella/jolly e negli scarti ho 1 carta pinella/jolly");
				return false;
			}
			if(table.existBurraco && IsAddable(table, hand[0]))
			{
				print("sono nel ramo in cui ho già un burraco e il jolly è attaccabile");
				return false; 
			}
			if(!table.existBurraco && refuse.Count > 1)
			{
				print("sono nel ramo in cui non ho ancora un burraco nel tavolo e le carte scartate sono + di 1");
				return true;
			}
			if (!table.existBurraco && refuse.Count == 1)
			{
				print("sono nel ramo in cui non ho ancora un burraco nel tavolo e ho solo una carta scartata");
				return false;
			}
		}
		if (hand.Count == 1 && (hand[0].Value != Card.MyValues.jolly && hand[0].Value != Card.MyValues.due))
		{
			print("sono nel ramo in cui ho solo una carta, non è ne un jolly ne una pinella");
			if (!player.CockpitAlreadyBeenTaken || player.CockpitAlreadyBeenTaken && table.existBurraco)
			{
				print("sono nel ramo in cui non ho preso il pozzetto oppure l'ho preso e ho già il burraco");
				if(refuse.Count == 1 && IsAddable(table, refuse[0]))
				{
					print("sono nel ramo in cui non ho ancora preso il pozzetto, negli scarti ho solo una carta e quella mi attacca al tavolo");
					return true;
				}
				else if (refuse.Count == 1 && !IsAddable(table, refuse[0]))
				{
					print("sono nel ramo in cui non ho ancora preso il pozzetto, negli scarti ho solo una carta MA quella NON mi attacca al tavolo");
					return false;
				}
				else
				{
					print("sono nel ramo in cui ho + di una carta negli scarti");
					foreach(Card card in refuse)
					{
						if (!IsAddable(table, card))
						{
							print("la carta " + card.name + " NON è aggiungibile!");
							return false;
						}
						else
						{
							print("la carta " + card.name + " è aggiungibile");
							continue;
						}
					}
					return true;
				}
			}
			else
			{
				print(" sono nel ramo in cui ho preso il pozzetto  e non ho fatto burraco");
				if(refuse.Count > 1)
				{
					print("se ho il monte scarti > 1 -> raccolgo");
					return true;
				}
				else
				{
					print("se ho il monte scarti == 1 devo controllare se la carta è attaccabile o meno");
					if (IsAddable(table, refuse[0]))
					{
						print("la carta scartata è attaccabile -> raccolgo");
						return true;
					}
					else
					{
						print("la carta scartata NON è attaccabile -> pesco");
						return false;
					}
					
				}
			}
		}
		else
		{
			print("sono nel ramo in cui in mano ho + di 1 carta");
		
			if(refuse.Count == 1)
			{
				print("sono nel ramo in cui ho solo una carta negli scarti");
				if (refuse[0].Value == Card.MyValues.due || refuse[0].Value == Card.MyValues.jolly)
				{
					print(" la carta scartata è un jolly o una pinella quindi la raccolgo");
					return true;
				}
				else
				{
					if (hand.Count(c => c.Value == refuse[0].Value) > 1)
					{
						print("ho 2 carte in mano con lo stesso valore della carta scartata");
						return true;
					}
					else if (hand.Exists(c => c.Suit == refuse[0].Suit && ((c.CurrentValue == refuse[0].CurrentValue - 1) || (c.CurrentValue == refuse[0].CurrentValue + 1) || (c.CurrentValue == refuse[0].CurrentValue - 2) || (c.CurrentValue == refuse[0].CurrentValue + 2))) &&
						!hand.Exists(c => c.Suit == refuse[0].Suit & c.CurrentValue == refuse[0].CurrentValue))
					{
						print(" sono nel ramo in cui in mano ho una carta di un numero o due più grandi o più piccoli di quella negli scarti e dello stesso seme");
						return true;
					}
					else
					{
						print("sono nel ramo in cui la carta non mi serve ne per il tris ne per la scala");
						return false;
					}
					
				}

			}
			else
			{
				print("sono nel ramo in cui ho + di una carta negli scarti");

				// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
				//						TO DO: 3 metodi che tornino un bool 1-se utili per il tavolo 2-se utili per la mano 3-se utili tra di loro 
				//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


			}

		}

			return false;
	}

	private bool AreUsefulToEachOther(List<Card> refuseCards)
	{
		if(refuseCards.Exists(c=> c.Value == Card.MyValues.J || c.Value == Card.MyValues.due))
		{
			return true;
		}
		foreach(Card card in refuseCards)
		{
			if(refuseCards.Count(x=>x.Value == card.Value) > 1															//buono per i tris
				|| (refuseCards.Exists(x => x.Suit == card.Suit && x.CurrentValue == card.CurrentValue + 1))			//combinazioni per un'ipotetica scala
				|| (refuseCards.Exists(x => x.Suit == card.Suit && x.CurrentValue == card.CurrentValue - 1)) 
				|| (refuseCards.Exists(x => x.Suit == card.Suit && x.CurrentValue == card.CurrentValue + 2)) 
				|| (refuseCards.Exists(x => x.Suit == card.Suit && x.CurrentValue == card.CurrentValue - 2)) 
				|| (refuseCards.Exists(x => x.Suit == card.Suit && x.CurrentValue == card.CurrentValue - 11)))			//caso di A e 3
			{
				print("sono nel ramo in cui ho almeno un'altra carta con lo stesso valore oppure");
				print("ho una carta dello stesso seme con un valore vicino ( +/- 1 o 2)");
				return true;
			}
			else
			{
				print("questa carta non ha nessun'altra buona per un tris o una scala");
				continue;
			}
		}
		return false;
	}

	private bool AreUsefulToHand(List<Card> hand, List<Card> refuseCards)
	{
		bool result = false;

		return result;
	}

	private bool AreUsefulToTable(Table table, List<Card> refuseCards)
	{
		bool result = false;

		return result;
	}

	private bool IsThereCanasta(List<Card> cards)
	{
		return (IsThereClearRummy(cards) || IsThereClearTris(cards) || IsThereRummy(cards) || IsThereTris(cards));
	}
		
	private bool IsThereClearTris(List<Card> cards)
	{
		print("sono nel metodo che controlla se ho un tris pulito");
		foreach(Card card in cards)
		{
			if(cards.Count(c=>c.CurrentValue == card.CurrentValue) > 2)
			{
				print("ho trovato 3 carte dello stesso valore -> ritorno true");
				return true;
			}
			else
			{
				print("NON  ho trovato 3 carte dello stesso valore -> cambio carta");
				continue;
			}
		}
		return false;
	}

	private bool IsThereTris(List<Card> cards)
	{
		print("sono nel metodo che controlla se ho un tris");
		foreach(Card card in cards)
		{
			if((cards.Count(c=>c.CurrentValue == card.CurrentValue) > 1) && (cards.Exists(c=>c.Value == Card.MyValues.due ) || (cards.Exists(c=>c.Value == Card.MyValues.jolly))))
			{
				print("ho trovato almeno 2 carte dello stesso valore e o un jolly o una pinella -> ritorno true");
				return true;
			}
			else
			{
				print("NON  ho trovato 2 carte dello stesso valore + jolly o pin -> cambio carta");
				continue;
			}
		}

		return false;
	}

	private bool IsThereClearRummy(List<Card> cards)
	{
		print("sono nel metodo che controlla se ho una scala pulita");
		foreach(Card card in cards)
		{
			if(card.Value == Card.MyValues.A)
			{
				if(cards.Exists(c=>c.Suit == card.Suit && c.Value == Card.MyValues.due) && cards.Exists(c => c.Suit == card.Suit && c.Value == Card.MyValues.tre))
				{
					print("sono nel ramo in cui oltre all'asso ho anche il 2 e il 3 dello stesso seme");
					return true;
				}else if (cards.Exists(c => c.Suit == card.Suit && c.Value == Card.MyValues.K) && cards.Exists(c => c.Suit == card.Suit && c.Value == Card.MyValues.Q))
				{
					print("sono nel ramo in cui oltre all'asso ho anche il K e il Q dello stesso seme");
					return true;
				}
				else
				{
					print("oltre l'A non ho ne il 2 e il 3 ne il K e il Q dello stesso seme -> cambio carta");
					continue;
				}
			}
			else
			{
				if (cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue + 1) && cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue - 1))
				{
					print("sono nel ramo in cui oltre alla mia carta ho 1 carta di un numero in più e 1 di un numero in meno");
					return true;
				}
				else if (cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue + 1) && cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue + 2))
				{
					print("sono nel ramo in cui oltre alla mia carta ho le 2 carte successive");
					return true;
				}
				else if (cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue - 1) && cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue - 2))
				{
					print("sono nel ramo in cui oltre alla mia carta ho le 2 precedenti");
					return true;
				}
				else
				{
					print("non ho nessuna carta vicina alla mia -> cambio carta");
					continue;
				}
			}
		}
		return false;
	}

	private bool IsThereRummy(List<Card> cards)
	{
		print("sono nel metodo che controlla se ho una scala");
		foreach(Card card in cards)
		{
			if(card.Value == Card.MyValues.A)
			{
				print("ho trovato un asso");
				if(cards.Exists(c=>c.Value == Card.MyValues.tre && c.Suit == card.Suit) && cards.Exists(c => c.CanBeJolly))
				{
					print("sono nel ramo in cui ho il 3 e una carta jolly");
					return true;
				}else if(cards.Exists(c => c.Value == Card.MyValues.due && c.Suit == card.Suit) && cards.Exists(c => c.CanBeJolly)||
						 cards.Exists(c => c.Value == Card.MyValues.due && c.Suit == card.Suit) && cards.Count(c=>c.Value == Card.MyValues.due) >1)
				{
					print("sono nel ramo in cui oltre al 2 ho un jolly o un altro due");
					return true;
				}
				else if (cards.Exists(c => c.Value == Card.MyValues.K && c.Suit == card.Suit) && cards.Exists(c => c.CanBeJolly))
				{
					print("sono nel ramo in cui oltre al K ho un jolly o un altro due");
					return true;
				}
				else if(cards.Exists(c => c.Value == Card.MyValues.Q && c.Suit == card.Suit) && cards.Exists(c => c.CanBeJolly))
				{
					print("sono nel ramo in cui oltre al Q ho un jolly o un altro due");
					return true;
				}
				else
				{
					print("ho l'asso ma non ho nessun 2,3,K o Q");
					continue;
				}
			}
			else
			{
				if((cards.Exists(c=> c.Suit == card.Suit && c.CurrentValue == card.CurrentValue - 1) ||
					cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue - 2)||
					cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue + 1)||
					cards.Exists(c => c.Suit == card.Suit && c.CurrentValue == card.CurrentValue + 2))&&
					cards.Exists(c => c.CanBeJolly)
					)
				{
					print("ho trovato una carta vicina + 1 jolly");
					return true;
				}
				else
				{
					print("NON ho trovato nessuna carta vicina con anche un jolly!!");
					continue;
				}
			}
		}
		return false;
	}



	private Canasta MakeCanasta(ref List<Card> refusecards)
	{
		//TO DO: testo il numero di carte che ho all'inizio del metodo
		print("all'inizio del metodo ho : " + refusecards.Count + " carte scartate");

		List<Card> myRefuseCards = new List<Card>();
		myRefuseCards = refusecards.ToList();
		Canasta canasta = new Canasta();
		if (IsThereClearRummy(refusecards))
		{
			List<Card> myCanasta = refuseCards
				.Where(c =>( myRefuseCards.Exists(card => card.Suit == c.Suit && (card.Value == c.Value + 1)) && myRefuseCards.Exists(card2 => card2.Suit == c.Suit &&(card2.Value == c.Value + 2))||
				 myRefuseCards.Exists(card => card.Suit == c.Suit && (card.Value == c.Value + 1)) && myRefuseCards.Exists(card2 => card2.Suit == c.Suit && (card2.Value == c.Value -1)) ||
				  myRefuseCards.Exists(card => card.Suit == c.Suit && (card.Value == c.Value - 1)) && myRefuseCards.Exists(card2 => card2.Suit == c.Suit && (card2.Value == c.Value - 2))||
				  myRefuseCards.Exists(card => card.Suit == c.Suit && (card.CurrentValue == c.CurrentValue + 1)) && myRefuseCards.Exists(card2 => card2.Suit == c.Suit && (card2.CurrentValue == c.CurrentValue + 2)) ||
				 myRefuseCards.Exists(card => card.Suit == c.Suit && (card.CurrentValue == c.CurrentValue + 1)) && myRefuseCards.Exists(card2 => card2.Suit == c.Suit && (card2.CurrentValue == c.CurrentValue - 1)) ||
				  myRefuseCards.Exists(card => card.Suit == c.Suit && (card.CurrentValue == c.CurrentValue - 1)) && myRefuseCards.Exists(card2 => card2.Suit == c.Suit && (card2.CurrentValue == c.CurrentValue - 2)))).ToList();
			canasta.cards = myCanasta;
			refuseCards.RemoveAll(c => myCanasta.Exists(card => c.Value == card.Value && c.Color == card.Color && c.Suit == card.Suit));

			//TO DO: metodo che ordina la canasta e cerca se ci sono altre carte da aggiungere

			return canasta;
		}
		else if (IsThereRummy(refuseCards))
		{

			return canasta;
		}
		else if (IsThereClearTris(refuseCards))
		{

			return canasta;
		}
		else if (IsThereTris(refuseCards))
		{

			return canasta;
		}
		else
		{
			print("se arrivo qui ho sbagliato qualcosa");
			return null;
		}

		
	}

	

	private bool IsAddable(Table table, Card card)
	{
		if(card.Value == Card.MyValues.jolly || card.Value == Card.MyValues.due)
		{
			print("c'è un jolly o un 2 negli scarti");
			return true;
		}
		
		foreach(Canasta canasta in table.canaste)
		{
			if (canasta.IsAddable(card))
			{
				print("ho trovato una canasta in cui la carta " + card.name + " è aggiungibile");
				return true;
			}
			else
			{
				print("ho trovato una canasta in cui la carta " + card.name + "NON è aggiungibile!!");
				continue;
			}
		}
		print("non ho trovato nessuna canasta a cui attaccare ");
		return false;
	}

}

