using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

	[SerializeField]
	internal Card[] deck = new Card[108];							// mazzo di carte completo

	[SerializeField]
	internal List<Card> myDeck;										// lista di carte, inizialmente uguale al mazzo

	private void FillDeck()
	{
		for(int index = 0; index < deck.Length; index++)
		{
			myDeck.Add(deck[index]);
		}
	}

	private void Awake()
	{
		FillDeck();
	}

	// Start is called before the first frame update
	void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
