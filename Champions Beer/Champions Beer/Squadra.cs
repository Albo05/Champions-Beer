using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Champions_Beer
{
    internal class Squadra
    {
        string name;
        char gir;
        int pt;

        public string nome { get { return name; } set { name = value; } }
        public char girone { get { return gir; } set { gir = value; } }
        public int points { get { return pt; } set { pt = value; } }

        public Squadra(string name, char gir)
        {
            this.name = name;
            this.gir = gir;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
