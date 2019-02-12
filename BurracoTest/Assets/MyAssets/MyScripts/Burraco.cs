using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Burraco : MonoBehaviour
{

	[SerializeField]
	private Deck deck;

	[SerializeField]
	private DeckForStartGame deckForStartGame;

	[SerializeField]
	GameObject deckFake;

	[SerializeField]
	GameObject playingDeckPosition;

	[SerializeField]
	GameObject cockpitPosition;

	[SerializeField]
	internal GameObject[] hands = new GameObject[4];										//per sapere la transform

	[SerializeField]
	internal GameObject[] cards = new GameObject[4];                                        //per sapere la transform

	internal List<Card> firstCockpit, secondCockpit;

	private List<CardForStartGame> myCardsForStart = new List<CardForStartGame>();
	private string playerstart;
	[SerializeField]
	private Player me;
	[SerializeField]
	private Player myMate;
	[SerializeField]
	private Player leftOpponent;
	[SerializeField]
	private Player rightOpponent;

	// Start is called before the first frame update
	void Start()
    {
		PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	
	public void PlayCards()
	{

		Shuffle(deck.myDeck);
		//Deal();
		StartCoroutine(SelectPlayerStartGame());
		StopCoroutine(SelectPlayerStartGame());
		
		

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

	//Serve solo per testare come creare un metodo che crei una carta vicino l'altra
	void Deal()
	{
		float xOffset = 0;
		float zOffset = 0.03f;
		foreach (Card card in deck.myDeck)
		{
			card.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
			card.IsVisible = true;
			xOffset += 0.3f;
			zOffset += 0.03f;

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
			CardForStartGame newCard = Instantiate(deckForStartGame.deck[index], new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z), Quaternion.identity, cards[i].transform);
			newCard.AbsoluteValue = index;
			newCard.gameObject.tag = cards[i].tag;
			myCardsForStart.Add(newCard);

		}
		int highestValue = myCardsForStart.Max(x => x.AbsoluteValue);
		
		CardForStartGame myCard = myCardsForStart.OrderByDescending(i => i.AbsoluteValue).FirstOrDefault();
		playerstart = myCard.gameObject.tag;
		print("il giocatore che inizia è : " + playerstart);
		StartCoroutine(RemovePreStartCards());

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
		//per adesso funziona ma non so come gestire le carte
		//StartCoroutine(CreateMyStartHand());
		StartCoroutine(InitialBunches());

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
					me.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);

				}
				else if (hands[newIndex].tag == "myMate")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x + xOffset, hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.identity;
					myMate.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}
				else if (hands[newIndex].tag == "leftOpponent")
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x , hands[newIndex].transform.position.y - yOffset, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					leftOpponent.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}
				else
				{
					newCard.transform.position = new Vector3(hands[newIndex].transform.position.x , hands[newIndex].transform.position.y -yOffset, hands[newIndex].transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					rightOpponent.myHand.Add(newCard);
					deck.myDeck.RemoveAt(deck.myDeck.Count - 1);
				}

				newCard.IsVisible = hands[newIndex].tag == "me" ? true : false;				//vedo solo le mie

			}
			xOffset += 0.9f;
			zOffset += 0.03f;
			yOffset += 0.7f;
		}
		zOffset = 0;
		for (int i = 0; i < 11; i++)
		{


			for (int index = 0; index < 2; index++)
			{

				Card newCard = deck.myDeck[0];

				if (index == 0)
				{
					newCard.transform.position = new Vector3(cockpitPosition.transform.position.x, cockpitPosition.transform.position.y, cockpitPosition.transform.position.z -zOffset);
					newCard.transform.rotation = Quaternion.identity;
					firstCockpit.Add(newCard);
					deck.myDeck.RemoveAt(0);
				}
				else
				{
					newCard.transform.position = new Vector3(cockpitPosition.transform.position.x, cockpitPosition.transform.position.y, cockpitPosition.transform.position.z - zOffset);
					newCard.transform.rotation = Quaternion.Euler(0, 0, 90);
					secondCockpit.Add(newCard);
					deck.myDeck.RemoveAt(0);
				}


			}
			zOffset += 0.02f;
		}

			}


	//versione in cui clonavo le carte invece di spostarle dal deck
	//IEnumerator CreateMyStartHand()
	//{
		
	//	int newIndex;
	//	float xOffset = 0;
	//	float zOffset = 0.03f;
	//	float yOffset = 0;
	//	for (int i = 0; i < 11; i++)  
	//	{
			
		
	//		for (int indexPlayer = 0; indexPlayer < 4; indexPlayer++)
	//		{
	//			newIndex = (indexPlayer + GetOffset()) % 4;
				
	//			yield return new WaitForSeconds(0.7f);
	//			Card newCard;
	//			if(hands[newIndex].tag == "me")
	//			{ 
	//				newCard = Instantiate(deck.myDeck[0], new Vector3(hands[newIndex].transform.position.x + xOffset, hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset),Quaternion.identity, hands[newIndex].transform);
	//				me.myHand.Add(newCard);
	//				deck.myDeck.RemoveAt(0);
					
	//			}
	//			else if(hands[newIndex].tag == "myMate")
	//			{
	//				newCard = Instantiate(deck.myDeck[0], new Vector3(hands[newIndex].transform.position.x + xOffset, hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset), Quaternion.identity, hands[newIndex].transform);
	//				myMate.myHand.Add(newCard);
	//				deck.myDeck.RemoveAt(0);
	//			}
	//			else if(hands[newIndex].tag == "leftOpponent")
	//			{
	//				newCard = Instantiate(deck.myDeck[0], new Vector3(hands[newIndex].transform.position.x, hands[newIndex].transform.position.y - yOffset, hands[newIndex].transform.position.z - zOffset), Quaternion.Euler(0, 0, 90), hands[newIndex].transform);
	//				leftOpponent.myHand.Add(newCard);
	//				deck.myDeck.RemoveAt(0);
	//			}
	//			else
	//			{
	//				newCard = Instantiate(deck.myDeck[0], new Vector3(hands[newIndex].transform.position.x, hands[newIndex].transform.position.y - yOffset, hands[newIndex].transform.position.z - zOffset), Quaternion.Euler(0, 0, 90), hands[newIndex].transform);
	//				rightOpponent.myHand.Add(newCard);
	//				deck.myDeck.RemoveAt(0);
	//			}

	//			//newCard.IsVisible = hands[newIndex].tag == "me" ? true : false;				//vedo solo le mie
	//			//per test le mettop tutte a true
	//			newCard.IsVisible = true;
	//		}
	//		xOffset += 0.9f;
	//		zOffset += 0.03f;
	//		yOffset += 0.7f;
	//	}

	//	print("La lista delle mie carte è : ");
	//	foreach(Card card in me.myHand)
	//	{
	//		print(card.Value + " " + card.Suit + " " + card.Color);
	//	}
	//	me.CountJolly();
	//	me.CountPins();
	//	myMate.CountJolly();
	//	myMate.CountPins();
	//	leftOpponent.CountJolly();
	//	leftOpponent.CountPins();
	//	rightOpponent.CountJolly();
	//	rightOpponent.CountPins();
	//	print("Il numero di Jolly che ho io è : " + me.NumberOfJolly + " Il numero di Pinelle è : " + me.NumberOfPins);
	//	print("Il numero di Jolly che ha Left è : " + leftOpponent.NumberOfJolly + " Il numero di Pinelle  è : " + leftOpponent.NumberOfPins);
	//	print("Il numero di Jolly che ha myMate : " + myMate.NumberOfJolly + " Il numero di Pinelle è : " + myMate.NumberOfPins);
	//	print("Il numero di Jolly che ha Right è : " + rightOpponent.NumberOfJolly + " Il numero di Pinelle  è : " + rightOpponent.NumberOfPins);
	//	int numOfPins = 0, numOfJolly = 0, numOfRemainingCards = 0;
	//	foreach(Card card in deck.myDeck)
	//	{
	//		numOfRemainingCards++;
	//		if (card.CanBePin)
	//		{
	//			numOfPins++;
	//		}
	//		if (card.CanBeJolly && !card.CanBePin)
	//		{
	//			numOfJolly++;
	//		}

	//	}
	//	print("il numero delle carte rimasto è : " + numOfRemainingCards);
	//	print("Il numero di Jolly rimasto è : " + numOfJolly + " Il numero di Pinelle rimaste  è : " + numOfPins);
	//	// To Do : da qui inizia il gioco

	//}

	int GetOffset()
	{
		int offset;
		if (playerstart == "me")
		{
			offset = 0;
		}
		else if (playerstart == "leftOpponent")
		{
			offset = 1;
		}
		else if (playerstart == "myMate")
		{
			offset = 2;
		}
		else if (playerstart == "rightOpponent")
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






	void StartToPlay(string tagPlayerToStart)
	{
		

	}

	
}

