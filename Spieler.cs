using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WerwolfFIUS2024
{
    internal class Spieler
    {
        public string Name { get; set; }
        public string Rolle { get; private set; }
        public int Nummer { get; private set; }
        public bool IstAmLeben { get; set; }

        public Spieler(string name, int nummer)
        {
            Name = name;
            Nummer = nummer;
            IstAmLeben = true;
        }

        public void SetzeRolle(string rolle)
        {
            Rolle = rolle;
        }

    }


}
    

