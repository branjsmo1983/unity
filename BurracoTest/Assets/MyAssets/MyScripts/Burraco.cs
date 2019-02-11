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
	internal GameObject[] hands = new GameObject[4];										//per sapere la transform

	[SerializeField]
	internal GameObject[] cards = new GameObject[4];                                        //per sapere la transform

	private List<CardForStartGame> myCardsForStart = new List<CardForStartGame>();
	private string playerstart;
	private Player[] players = new Player[4];
	private string[] namesOfPlayers = new string[4];
	// Start is called before the first frame update
	void Start()
    {
		PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SetPlayersOrder()
	{
		namesOfPlayers[0] = playerstart;
		if(playerstart == "me")
		{
			namesOfPlayers[1] = "leftOpponent";
			namesOfPlayers[2] = "myMate";
			namesOfPlayers[3] = "rightOpponent";
		}else if(playerstart == "leftOpponent")
		{
			namesOfPlayers[1] = "myMate";
			namesOfPlayers[2] = "rightOpponent";
			namesOfPlayers[3] = "me";
		}else if(playerstart == "myMate")
		{
			namesOfPlayers[1] = "rightOpponent";
			namesOfPlayers[2] = "me";
			namesOfPlayers[3] = "leftOpponent";
		}else if(playerstart == "rightOpponent")
		{
			namesOfPlayers[1] = "me";
			namesOfPlayers[2] = "leftOpponent";
			namesOfPlayers[3] = "myMate";
		}
		else
		{
			print("Errore!!! non ha preso nessun nome corretto");
		}
	}

	public void PlayCards()
	{

		Shuffle(deck.myDeck);
		//Deal();
		StartCoroutine(SelectPlayerStartGame());
		StopCoroutine(SelectPlayerStartGame());
		CreateMystartHand();
		//StartCoroutine(CreateAllDecks());
		//StartToPlay(playerstart);

	}

	IEnumerator SelectPlayerStartGame()
	{
		System.Random random = new System.Random();
		for(int i = 0; i<4 ; i++)
		{
			yield return new WaitForSeconds(0.5f);
			int index = random.Next(deckForStartGame.deck.Count);
			CardForStartGame newCard = Instantiate(deckForStartGame.deck[index], new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z), Quaternion.identity, cards[i].transform);
			newCard.AbsoluteValue = index;
			newCard.gameObject.tag = cards[i].tag;
			myCardsForStart.Add(newCard);

		}
		int highestValue = myCardsForStart.Max(x => x.AbsoluteValue);
		
		CardForStartGame myCard = myCardsForStart.OrderByDescending(i => i.AbsoluteValue).FirstOrDefault();
		playerstart = myCard.gameObject.tag;
		print("il giocatore che inizia la partita è : " + playerstart);

		StartCoroutine(RemovePreStartCards());

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
		foreach(Card card in deck.myDeck)
		{
			card.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
			card.IsVisible = true;
			xOffset += 0.3f;
			zOffset += 0.03f;

		}
	}

	void CreateMystartHand()
	{
		
		int indexPlayer = 0;
		for (indexPlayer = 0;indexPlayer < 4; indexPlayer++)
		{
			float xOffset = 0;
			float zOffset = 0.03f;
			float yOffset = 0;
			for (int i = 0; i < 11; i++)
			{
				Card newCard;
				if((indexPlayer == 0) || (indexPlayer == 2))
				{ 
					newCard = Instantiate(deck.myDeck[0], new Vector3(hands[indexPlayer].transform.position.x + xOffset, hands[indexPlayer].transform.position.y, hands[indexPlayer].transform.position.z + zOffset),Quaternion.identity, hands[indexPlayer].transform);
					deck.myDeck.RemoveAt(0);
				}
				else
				{
					newCard = Instantiate(deck.myDeck[0], new Vector3(hands[indexPlayer].transform.position.x, hands[indexPlayer].transform.position.y - yOffset, hands[indexPlayer].transform.position.z + zOffset), Quaternion.Euler(0, 0, 90), hands[indexPlayer].transform);
					deck.myDeck.RemoveAt(0);
				}
				newCard.IsVisible = true;
				xOffset += 0.9f;
				zOffset += 0.03f;
				yOffset += 0.7f;
			}
			
		}

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

	IEnumerator CreateAllDecks()
	{
		System.Random random = new System.Random();
		float xOffset = 0;
		float zOffset = 0.03f;
		List<int> numbersDrawn = new List<int>();							//tengo in memoria i numeri già estratti
		for(int indiceCarta = 0; indiceCarta < 11; indiceCarta++)			// per ognuna delle 11 carte della mano iniziale
		{
			for(int indicePlayer = 0; indicePlayer < 4; indicePlayer++)		// per ognuno dei 4 giocatori
			{
				yield return new WaitForSeconds(0.1f);
				int index;
				do index = random.Next(deck.deck.Length);
				while (numbersDrawn.Contains(index));
				deck.deck[index].transform.position = new Vector3(players[indicePlayer].hand.initialHand[indiceCarta].transform.position.x + xOffset, players[indicePlayer].hand.initialHand[indiceCarta].transform.position.y, players[indicePlayer].hand.initialHand[indiceCarta].transform.position.z +zOffset);
				if(namesOfPlayers[indicePlayer] == "me")
				{
					deck.deck[index].IsVisible = true;			//le mie carte le vedo
				}
				else
				{
					deck.deck[index].IsVisible = false;			//quelle degli altri no
				}
				deck.isAssigned[index] = true;
				players[indicePlayer].hand.initialHand[indiceCarta] = deck.deck[index];
				xOffset += 0.3f;
				zOffset += 0.03f;
			}
		}


		//una cosa simile a queste 2 parti commentate ma con l'altro deck

		//float xOffset = 0;
		//float zOffset = 0.03f;
		//foreach (Card card in deck.myDeck)
		//{
		//	card.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
		//	card.IsVisible = true;
		//	xOffset += 0.3f;
		//	zOffset += 0.03f;

		//}

		//System.Random random = new System.Random();
		//for (int i = 0; i < 4; i++)
		//{
		//	yield return new WaitForSeconds(0.1f);
		//	int index = random.Next(deckForStartGame.deck.Count);
		//	CardForStartGame newCard = Instantiate(deckForStartGame.deck[index], new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z), Quaternion.identity, cards[i].transform);
		//	newCard.AbsoluteValue = index;
		//	newCard.gameObject.tag = cards[i].tag;
		//	myCardsForStart.Add(newCard);

		//}

		
	}


	void StartToPlay(string tagPlayerToStart)
	{
		

	}

	
}
