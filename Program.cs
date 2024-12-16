namespace WerwolfFIUS2024
{
    

    class Program
    {
        static void Main(string[] args)
        {
            //Hier ist die Einleitung in das Spiel.
            Console.WriteLine("Das Spiel wird mit genau 5 Spielern gestartet.");
            var spiel = new Spiel();
            spiel.StarteSpiel();

        }


    }
}