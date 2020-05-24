using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L4
{
    /// <summary>
    /// Užsakymo klasė
    /// </summary>
    public class Uzsakymas
    {
        public string Pavadinimas { get; set; }
        public int Kiekis { get; set; }
        public Uzsakymas(string pav, int kiek)
        {
            Pavadinimas = pav;
            Kiekis = kiek;
        }

        public override string ToString()
        {
            string line = string.Format("{0, -20} {1, -20}", Pavadinimas, Kiekis);
            return line;
        }
    }
}