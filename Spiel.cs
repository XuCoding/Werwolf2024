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
        //Spielerliste und zufällige Rollenverteilung.
        private List<Spieler> SpielerListe;
        private string[] Rollen = { "Werwolf", "Seher", "Dorfbewohner", "Dorfbewohner", "Dorfbewohner" };
        private Random random = new Random();

        public Spiel()
        {
            SpielerListe = new List<Spieler>();
            InitialisiereSpieler();
            VerteileRollen();
            ZeigeRollen();
        }

        private void InitialisiereSpieler() //zu beginn müsen alle 5 Spieler ihren Namen angeben.
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.Write($"Gib den Namen von dir ein Spieler {i} : ");
                string name = Console.ReadLine();
                SpielerListe.Add(new Spieler(name, i));
            }
        }

        private void VerteileRollen() //hier werden die Rollen nach der Namenseingabe veteilt.
        {
            var gemischteRollen = Rollen.OrderBy(r => random.Next()).ToArray();

            for (int i = 0; i < SpielerListe.Count; i++)
            {
                SpielerListe[i].SetzeRolle(gemischteRollen[i]);
            }
        }

        private void ZeigeRollen()
        {
            foreach (var spieler in SpielerListe) //hier bekommen die Spieler ihre Rolle mitgeteilt.
            {
                //mit diesem Befehl kann ich die UTF8 Unicode - Zeichen nutzen (der kursive Text, vorher gab er mir nur ?????? aus)
                Console.OutputEncoding = System.Text.Encoding.UTF8;                 
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("𝑒𝑡𝑤𝑎𝑠 𝑓𝑙ü𝑠𝑡𝑒𝑟𝑡 𝑑𝑖𝑟 𝑧𝑢 ..."); //dies soll ein flüstern visualisieren in der Konsole
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{spieler.Name}, deine Rolle ist: {spieler.Rolle}");
                Console.ResetColor();
            }
        }

        public void StarteSpiel() //wir wechseln zwischen den Phasen inklusive den Aktionen 
        {
            Console.WriteLine("Das Spiel beginnt!");

            while (SpielLaeuft())
            {
                NachtPhase();
                if (!SpielLaeuft()) break;
                TagPhase();
            }

            BeendeSpiel();
        }

        private void NachtPhase()
        {
            Console.WriteLine("Nachtphase beginnt...");
            WerwolfAktion();
            SeherAktion();
        }

        private void TagPhase()
        {
            Console.WriteLine("Tagphase beginnt...");
            SpielerAbstimmung();
        }

        private void WerwolfAktion() //hier wird die Aktion der Wölfe zur Nacht erstellt
        {
            var wölfe = SpielerListe.Where(s => s.Rolle == "Werwolf" && s.IstAmLeben).ToList();
            if (wölfe.Count == 0) return;

            Console.WriteLine("Werwölfe, wählt einen Spieler zum Eliminieren aus (Nummer):");
            foreach (var spieler in SpielerListe.Where(s => s.IstAmLeben))
            {
                Console.WriteLine($"Spieler {spieler.Nummer}: {spieler.Name}");
            }

            int gewaehlteNummer;
            do
            {
                Console.Write("Gib die Nummer ein die rausfliegen soll: ");
            } while (!int.TryParse(Console.ReadLine(), out gewaehlteNummer) || !SpielerListe.Any(s => s.Nummer == gewaehlteNummer && s.IstAmLeben));

            var getoeteterSpieler = SpielerListe.First(s => s.Nummer == gewaehlteNummer);
            getoeteterSpieler.IstAmLeben = false;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{getoeteterSpieler.Name} wurde in der Nacht eliminiert. Rolle: {getoeteterSpieler.Rolle}");
            Console.ResetColor();
        }

        private void SeherAktion() //hier sind die Funkionen des Seher zur Nacht 
        {
            var seher = SpielerListe.FirstOrDefault(s => s.Rolle == "Seher" && s.IstAmLeben);
            if (seher == null) return;

            Console.WriteLine("Seher, wähle einen Spieler zum Untersuchen aus (Nummer):");
            foreach (var spieler in SpielerListe.Where(s => s.IstAmLeben))
            {
                Console.WriteLine($"Spieler {spieler.Nummer}: {spieler.Name}");
            }

            int gewaehlteNummer;
            do
            {
                Console.Write("Welche Nummer möchtest du überprüfen? : ");
            } while (!int.TryParse(Console.ReadLine(), out gewaehlteNummer) || !SpielerListe.Any(s => s.Nummer == gewaehlteNummer && s.IstAmLeben));

            var untersuchterSpieler = SpielerListe.First(s => s.Nummer == gewaehlteNummer);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Der Spieler {untersuchterSpieler.Name} hat die Rolle: {untersuchterSpieler.Rolle}");
            Console.ResetColor();
        }

        private void SpielerAbstimmung() //Tagsüber wird gevotet wer gehängt werden soll
        {
            Console.WriteLine("Abstimmung: Wer soll gelyncht werden? (Nummer eingeben)");
            foreach (var spieler in SpielerListe.Where(s => s.IstAmLeben))
            {
                Console.WriteLine($"Spieler {spieler.Nummer}: {spieler.Name}");
            }

            int gewaehlteNummer;
            do
            {
                Console.Write("Votet für die Nummer die gelyncht werden soll: ");
            } while (!int.TryParse(Console.ReadLine(), out gewaehlteNummer) || !SpielerListe.Any(s => s.Nummer == gewaehlteNummer && s.IstAmLeben));

            var gelynchterSpieler = SpielerListe.First(s => s.Nummer == gewaehlteNummer);
            gelynchterSpieler.IstAmLeben = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{gelynchterSpieler.Name} wurde gelyncht. Rolle: {gelynchterSpieler.Rolle}");
            Console.ResetColor();
        }

        private bool SpielLaeuft() //hier wird überprüft ob die Spiebedingungen weiterhin gegeben sind und noch Wölfe bzw Dorfewohner vorhanden sind.
        {
            bool wölfeLeben = SpielerListe.Any(s => s.Rolle == "Werwolf" && s.IstAmLeben);
            bool dorfbewohnerLeben = SpielerListe.Any(s => s.Rolle != "Werwolf" && s.IstAmLeben);
            return wölfeLeben && dorfbewohnerLeben;
        }

        private void BeendeSpiel() //sollte das obige nicht der Fal sein, wird das Spiel zugnsten der Wölfe oder Dorfbewohner beendet. 
        {
            bool wölfeLeben = SpielerListe.Any(s => s.Rolle == "Werwolf" && s.IstAmLeben);
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (wölfeLeben)                       
                Console.WriteLine("Die Werwölfe haben gewonnen!");
            else          
                Console.WriteLine("Die Dorfbewohner haben gewonnen!");
                Console.ResetColor();



        }

    }






}
