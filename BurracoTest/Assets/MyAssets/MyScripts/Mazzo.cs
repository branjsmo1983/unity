using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazzo : MonoBehaviour
{
	private const string JOLLY = "Jolly";
	private const string BLUE = "Blue";
	private const string RED = "Red";
	private const string CLUB = "Club";
	private const string DIAMONDS = "Diamonds";
	private const string HEARTS = "Hearts";
	private const string SPADES = "Spades";
	private static Card[] allCards = new Card[108];
	private static string[] suits = new string[] { CLUB, DIAMONDS, HEARTS , SPADES };
	private static string[] colors = new string[] { BLUE , RED };
	private static int[] values = new int[] { 1, 2, 3 , 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

	private List<Card> deck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PlayCards()
	{
		deck = GenerateDeck();
		Shuffle(deck);
	}

	internal static List<Card> GenerateDeck()
	{
		List<Card> newDeck = new List<Card>();

		foreach(string c in colors)
		{
			foreach(string s in suits)
			{
				foreach(int v in values)
				{
					newDeck.Add(GenerateCard(c, s, v));
				}
			}
		}

		newDeck.Add(GenerateCard(BLUE, JOLLY, 0));
		newDeck.Add(GenerateCard(RED, JOLLY, 0));
		newDeck.Add(GenerateCard(RED, JOLLY, 0));
		newDeck.Add(GenerateCard(BLUE, JOLLY, 0));

		return newDeck;
	}

	private static Card GenerateCard(string color, string suit, int value)
	{
		Card card = new Card();
		card.Colore = color;
		card.Seme = suit;
		card.Numero = value;
		switch (value)
		{
			case 0:
				card.Valore = 30;
				break;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				card.Valore = 5;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
				card.Valore = 10;
				break;
			case 1:
				card.Valore = 15;
				break;
			case 2:
				card.Valore = 20;
				break;
			default:
				card.Valore = -1;
				break;
		}

		return card;
	}

	void Shuffle<T>(List<T> list)
	{
		System.Random random = new System.Random();
		int n = list.Count;
		while( n > 1)
		{
			int k = random.Next(n);
			n--;
			T temp = list[n];
			list[n] = temp;
		}
	}

}
