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
		bool result = false;

		if(cards.Exists(c => c.CanBeJolly) && card.CanBeJolly)					//check sul numero di pinelle
		{
			print(" nella canasta ho già un jolly ");
			return false;
		}
		if(card.CanBeJolly && !cards.Exists(c => c.CanBeJolly))
		{
			print(" nella canasta non ho un jolly e io sto attaccando un jolly");
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
		if(cards.Exists(c=>c.CurrentValue == card.CurrentValue && c.CanBeJolly))
		{
			print(" sto mettendo la carta dove c'era il jolly");
			return true;
		}

		return result;
	}

	internal bool AreAddables(ref List<Card> cardSelected)
	{
		bool result = false;

		foreach(Card card in cardSelected)
		{
			if (IsAddable(card))
			{
				result = true;
			}
			else
			{
				result = false;
				return result;
			}
		}

		return result;
	}


	internal static bool IsCanasta(ref List<Card> card)
	{
		bool check = false;

		//check sui giolly e pinelle
		int numberOfJolly = card.Count(c => (c.CanBeJolly) && (!c.CanBePin));
		print("numero di jolly : " + numberOfJolly);
		if (numberOfJolly > 1)										//se ho + di 1 Jolly NON va bene
		{
			print("numero di jolly maggiore di 1");
			return false;
		}

		int numberOfPin = card.Count(c => c.CanBePin);
		print("numero di pinelle : " + numberOfPin);
		if (numberOfPin > 2)										//se ho + di 2 pinelle NON va bene
		{
			print("numero di pinelle maggiore di 2");
			return false;
		}

		if (numberOfJolly > 1 && numberOfPin > 1)					//se ho + di 1 jolly e 1 pinella
		{
			print("più di un jolly e più di una pinella");
			return false;
		}

		//check sui tris
		if (card.Select(c => c.Value).Distinct().Count() == 1)      //tris pulito senza jolly
		{
			print("tris pulito");
			return true;
		}

		int myValues = card.FirstOrDefault(c => !c.CanBeJolly).CurrentValue;    //tris con jolly
		print(" il valore che ho trovato è : " + myValues);
		if (card.All(c => c.PossibleValues.Contains(myValues)))					//coi possible values 
		{
			if(card.Where(c=>c.CanBeJolly).Count() < 2)
			{
				print("tris con jolly");
				return true;
			}
			else
			{
				return false;
			}
			      
		}

		//check sulla scala
		Card.MySuits mySuits = card.FirstOrDefault(c => !c.CanBeJolly).Suit;
		//if (card.Where(c => c.Suit == mySuits).Count() == card.Count - 1)
		//{
		//};
		if ((card.All(c => (c.Suit == mySuits) || c.CanBeJolly)))               //controllo che siano tutte dello stesso seme (a meno che non sia un jolly)
		{
			print("sono tutte dello stesso seme (a meno che non ci sia un jolly)");
			//controllo che siano elementi distini (visto che voglio fare una scala

			if(card.Distinct(new CardEqualityComparer()).Count() != card.Count())
			{
				print("entro nel ramo in cui gli elementi NON sono distinti");
				return false;
			}

			//IEnumerable<Card> query = card.OrderByDescending(c => c, new Specialcomparer());

			//metto a posto in caso di pinella dello stesso seme
			if (card.Exists(c => c.CanBePin))
			{
				print("sono entrato nel metodo in cui controllo la pinella");
				CheckPinIs2(ref card);
			}
			
			if (card.Where(c => c.CanBePin).Count() > 1) return false;

			//metto a posto l'asso
			if(card.Exists(c => c.Value == Card.MyValues.A))
			{
				print("sono entrato nel metodo in cui controllo l'asso");
				CheckAcePosition(ref card);
			}
			List<Card> temp = card.OrderByDescending(c => c.CurrentValue).ToList();
			card = temp;

			bool jollyAlreadyUsed = false;              // check su quando uso il jolly (se c'è)
			bool jollyFounded = false;                  // controllo se c'è un jolly
			for (int index = 0; index < card.Count() - 1; index++)     // dati 'n' elementi devo fare 'n-1' controlli
			{
				

				print("entro nel ciclo for in cui controllo le cards");
				jollyFounded = (card[index].CanBeJolly);
				print(" sono su un jolly? : " + jollyFounded);
				if (jollyFounded)
				{
					print("ho trovato un jolly/pinella, eseguo il continue per skippare un index");
					continue;
				}
				// se ho più di una pinella allora una delle 2 deve avere la stessa suite delle altre carte
				if(card[index].CurrentValue - card[index + 1].CurrentValue == 1)
				{
					print("la differenza tra " +card[index].name + " e " + card[index + 1].name + " è : " + card[index].CurrentValue + " - " + card[index + 1].CurrentValue + " = " +(card[index].CurrentValue - card[index + 1].CurrentValue));
					check = true;
					continue;
				}else if(card[index].CurrentValue - card[index + 1].CurrentValue == 2 && !jollyAlreadyUsed)
				{
					print(" sono entrato nel ramo in cui uso il jolly ");
					card.First(c => c.CanBeJolly).CurrentValue = card[index].CurrentValue - 1;
					jollyAlreadyUsed = true;
					check = true;
					continue;
				}
				else
				{
					print("la differenza tra il valore corrente e il prossimo è maggiore di 1 oppure ho già usato il jolly");
					print("valore corrente = " + card[index].CurrentValue + " valore prossimo = " + card[index + 1].CurrentValue);
					return false;
				}
				
			}

		}


		return check;
	}

	internal static int GetTrisNumber(List<Card> cards)
	{
		int myValues = cards.FirstOrDefault(c => !c.CanBeJolly).CurrentValue;    //tris con jolly
		
		if (cards.All(c => c.PossibleValues.Contains(myValues)))
		{
			print(" è un tris di : " + myValues);
			return myValues;
		}
		else
		{
			print(" non è un tris");
			return -1;
		}
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
	

	private static void CheckPinIs2(ref List<Card> myCards)
	{
		Card.MySuits mySuits = myCards.FirstOrDefault(c => !c.CanBeJolly).Suit;			//seme della prima carta non jolly
		if(myCards.Exists(c => c.Value == Card.MyValues.due && c.Suit == mySuits))
		{
			Card pin = myCards.FirstOrDefault(c => c.Value == Card.MyValues.due && c.Suit == mySuits);
			if ((myCards.Exists(c => c.Value == Card.MyValues.A) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
				(myCards.Exists(c => c.Value == Card.MyValues.tre) && myCards.Exists(c => c.Value == Card.MyValues.quattro)) ||
				(myCards.Exists(c => c.Value == Card.MyValues.A) && myCards.Exists(c => c.CanBeJolly && c != pin)) ||
				(myCards.Exists(c => c.Value == Card.MyValues.tre) && myCards.Exists(c => c.CanBeJolly && c != pin)) ||
				(myCards.Exists(c => c.Value == Card.MyValues.quattro) && myCards.Exists(c => c.CanBeJolly && c != pin)))
			{
				pin.CurrentValue = 2;
				pin.CanBeJolly = false;
				pin.CanBePin = false;
			}
		}

	}

	private static void CheckAcePosition(ref List<Card> myCards)
	{
		Card ace = myCards.FirstOrDefault(c => c.Value == Card.MyValues.A);
		//Card.MySuits aceSuits = myCards.FirstOrDefault(c => c.Value == Card.MyValues.A).Suit;
		if ((myCards.Exists(c => c.Value == Card.MyValues.due) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
			(myCards.Exists(c => c.Value == Card.MyValues.due) && myCards.Exists(c => c.Value == Card.MyValues.jolly)) ||
			(myCards.Exists(c => c.Value == Card.MyValues.jolly) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
			(myCards.Where(c => c.Value == Card.MyValues.due).Count() == 2))
		{
			ace.CurrentValue = 1;
		}


	}


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
