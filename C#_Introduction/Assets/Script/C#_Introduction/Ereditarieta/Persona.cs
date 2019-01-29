using UnityEngine;

public class Persona {
    string mNome;
    protected string mCognome;


    public Persona (string Nome , string Cognome) {
        //Imposta le proprietà iniziali della classe.
        if (Nome == string.Empty) {
            mNome = "(Nessun nome)";
        } else {
            mNome = Nome;
        }
        if (Cognome == string.Empty) {
            mCognome = "(Nessun cognone)";
        } else {
            mCognome = Cognome;
        }
    }

    //Overloading lo vediamo dopo
    public Persona () {
        mNome = "";
        mCognome = "";
    }

    public virtual void StampaMessaggio () {
        Debug.Log ("Nome: " + Nome + " " +Cognome);
    }

    private void Appoggio () { }

    public string Nome
    {
        get { return mNome; }
        set
        {
            if (value == string.Empty)
                mNome = "(Nessun nome)";
            else
                mNome = value + "(ciao)";
        }
    }

    public string Cognome
    {
        get { return mCognome; }
        set
        {
            if (value == string.Empty)
                mCognome = "(Nessun cognome)";
            else
                mCognome = value;
        }
    }
}
