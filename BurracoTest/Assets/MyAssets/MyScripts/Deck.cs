using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

	[SerializeField]
	internal Card[] deck = new Card[108];

	public List<Card> myDeck;


	private void FillDeck()
	{
		if (deck != null)
		{
			for (int i = 0; i < deck.Length; i++)
			{
				myDeck.Add(deck[i]);
			}
		}
		else
		{
			print("ERRORE :  il mazzo non può essere null !!!!!");
		}
	}
	// Start is called before the first frame update
	void Start()
    {
		FillDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
