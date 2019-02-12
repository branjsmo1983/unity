using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	internal List<Card> myHand;
	
	public int NumberOfPins { get; set; }
	public int NumberOfJolly { get; set; }
	internal bool IsMyRound { get; set; }
	internal bool CockpitAlreadyBeenTaken { get; set; }
	internal bool Iwon { get; set; }
	internal string Name { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	internal void CountPins()
	{
		foreach(Card card in myHand)
		{
			if (card.CanBePin)
			{
				NumberOfPins++;
			}
		}
	}
	internal void CountJolly()
	{
		foreach(Card card in myHand)
		{
			if (card.CanBeJolly && !card.CanBePin)
			{
				NumberOfJolly++;
			}
		}
	}
}
