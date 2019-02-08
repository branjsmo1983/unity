using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckForStartGame : MonoBehaviour
{
	[SerializeField]
	internal CardForStartGame[] deckForStart = new CardForStartGame[52];

	[SerializeField]
	internal List<CardForStartGame> deck;


	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
