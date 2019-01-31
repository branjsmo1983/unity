using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	
	private int valore, numero;

	
	private string seme, colore;

	
	private bool canBePin, canBeJolly, isVisible;

	public int Valore { get => valore; set => valore = value; }
	public int Numero { get => numero; set => numero = value; }
	public string Seme { get => seme; set => seme = value; }
	public string Colore { get => colore; set => colore = value; }
	public bool CanBePin { get => canBePin; set => canBePin = value; }
	public bool CanBeJolly { get => canBeJolly; set => canBeJolly = value; }
	public bool IsVisible { get => isVisible; set => isVisible = value; }



}
