using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Champions_Beer
{
    internal class Partita
    {
        Squadra sq1;
        Squadra sq2;
        string ris;
        DateTime gg;
        int hh;
        string stad;

        public Squadra squadra1 { get { return sq1; } set { sq1 = value; } }
        public Squadra squadra2 { get { return sq2; } set { sq2 = value; } }
        public string risultato { get { return ris; } set { ris = value; } }
        public DateTime giorno { get { return gg; } set { gg = value; } }
        public int ore { get { return hh; } set { hh = value; } }
        public string stadio { get { return stad; } set { stad = value; } }

        public Partita() { }
        public Partita(Squadra sq1, Squadra sq2, string ris, DateTime gg, int hh, string stad)
        {
            this.sq1 = sq1;
            this.sq2 = sq2;
            this.ris = ris;
            this.gg = gg;
            this.hh = hh;
            this.stad = stad;
        }

        public string vincitore()
        {
            string[] risultati = ris.Split('-');
            int[] results = new int[risultati.Length];
            results[0] = Convert.ToInt32(risultati[0]);
            results[1] = Convert.ToInt32(risultati[1]);
            if (results[0] > results[1])
                return sq1.ToString();
            else if (results[1] > results[0])
                return sq2.ToString();
            else
                return "pareggio";
        }

        public int goalsegnati(string a)
        {
            string[] risultati = ris.Split('-');
            if (sq1.ToString() == a)
                return (Convert.ToInt32(risultati[0]));
            return (Convert.ToInt32(risultati[1]));
        }
        public int goalsubiti(string a)
        {
            string[] risultati = ris.Split('-');
            if (sq1.ToString() == a)
                return (Convert.ToInt32(risultati[1]));
            return (Convert.ToInt32(risultati[0]));
        }

        public override string ToString()
        {
            string s = $"{squadra1} - {squadra2}";
            return s;
        }
    }
}
