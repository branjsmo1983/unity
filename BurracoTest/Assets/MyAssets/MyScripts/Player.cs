using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	internal List<Card> myHand;

	internal List<Card> cardsSelected = new List<Card>();                                   // lista delle carte nella mia mano che ho selezionato

	public int NumberOfPins { get; set; }						//numero di pinelle
	public int NumberOfJolly { get; set; }						//numero di jolly nella mia mano
	internal bool IsMyRound { get; set; }						//se è il mio turno
	internal bool CockpitAlreadyBeenTaken { get; set; }			//se ha preso il pozzetto
	public bool HasFished { get; set; }							//se ha pescato
	public bool HasCollected { get; set; }						//se ha raccolto
	public bool HasDiscarded { get; set; }						//se ha scartato
	internal bool IsWinner { get; set; }						//se ha vinto
	internal string Name { get; set; }							//nome ( non so se mi serve )

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
