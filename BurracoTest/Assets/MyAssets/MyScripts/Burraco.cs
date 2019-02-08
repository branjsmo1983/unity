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
	internal Hand myHand, teammateHand, rightOpponentHand, leftOpponentHand;

	[SerializeField]
	internal Card[] cards = new Card[4];                                        //per sapere la transform

	private List<CardForStartGame> myCardsForStart;
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
		//foreach(Card card in deck.myDeck)
		//{
		//	print(card.name+"_"+card.Value+"_"+card.Suit+"_"+card.Color+"_"+card.Cost);
		//}
		Deal();
		StartCoroutine(TestExtracCards());

	}

	IEnumerator TestExtracCards()
	{
		System.Random random = new System.Random();
		for(int i = 0; i<4 ; i++)
		{
			yield return new WaitForSeconds(0.08f);
			int index = random.Next(deckForStartGame.deck.Count);
			CardForStartGame newCard = Instantiate(deckForStartGame.deck[index], new Vector3(cards[i].transform.position.x, cards[i].transform.position.y, cards[i].transform.position.z), Quaternion.identity, cards[i].transform);
			newCard.AbsoluteValue = index;
			myCardsForStart.Add(newCard);
			print("la carta : " + deckForStartGame.deck[index].name);
			print("diventata : " + newCard.name);
			print("ha valore estratto : " + newCard.AbsoluteValue);


		}
		var mystartCard = myCardsForStart.Max(x => x.AbsoluteValue);
		var myCard = myCardsForStart.OrderByDescending(i => i.AbsoluteValue).Take(1);

		print("");
		print("");
		print("");
		;	}

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

	

	
}
