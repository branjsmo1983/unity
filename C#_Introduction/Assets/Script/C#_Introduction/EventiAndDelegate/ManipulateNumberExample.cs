using System;
using UnityEngine;

public class ManipulateNumberExample : MonoBehaviour {

    void Start () {
        ManipulateNumber manipulateNumber = new ManipulateNumber ();
        PerformAdd addition = new PerformAdd ();
        PerformMul multiply = new PerformMul ();
        PerformSub sub = new PerformSub ();
        manipulateNumber.PerformManipulation += addition.OnPerformManipulation; //non sto chaimando il metodo, nessuna parentesi. è solo un puntatore della funzione.
        manipulateNumber.PerformManipulation += multiply.OnPerformManipulation; //come sopra
        manipulateNumber.PerformManipulation += sub.OnPerformManipulation;
        manipulateNumber.Manipulate (3 , 5);
    }

}

public class ManipulateNumber  {
    public delegate void ManipulateNumberEventHnadler (object source , ManipulateNumberEventArgs args);
    public event ManipulateNumberEventHnadler PerformManipulation;

    public void Manipulate (int a, int b) {
        OnPerformManipulation (a,b);
    }

    protected virtual void OnPerformManipulation (int a, int b) {
        if (PerformManipulation != null) {
            PerformManipulation (this , new ManipulateNumberEventArgs (a,b));
        }
    }
}

public class ManipulateNumberEventArgs : EventArgs {
    public int number1;
    public int number2;
    public ManipulateNumberEventArgs (int number1, int number2) {
        this.number1 = number1;
        this.number2 = number2;
    }
}


public class PerformAdd {
    public void OnPerformManipulation (object source, ManipulateNumberEventArgs e) {
        int temp = e.number1 + e.number2;
        Debug.Log (temp);
    }
}

public class PerformMul {
    public void OnPerformManipulation (object source, ManipulateNumberEventArgs e) {
        int temp = e.number1 * e.number2;
        Debug.Log (temp);
    }
}

public class PerformSub {
    public void OnPerformManipulation (object source, ManipulateNumberEventArgs e) {
        int temp = e.number1 - e.number2;
        Debug.Log (temp);
    }
}
