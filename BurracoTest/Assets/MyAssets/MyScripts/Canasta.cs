using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Canasta : MonoBehaviour
{

	internal List<Card> cards;

	public bool IsBurraco { get; set; }
	public bool IsBurracoClean { get; set; }
	public bool IsBurracoHalfClean { get; set; }
	public bool IsFull { get; set; }
	public bool IsTris { get; set; }
	public int TrisValue { get; set; }



	internal bool IsAddable(Card card)
	{
		print("  °°°° entro nel metodo che controlla se una carta è aggiungibile °°°°");
		if(cards.Exists(c => c.CanBeJolly) && card.CanBeJolly)					//check sul numero di pinelle
		{
			print(" nella canasta ho già un jolly ");
			return false;
		}
		if(card.CanBeJolly && !cards.Exists(c => c.CanBeJolly))
		{
			print(" nella canasta non ho un jolly e io sto attaccando un jolly");
			cards.Add(card);
			List<Card> temp = cards.OrderByDescending(c => c.CurrentValue).ToList();
			return true;
		}
		if((card.CurrentValue != TrisValue) && (TrisValue != -1))				//check sul tris
		{
			print(" il valore della carta " + card.CurrentValue);
			print(" è diverso da quello del tris :" + TrisValue);
			return false;
		}
		Card.MySuits suit = cards.FirstOrDefault(c => !c.CanBeJolly).Suit;		//check seme della scala
		if((card.Suit != suit) && !card.CanBePin && TrisValue == -1)
		{
			print(" il seme della carta è : " + card.Suit);
			print(" è diverso da quello della scala che è : " + suit);
			return false;
		}
		if(card.Value == Card.MyValues.A && cards.Exists(c=>c.Value == Card.MyValues.A) && TrisValue == -1)
		{
			print(" sto tentando di aggiungere un Asso in  una scala che ha già un asso");
			return false;
		}
		if(cards.Exists(c=>c.CurrentValue == card.CurrentValue && c.CanBeJolly ))
		{
			print(" sto mettendo la carta dove c'era");
			if(cards.Find(c=>c.CanBeJolly).Value == Card.MyValues.jolly)
			{
				print(" il jolly ");
				cards.Find(c => c.CanBeJolly).CurrentValue = 16;
				cards.Add(card);
				List<Card> temp = cards.OrderByDescending(c => c.CurrentValue).ToList();
				cards = temp;
			}
			else if (cards.Find(c => c.CanBeJolly).Value == Card.MyValues.due)
			{
				print(" la pinella ");
				cards.Find(c => c.CanBeJolly).CurrentValue = 15;
				cards.Add(card);
				List<Card> temp = cards.OrderByDescending(c => c.CurrentValue).ToList();
				cards = temp;
			}
			return true;
		}
		if (cards.Exists(c => c.CurrentValue == card.CurrentValue && !c.CanBeJolly))
		{
			print(" sto cercando di mettere una carta già presente");
			return false;
		}
		if(cards.Exists(c => c.CurrentValue == (card.CurrentValue + 1))) 
		{
			print("sto inserendo un elemento minore");
			int rightIndex = cards.FindIndex(c => c.CurrentValue == card.CurrentValue + 1);		//in teoria in fondo alla lista
			print(" all' indice " + (int)(rightIndex +1));
			cards.Insert(rightIndex + 1, card);
			return true;
		}
		else if(cards.Exists(c => c.CurrentValue == (card.CurrentValue - 1)))
		{
			print(" sto inserendo un elemento maggiore");
			//int rightIndex = cards.FindIndex(c => c.CurrentValue == card.CurrentValue + 1);         //probabilmente sarà 0 o 1
			//print(" all' indice " + rightIndex);
			//cards.Insert(rightIndex, card);
			cards.Insert(0, card);
			return true;
		}
		else if (cards.Exists(c => c.CurrentValue == (card.CurrentValue + 2)) && cards.Exists(c => c.CanBeJolly))
		{
			Card jolly = cards.Find(c => c.CanBeJolly);
			jolly.CurrentValue = card.CurrentValue + 1;
			cards.Remove(jolly);
			cards.Insert(cards.Count - 1, jolly);	//metto in ultima posizione il jolly
			cards.Insert(cards.Count - 1, card);    //metto in ultima posizione la mia carta
			return true;
		}
		else if (cards.Exists(c => c.CurrentValue == (card.CurrentValue - 2)) && cards.Exists(c => c.CanBeJolly))
		{
			Card jolly = cards.Find(c => c.CanBeJolly);
			jolly.CurrentValue = card.CurrentValue - 1;
			cards.Remove(jolly);
			cards.Insert(0, jolly);				//metto in prima posizione il jolly
			cards.Insert(0, card);              //metto in prima posizione la mia carta
			return true;
		}
		else
		{
			print(" se sono arrivato qui ho dimenticato qualcosa");
			return false;
		}


	}

	internal bool AreAddables(ref List<Card> cardSelected)
	{
		bool result = false;

		foreach(Card card in cardSelected)
		{
			if (IsAddable(card))
			{
				print(" la carta : " + card.name + " è aggiungibile");
				result = true;
			}
			else
			{
				print(" la carta : " + card.name + " NON è aggiungibile!!!");
				result = false;
				return result;
			}
		}

		return result;
	}


	



	internal void GetTrisNumber()
	{
		int myValues = cards.FirstOrDefault(c => !c.CanBeJolly).CurrentValue;    //tris con jolly
		
		if (cards.All(c => c.PossibleValues.Contains(myValues)))
		{
			print(" è un tris di : " + myValues);
			TrisValue = myValues;
		}
		else
		{
			print(" non è un tris");
			TrisValue =  -1;
		}
	}
	

	//private static void CheckPinIs2(ref List<Card> myCards)
	//{
	//	print(" sono entrato nell'evento in cui controllo i 2");
	//	Card.MySuits mySuits = myCards.FirstOrDefault(c => !c.CanBeJolly).Suit;         //seme della prima carta non jolly
	//	int numOfPin = myCards.Count(c => c.CanBeJolly);
	//	print(""+mySuits);
	//	if(myCards.Exists(c => c.Value == Card.MyValues.due && c.Suit == mySuits))
	//	{
	//		Card pin = myCards.FirstOrDefault(c => c.Value == Card.MyValues.due && c.Suit == mySuits);
	//		if ((myCards.Exists(c => c.Value == Card.MyValues.A) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
	//			(myCards.Exists(c => c.Value == Card.MyValues.tre) && myCards.Exists(c => c.Value == Card.MyValues.quattro) && numOfPin == 2) ||
	//			(myCards.Exists(c => c.Value == Card.MyValues.A) && numOfPin == 2 )||
	//			(myCards.Exists(c => c.Value == Card.MyValues.tre) && numOfPin == 2) ||
	//			(myCards.Exists(c => c.Value == Card.MyValues.quattro) && numOfPin == 2))
	//		{
	//			pin.CurrentValue = 2;
	//			pin.CanBeJolly = false;
	//			pin.CanBePin = false;
	//		}else if(myCards.Exists(c => c.Value == Card.MyValues.tre) && myCards.Exists(c => c.Value == Card.MyValues.quattro))
	//		{
	//			pin.CurrentValue = 2;
	//			pin.CanBeJolly = true;
	//			pin.CanBePin = true;

	//		}
	//	}

	//}


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}


	public class Specialcomparer : IComparer<Card>
	{
		public int Compare(Card x, Card y)
		{
			print("Sto comparando :" + x.Name);
			print("");
			if (x.PossibleValues.Max() > y.PossibleValues.Max())
			{
				return 1;
			}
			else return -1;
		}
	}

	public class CardEqualityComparer : IEqualityComparer<Card>				//compara solo il valore
	{
		public bool Equals(Card x, Card y)
		{
			//return x.Value == y.Value;
			if((x.Value != y.Value && x.Suit != y.Suit )|| (x.Value == Card.MyValues.jolly && y.Value == Card.MyValues.jolly)|| (x.Value == Card.MyValues.due && y.Value == Card.MyValues.due))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public int GetHashCode(Card obj)
		{
			return obj.CurrentValue;
		}
	}

	public class CardValueComparer : IEqualityComparer<Card>			//compara anche il seme
	{
		public bool Equals(Card x, Card y)
		{
			//return x.Value == y.Value;
			if (x.Value != y.Value || (x.Value == Card.MyValues.jolly && y.Value == Card.MyValues.jolly) || (x.Value == Card.MyValues.due && y.Value == Card.MyValues.due))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public int GetHashCode(Card obj)
		{
			return obj.CurrentValue;
		}
	}

}
