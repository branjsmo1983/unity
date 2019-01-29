using UnityEngine;

public class Studente : Persona {
    private int mMatricola;

    public Studente (string Nome , string Cognome , int Matricola)
        : base (Nome , Cognome) {
        mMatricola = Matricola;
    }

    public override void StampaMessaggio () {
        base.StampaMessaggio ();
        Debug.Log ("Matricola: " + Matricola);
    }

    public int Matricola
    {
        get { return mMatricola; }
        set { mMatricola = value; }
    }
}

