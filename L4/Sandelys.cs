using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L4
{
    /// <summary>
    /// Sandėlio klasė
    /// </summary>
    public class Sandelys : IEquatable<Sandelys>, IComparable<Sandelys>
    {
        public int Numeris { get; set; }
        public string Vardas { get; set; }
        public int Kiekis { get; set; }
        public double Kaina { get; set; }

        public Sandelys() { }

        public Sandelys(int nr, string vard, int kiek, double kain)
        {
            Numeris = nr;
            Vardas = vard;
            Kiekis = kiek;
            Kaina = kain;
        }
        public override int GetHashCode()
        {
            return Vardas.GetHashCode() ^ Kiekis.GetHashCode();
        }
        public bool Equals(Sandelys other)
        {
            if (other == null) return true;
            return Vardas.Equals(other.Vardas) && Kiekis.Equals(other.Kiekis);
        }

        public int CompareTo(Sandelys other)
        {
            if (other == null) return 1;
            if (Vardas.CompareTo(other.Vardas) != 0) return Vardas.CompareTo(other.Vardas);
            else return Kiekis.CompareTo(other.Kiekis);
        }

        public override string ToString()
        {
            string line = string.Format("{0, -20} {1, -20} {2, -20} {3, -20}", Numeris, Vardas, Kiekis, Kaina);
            return line;
        }
    }
}