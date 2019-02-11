using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	[SerializeField]
	internal List<Card> myHand = new List<Card>();

	internal Card[] initialHand = new Card[11];
	
	public int NumberOfPins { get; set; }
	public int NumberOfJolly { get; set; }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	internal int CountNumbersOfCards()
	{
		return myHand.Count;
	}

}
