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

	// Start is called before the first frame update
	void Start()
    {
		PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	int GetOffset()
	{
		int offset;
		if(playerstart == "me")
		{

			offset = 0;

		}
		else if(playerstart == "leftOpponent")
		{
			offset = 1;


		}
		else if(playerstart == "myMate")
		{
			offset = 2;


		}
		else if(playerstart == "rightOpponent")
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


	public void PlayCards()
	{

		Shuffle(deck.myDeck);
		//Deal();
		StartCoroutine(SelectPlayerStartGame());
		StopCoroutine(SelectPlayerStartGame());
		
		

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

	IEnumerator CreateMystartHand()
	{
		int playerOffset = GetOffset();
		int newIndex;
		float xOffset = 0;
		float zOffset = 0.03f;
		float yOffset = 0;
		for (int i = 0; i < 11; i++)  
		{
			
		
			for (int indexPlayer = 0; indexPlayer < 4; indexPlayer++)
			{
				newIndex = (indexPlayer + playerOffset) % 4;
				yield return new WaitForSeconds(0.7f);
				Card newCard;
				if((hands[indexPlayer].tag == "me") || (hands[indexPlayer].tag == "myMate"))
				{ 
					newCard = Instantiate(deck.myDeck[0], new Vector3(hands[newIndex].transform.position.x + xOffset, hands[newIndex].transform.position.y, hands[newIndex].transform.position.z - zOffset),Quaternion.identity, hands[newIndex].transform);
					deck.myDeck.RemoveAt(0);
				}
				else if((hands[indexPlayer].tag == "leftOpponent") || (hands[indexPlayer].tag == "rightOpponent"))
				{
					newCard = Instantiate(deck.myDeck[0], new Vector3(hands[newIndex].transform.position.x, hands[newIndex].transform.position.y - yOffset, hands[newIndex].transform.position.z - zOffset), Quaternion.Euler(0, 0, 90), hands[newIndex].transform);
					deck.myDeck.RemoveAt(0);
				}
				else
				{
					print("Errore, non ha preso i tag");
					newCard = null;
				}
				newCard.IsVisible = true;
				
			}
			xOffset += 0.9f;
			zOffset += 0.03f;
			yOffset += 0.7f;
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
		StartCoroutine(CreateMystartHand());

	}


		
	


	void StartToPlay(string tagPlayerToStart)
	{
		

	}

	
}

