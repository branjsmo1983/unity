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
	internal Hand[] hands = new Hand[4];										//per sapere la transform

	[SerializeField]
	internal Card[] cards = new Card[4];                                        //per sapere la transform

	private List<CardForStartGame> myCardsForStart = new List<CardForStartGame>();
	private string playerstart;
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
		Deal();
		StartCoroutine(SelectPlayerStartGame());
		StopCoroutine(SelectPlayerStartGame());
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

		yield return new WaitForSeconds(0.1f);
	}


	void StartToPlay(string tagPlayerToStart)
	{
		

	}

	
}
