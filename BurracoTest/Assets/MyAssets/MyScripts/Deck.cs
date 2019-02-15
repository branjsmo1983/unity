using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

	[SerializeField]
	internal Card[] deck = new Card[108];                           // mazzo di carte completo

	internal bool[] isAssigned = new bool[108];

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

	internal static int IndexOfFirstJollyOrPin(List<Card> cards)
	{
		int result = -1;
		int index = 0;
		foreach(Card card in cards)
		{
			if (card.CanBeJolly)
			{
				result = index;
				break;
			}
			else
			{
				index++;
			}

		}
		return result;
	}

	internal static void Swap2CardOf2list(List<Card> firstList,List<Card> secondList, int indexFirstList, int indexSecondList)
	{
		Card cardOfFirstList = firstList[indexFirstList];
		Card cardOfSecondList = secondList[indexSecondList];
		Vector3 positionCardFirstList = new Vector3(firstList[indexFirstList].transform.position.x, firstList[indexFirstList].transform.position.y, firstList[indexFirstList].transform.position.z);
		Vector3 positionCardSecondList = new Vector3(secondList[indexFirstList].transform.position.x, secondList[indexFirstList].transform.position.y, secondList[indexFirstList].transform.position.z);
		firstList[indexFirstList].transform.position = positionCardSecondList;
		secondList[indexSecondList].transform.position = positionCardFirstList;
		secondList[indexSecondList] = cardOfFirstList;
		firstList[indexFirstList] = cardOfSecondList;
	}
}
