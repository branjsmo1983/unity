using System.Collections.Generic;
using UnityEngine;

public class VariablesAndStructure : MonoBehaviour
{

    public int intero; //Un numero intero. Range da -2.147.483.648 a + 2.147.483.648
    public float singolaPrecisione; //Numero in virgola mobile singola precisione 4 bytes
    public double doppiaPrecisione; //Numero in virgola mobile doppia precisione 8 bytes
    public char carattere; //Singolo carattere
    public string stringa; //Stringa (Array di caratteri).
    public int[] arrayInteri = new int[25]; //Array di interi, inizializzato durante la dichiarazione. è formato da 25 elementi.
    public float[] arraySingolaPrecisione;


    //PER IL MOMENTO PENSATE CHE TUTTO QUELLO CHE VIENE SCRITTO QUI DENTRO VIENE ESEGUITO QUANDO SCHIACCIATE PLAY. E' COME SE FOSSE IL MAIN IN UN PROGETTO C# VERO E PROPRIO
    void Start () {
        /*
        //Debug.Log scrive sulla console di Unity.
        //Inizializzazione di default di C#
        Debug.Log (intero); 
        Debug.Log (singolaPrecisione);
        Debug.Log (doppiaPrecisione);
        Debug.Log (carattere);
        Debug.Log (stringa);
        
        //Inizializzare delle variabili
        intero = 4;
        singolaPrecisione = 0.4f; //f va aggiunto per distinguere un float da un double;
        doppiaPrecisione = 3.4;
        carattere = 'a'; //singolo apice per carattere, doppio apice per stringa
        stringa = "può avere una dimensione qualsiasi";
        Debug.Log (intero);
        Debug.Log (singolaPrecisione);
        Debug.Log (doppiaPrecisione);
        Debug.Log (carattere);
        Debug.Log (stringa);
        
        Debug.Log (arraySingolaPrecisione);
        //Inizializzare Array
        arraySingolaPrecisione = new float[4];
        //Accedere ad un elemento dell'array
        Debug.Log (arraySingolaPrecisione[1]);
        //Modifica elemento di un array
        arraySingolaPrecisione[1] = 13.5f;
        Debug.Log (arraySingolaPrecisione[1]);
        Debug.Log (arraySingolaPrecisione.Length); //Proprietà che indica la dimensione dell'array
        */
        
        //Liste
        List<int> listaInteri; //Dichiarazione
        listaInteri = new List<int> (); //Inizializzazione
        listaInteri.Add (5); //Aggiungere un elemento
        Debug.Log (listaInteri.Count); //Proprietà che indica la dimensione dell'array
        List<int> secondaListaInteri = new List<int> ();
        secondaListaInteri.Add (4);
        secondaListaInteri.Add (3);
        listaInteri.AddRange (secondaListaInteri);
        Debug.Log (listaInteri.Count);
    }

}
