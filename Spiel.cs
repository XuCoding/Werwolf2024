using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WerwolfFIUS2024
{
    public class Spiel
    {
        private List<Spieler> SpielerListe; 
        private bool NachtPhase;


        public Spiel(List<Spieler> spielerListe)
            //Konstruktor um die Spieler des Spiels zu initialisieren.
        {
            SpielerListe = spielerListe; 
            NachtPhase = true; //Das Spiel soll in der Nachtphase beginnen.

        }

        public void Start()
        {
            //Diese while Schleife soll die Nacht und Tagphasen wechseln bis eine Partei gewonnen hat.
            Console.WriteLine("Das Spiel beginnt!");
            while (!IstSpielBeendet())
            {
                Console.WriteLine(NachtPhase ? "Nachtphase beginnt..." : "Tagphase beginnt...");
                if (NachtPhase) NachtAktionen(); else TagAktionen();
                NachtPhase = !NachtPhase; //Phasenwechsel
            }
            Console.WriteLine($"Spiel beendet! {(WerwolfGewonnen() ? "Die Werwölfe" : "Die Dorfbewohner")} haben gewonnen!");
        }

        private void NachtAktionen()
        {
            WerwolfAktion(); //hiermit können die Wölfe ein Ziel für die Nacht auswählen.
            SeherAktion(); //Hiermit kann der Seher Nachts einen Spieler untersuchen.
        }

        private void WerwolfAktion()
            //Hier ist der Code für die Aktion Nachts für die Wölfe , zuerst checken wir 
            //wie viele Wölfe lebendig sind . Diese töten ein Opfer in der Nacht.
            //Am Ende wird angezeigt welhces Opfer getötet wurde.
      
        {
            var werwoelfe = LebendigeSpieler(Rolle.Werwolf);
            if (werwoelfe.Any())
            {
                var opfer = SpielerWaehlen(LebendigeSpieler());
                opfer.Lebendig = false;
                Console.WriteLine($"Die Werwölfe haben {opfer.Name} getötet.");
            }
        }

        private void SeherAktion()

            //Wir lassen den Seher in der Nacht die Rolle eines anderen Spielers checken.
            //zuerst checken wir auch hier ob noch ein Seher vorhanden ist
            //& wenn ja darf er einen anderen Spieler untersuchen.
        {
            var seher = LebendigeSpieler(Rolle.Seher).FirstOrDefault();
            if (seher != null)
            {
                var untersuchung = SpielerWaehlen(LebendigeSpieler());
                Console.WriteLine($"Der Seher untersucht {untersuchung.Name}. Rolle: {untersuchung.SpielerRolle}");
            }
        }

        private void TagAktionen()
            //In dieser Phase stimmen die Spieler ab,
            //wer ihnen verdächtig vorkommt oder wer entdeckt wurde und somit wird dieser Spieler gelyncht.
        {
            var verdacht = SpielerWaehlen(LebendigeSpieler());
            verdacht.Lebendig = false;
            Console.WriteLine($"{verdacht.Name} wurde gelyncht.");
        }

        private Spieler SpielerWaehlen(List<Spieler> spieler)
            //Hier wird die Liste der Spieler die gewählt werden können ausgegeben.
        {
            return spieler[new Random().Next(spieler.Count)];
        }

        private List<Spieler> LebendigeSpieler(Rolle? rolle = null)
            //Hier soll die Liste der noch lebendigen Spieler zurückgegeben werden.
        {
            return SpielerListe.Where(s => s.Lebendig && (rolle == null || s.SpielerRolle == rolle)).ToList();
        }

        private bool IstSpielBeendet()
            //Das Spiel endet wenn entweder alle Wölfe gelyncht wurden oder die Wöfle dominieren.
        {
            var lebendigeWerwoelfe = LebendigeSpieler(Rolle.Werwolf).Count;
            var lebendigeSpieler = LebendigeSpieler().Count;
            //die beiden var's rufen die Anzahl der lebendigen Wölfe & Spieler ab.
            return lebendigeWerwoelfe == 0 || lebendigeWerwoelfe >= lebendigeSpieler - lebendigeWerwoelfe;
            //Spiel endet wenn: keine Wölfe mehr da sind, die Wölfe gleich viel oder mehr als die Dorfbewohner sind.
        }

        private bool WerwolfGewonnen()
            //Hier prüfe ich ob die Wölfe gewonnen haben. Ob noch Wölfe leben.
        {
            return LebendigeSpieler(Rolle.Werwolf).Any();
        }
    }






}
