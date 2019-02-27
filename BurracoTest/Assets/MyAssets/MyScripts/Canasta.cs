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

	internal bool IsAddable(Card card)
	{
		bool result = false;

		// TO DO: logica per vedere se una carta è attaccabile


		return result;
	}

	internal bool Areaddable(List<Card> cards)
	{
		bool result = true;
		foreach (Card card in cards)
		{
			if (!IsAddable(card))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	internal bool IsCanasta()
	{
		bool check = false;

		//TO DO: controllare quando una lista di carte è una canasta

		return check;
	}

	internal static bool IsCanasta(ref List<Card> card)
	{
		bool check = false;

		//check sui giolly e pinelle
		int numberOfJolly = card.Count(c => (c.CanBeJolly) && (!c.CanBePin));
		print("numero di jolly : " + numberOfJolly);
		if (numberOfJolly > 1)
		{
			print("numero di jolly maggiore di 1");
			return false;
		}

		int numberOfPin = card.Count(c => c.CanBePin);
		print("numero di pinelle : " + numberOfPin);
		if (numberOfPin > 2)
		{
			print("numero di pinelle maggiore di 2");
			return false;
		}

		if (numberOfJolly > 1 && numberOfPin > 1)
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
		if (card.All(c => c.PossibleValues.Contains(myValues)))
		{
			print("tris con jolly");
			return true;       //il controllo lo faccio su PossibleValues e non sul current così becco anche i jolly o pinelle
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
			

			//metto a posto l'asso
			if(card.Exists(c => c.Value == Card.MyValues.A))
			{
				print("sono entrato nel metodo in cui controllo l'asso");
				CheckAcePosition(ref card);
			}

			IEnumerable<Card> query = card.OrderByDescending(c => c.CurrentValue);

			List<Card> myCards = query.ToList<Card>();



			bool jollyAlreadyUsed = false;              // check su quando uso il jolly (se c'è)
			bool jollyFounded = false;                  // controllo se c'è un jolly
			bool pinFounded = false;                    // controllo se c'è una pinella
			for (int index = 0; index < query.Count() - 1; index++)     // dati 'n' elementi devo fare 'n-1' controlli
			{
				// TO DO: implementare meglio il controllo sulla scala

				print("entro nel ciclo for in cui controllo le cards");
				jollyFounded = (card[index].CanBeJolly) && (!card[index].CanBePin);
				if (jollyFounded) continue;
				Card.MySuits mySuit1 = Card.MySuits.none;
				Card.MySuits mySuit2 = Card.MySuits.none;
				int pins = 0;
				pinFounded = card[index].CanBePin;
				if (pinFounded)
				{
					if (mySuit1 == Card.MySuits.none)
					{
						mySuit1 = card[index].Suit;
						pins++;
					}
					else
					{
						mySuit2 = card[index].Suit;
						pins++;
					}

					continue;
				}
				// se ho più di una pinella allora una delle 2 deve avere la stessa suite delle altre carte
				if (pins > 1)
				{
					print("ho più di una pinella");
					List<Card> myPins = myCards.FindAll(c => c.CanBePin);
					if (myPins.All(c => c.Suit != myCards[index].Suit))         //se le 2 suite sono diverse allora non è una combinazione valida
					{
						print("le 2 pinelle hanno semi diversi");
						return false;
					}
				}
				//if (card[index].CurrentValue == 14)               //faccio i controlli sull'asso
				//{
				//	print("se è un asso: ");
				//	if (myCards.Exists(x => ((x.Suit == myCards[index].Suit) && ((int)x.Value == 13)))
				//		&&
				//		myCards.Exists(x => ((x.Suit == myCards[index].Suit) && ((int)x.Value == 12))))
				//	{
				//		print("sono nel ramo in cui ho il K e il Q");
				//		continue;
				//	}
				//	else if(myCards.Exists(x => ((x.Suit == myCards[index].Suit) && ((int)x.Value == 12)))
				//		&&
				//		myCards.Exists(x => x.CanBeJolly))
				//	{
				//		print("sono nel ramo in cui ho il Q e un jolly");
				//		jollyAlreadyUsed = true;
				//		continue;
				//	}
				//	else if (myCards.Exists(x => ((x.Suit == myCards[index].Suit) && ((int)x.Value == 2)))
				//			&&
				//			myCards.Exists(x => ((x.Suit == myCards[index].Suit) && ((int)x.Value == 3))))
				//	{
				//		print("sono nel ramo in cui ho il 2 e il 3");
				//		card[index].CurrentValue = 1;
				//		card = myCards;
				//		return IsCanasta(ref card);
				//	}
				//}
				//else
				//{
					if(card[index].CurrentValue - card[index + 1].CurrentValue == 1)
					{
						check = true;
						continue;
					}else if(card[index].CurrentValue - card[index + 1].CurrentValue == 2 && !jollyAlreadyUsed)
					{
						jollyAlreadyUsed = true;
						check = true;
						continue;
					}
					else
					{
						return false;
					}
				//}
			}
		}


		return check;
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
			(myCards.Exists(c => c.Value == Card.MyValues.jolly) && myCards.Exists(c => c.Value == Card.MyValues.tre))||
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
