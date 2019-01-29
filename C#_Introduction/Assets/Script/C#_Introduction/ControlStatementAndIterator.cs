using System.Collections.Generic;
using UnityEngine;

public class ControlStatementAndIterator : MonoBehaviour {


    void Start () {
        int[] testArray = new int[25];
        //Ciclo for
        for (int i = 0; i < testArray.Length; i++) {
            testArray[i] = testArray.Length - i;
        }
        //Ciclo foreach
        foreach (int elemento in testArray) {
            Debug.Log (elemento);
        }
        //Ciclo While
        int counter = 0;
        while (counter < testArray.Length) {
            Debug.Log (testArray.Length);
            counter++;
        }

        //Condizionale
        if (testArray.Length < testArray[20]) {
            Debug.Log ("La condizione è vera");
        } else {
            Debug.Log ("La condizione è falsa");
        }
        //Operatore ternario
        Debug.Log (testArray.Length < testArray[20] ? "La condizione è vera" : "La condizione è falsa");

        //Più condizioni
        if (testArray[3] == 3) {
            Debug.Log ("è uguale a tre");
        } else if (testArray[3] == 4) {
            Debug.Log ("è uguale a 4");
        } else {
            Debug.Log ("non è uguale a tre e non è uguale a 4");
        }

        //Switch case
        switch (testArray[3]) {
            case 3:
                Debug.Log ("Uguale a 3");
                break;
            case 4:
                Debug.Log ("Uguale a 4");
                break;
            default:
                Debug.Log ("Non è uguale ne a 3 ne a 4");
                break;
        }
    }


}
