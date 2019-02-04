using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burraco : MonoBehaviour
{


	[SerializeField]
	internal Sprite[] cardFaces,cardBacks;

	[SerializeField]
	private GameObject cardPrefab;

	public static string[] suits = new string[] { "Clubs", "Diamonds", "Hearts", "Spades" };
	public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
	public static string[] colors = new string[] { "Red", "Blue" };

	internal List<string> deck;
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
		deck = GenerateDeck();
		Shuffle(deck);
		//to remove, it's only for testing
		foreach(string card in deck)
		{
			print(card);
		}
		BurracoDeal();

	}

	public static List<string> GenerateDeck()
	{

		List<string> newDeck = new List<string>();
		foreach(string color in colors)
		{
			foreach(string suit in suits)
			{
				foreach(string value in values)
				{
					newDeck.Add(value + "_" + suit + "_" + color);
				}
			}
		}
		newDeck.Add("1_Jolly_Blue");
		newDeck.Add("2_Jolly_Blue");
		newDeck.Add("1_Jolly_Red");
		newDeck.Add("2_Jolly_Red");
		return newDeck;
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

	void BurracoDeal()
	{
		float xOffset = 0;
		float zOffset = 0.03f;
		foreach(string card in deck)
		{
			GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), Quaternion.identity);
			newCard.name = card;

			xOffset += 0.15f;
			zOffset += 0.03f;
		}
	}


}
