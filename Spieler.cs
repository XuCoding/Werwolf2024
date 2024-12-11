using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerwolfFIUS2024
{

    public enum Rolle
    //Hier definieren wir die Rollen die es geben soll
    //(Enum ist für die Aufzählung der Werte)
    {
        Dorfbewohner,
        Seher,
        Werwolf
    }
    public class Spieler

    {
        //Hier erstelle ich die Eigenschaften für die Spieler
        public string Name { get; private set; }
        public Rolle SpielerRolle { get; private set; }
        public bool Lebendig { get; set; }


        public Spieler(string name, Rolle rolle)
            //Konstruktor fürs erstellen eines neuen Spielers.
        {
            Name = name;
            Lebendig = true;
            SpielerRolle = rolle;
        }
        public override string ToString()
            //Darstellung des Spielers mit der überschreibung des Strings.
        {
            // dieser code soll mir den namen und die Rolle des Spielers als String zurückgeben.
            return $"{Name} ({SpielerRolle})";
        }


    }
}
