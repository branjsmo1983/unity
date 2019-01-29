
using UnityEngine;

public class UtilizzoEreditariertà : MonoBehaviour {

    void Start () {
        Studente stud = new Studente ("Marco" , "" , 0);
        stud.Matricola = 232440;    //Corretto: Matricola è una property della classe Studente.
        stud.Cognome = "Minerva";   //Coretto: Cognome è una property ereditata dalla classe base Persona.
        Debug.Log ("Il nome dello studente è: " + stud.Nome + ", la sua matricola è: " + stud.Matricola);

        Persona pers = new Persona ("Marco" , "");
        pers.Cognome = "Minerva";   //Coretto: Cognome è una property della classe Persona.
        //pers.Matricola = 232440;	//Errato: la property Matricola non è visibile dalla classe Persona.
        Debug.Log ("Il cognome della persona è: " + pers.Cognome);
        stud = new Studente ("Marco" , "Minerva" , 232440);
        stud.StampaMessaggio ();
        pers = new Persona ("Donald" , "Duck");
        pers.StampaMessaggio ();
    }
}