using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using System.Threading;

namespace Champions_Beer
{
    public partial class Form1 : Form
    {
        List<Partita> match = new List<Partita>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            int d = 50;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            pictureBox1.Region = new Region(gp);

            //leggiECaricaFile();
            partite();
        }

        public void leggiECaricaFile()
        {
            string[] righe = File.ReadAllLines("P1.txt");
            string[,] matrice = new string[righe.Length, 7];
            for (int i = 0; i < matrice.GetLength(1); i++)
            {
                string[] appo = righe[i].Split(';');
                for (int j = 0; j < 7; j++)
                {
                    matrice[i,j] = appo[j];
                }
            }
            string[,] appo2 = new string[2 , righe.Length];
            for (int i = 0; i < appo2.GetLength(1); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    appo2[j,i] = matrice[i, j];
                }
            }
            string[] appo3 = squadre(appo2);
            for (int i = 0; i < appo3.Length; i++)
            {
                comboBox3.Items.Add(appo3[i]);
            }
        }

        public String[] squadre(string[,] doppie)
        {
            List<string> returnable = new List<string>();
            for (int i = 0; i < doppie.GetLength(0); i++)
            {
                for (int j = 0; j < doppie.GetLength(1); j++)
                {
                    bool presente = false;
                    for (int k = 0; k < returnable.Count; k++)
                    {
                        if (returnable[k] == doppie[i, j] || doppie[i,j] == null)
                        {
                            presente = true;
                        }
                    }
                    if (!presente)
                    {
                        returnable.Add(doppie[i, j]);
                    }
                }
            }
            string[] daReturn = new string[returnable.Count];
            for (int i = 0; i < daReturn.Length; i++)
            {
                daReturn[i] = returnable[i];
            }
            return daReturn;
        }

        public void partite()
        {
            string[] righe = File.ReadAllLines("P1.txt");
            string[,] matrice = new string[righe.Length, 7];
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                string[] appo = righe[i].Split(';');
                for (int j = 0; j < 7; j++)
                {
                    matrice[i, j] = appo[j];
                }
            }
            for (int i = 0; i < righe.Length; i++)
            {
                Partita a = new Partita();
                Squadra appo = new Squadra(matrice[i,0], matrice[i, 3][0]);
                a.squadra1 = appo;
                appo = new Squadra(matrice[i, 1], matrice[i, 3][0]);
                a.squadra2 = appo;
                a.risultato = matrice[i, 2];
                a.giorno = Convert.ToDateTime(matrice[i, 5]);
                a.ore = Convert.ToInt32(matrice[i,6]);
                a.stadio = matrice[i, 4];
                match.Add(a);
            }
            string[] indicePartite = new string[match.Count];
            int c = match.Count;
            for (int i = 0; i < c - 1; i++)
            {
                indicePartite[i] = $"{match[i].squadra1.ToString()}-{match[i].squadra2.ToString()}";
                comboBox3.Items.Add(indicePartite[i]);
            }
        }

        public void caricaPartite()
        {
            int index = comboBox3.SelectedIndex;
            textBox1.Text = match[index].squadra1.ToString();
            textBox1.Invalidate();
            textBox2.Text = match[index].squadra2.ToString();
            textBox2.Invalidate();
            string[] appo = match[index].risultato.Split('-');
            textBox3.Text = appo[0];
            textBox3.Invalidate();
            textBox4.Text = appo[1];
            textBox4.Invalidate();
            textBox5.Text = match[index].stadio;
            textBox5.Invalidate();
            textBox6.Text = Convert.ToString(match[index].ore);
            textBox6.Invalidate();
            textBox7.Text = Convert.ToString(match[index].giorno.ToShortDateString());
            textBox7.Invalidate();
        }
        public void giornate()
        {
            DateTime giorno = dateTimePicker1.Value;
            string partite = "";
            for (int i = 0; i < match.Count; i++)
            {
                if (match[i].giorno.ToShortDateString() == giorno.ToShortDateString())
                {
                    partite += $"{match[i].ToString()}\n";
                }
            }
            if (partite.Length == 0)
                label12.Text = "Non sono state giocate partite in questa data";
            else
                label12.Text = partite;
        }

        public void gironi()
        {
            char gir = comboBox1.Text[0];
            List <string> appo = new List <string>();
            for (int i = 0; i < match.Count; i++)
            {
                if (match[i].squadra1.girone == gir)
                {
                    appo.Add(match[i].squadra1.ToString());
                    appo.Add(match[i].squadra2.ToString());
                }
            }
            string[] doppie = appo.ToArray();
            appo.RemoveRange(0, doppie.Length);
            string appo2 = "Squadra, Punti, Goal fatti, Goal subiti\n";
            for (int i = 0; i < doppie.Length; i++)
            {
                bool giaPresente = false;
                for (int j = 0; j < appo.Count; j++)
                    if (doppie[i] == appo[j])
                        giaPresente = true;
                if (!giaPresente)
                    appo.Add(doppie[i]);
            }
            int[] risultati = new int[appo.Count];
            for (int i = 0; i < match.Count; i++)
            {
                string win = match[i].vincitore();
                int h = 0;
                for (int j = 0; j < appo.Count; j++)
                {
                    if (match[i].squadra1.ToString() == appo[j] || match[i].squadra2.ToString() == appo[j])
                    {
                        if (win == appo[j])
                            risultati[j] += 3;
                        else if (win == "pareggio")
                            risultati[j]++;
                    }
                }
            }
            int goal1 = 0;
            int subiti1 = 0;
            for (int i = 0; i < appo.Count; i++)
            {
                for (int j = 0; j < appo.Count; j++)
                {
                    if (risultati[i] > risultati[j])
                    {
                        int appoRis = risultati[i];
                        risultati[i] = risultati[j];
                        risultati[j] = appoRis;
                        string appoSqd = appo[i];
                        appo[i] = appo[j];
                        appo[j] = appoSqd;
                    }
                    else if (risultati[i] == risultati[j] && scambia(appo[i], appo[j]))
                    {
                        int appoRis = risultati[i];
                        risultati[i] = risultati[j];
                        risultati[j] = appoRis;
                        string appoSqd = appo[i];
                        appo[i] = appo[j];
                        appo[j] = appoSqd;
                    }
                }
            }
            for (int i = 0; i < appo.Count; i++)
            {
                for (int j = 0; j < match.Count; j++)
                {
                    if (match[j].squadra1.ToString() == appo[i] || match[j].squadra2.ToString() == appo[i])
                    {
                        goal1 += match[j].goalsegnati(appo[i]);
                        subiti1 += match[j].goalsubiti(appo[i]);
                    }
                }
                appo2 += $"{appo[i]}: {risultati[i]}, {goal1}, {subiti1}\n";
                goal1 = 0;
                subiti1 = 0;
            }
            label13.Text = appo2;
        }

        public void classifica()
        {
            //classifica nuova
            Label[] lbl = { label30, label31, label32, label33, label34, label35, label36, label37 };
            char[] gironi = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            foreach (char gir in gironi)
            {
                List<string> appo = new List<string>();
                for (int i = 0; i < match.Count; i++)
                {
                    if (match[i].squadra1.girone == gir)
                    {
                        appo.Add(match[i].squadra1.ToString());
                        appo.Add(match[i].squadra2.ToString());
                    }
                }
                string[] doppie = appo.ToArray();
                appo.RemoveRange(0, doppie.Length);
                string appo2 = "";
                for (int i = 0; i < doppie.Length; i++)
                {
                    bool giaPresente = false;
                    for (int j = 0; j < appo.Count; j++)
                        if (doppie[i] == appo[j])
                            giaPresente = true;
                    if (!giaPresente)
                        appo.Add(doppie[i]);
                }
                int[] risultati = new int[appo.Count];
                for (int i = 0; i < match.Count; i++)
                {
                    string win = match[i].vincitore();
                    int h = 0;
                    for (int j = 0; j < appo.Count; j++)
                    {
                        if (match[i].squadra1.ToString() == appo[j] || match[i].squadra2.ToString() == appo[j])
                        {
                            if (win == appo[j])
                                risultati[j] += 3;
                            else if (win == "pareggio")
                                risultati[j]++;
                        }
                    }
                }
                for (int i = 0; i < appo.Count; i++)
                {
                    for (int j = 0; j < appo.Count; j++)
                    {
                        if (risultati[i] > risultati[j])
                        {
                            int appoRis = risultati[i];
                            risultati[i] = risultati[j];
                            risultati[j] = appoRis;
                            string appoSqd = appo[i];
                            appo[i] = appo[j];
                            appo[j] = appoSqd;
                        }
                        else if (risultati[i] == risultati[j] && scambia(appo[i], appo[j]))
                        {
                            int appoRis = risultati[i];
                            risultati[i] = risultati[j];
                            risultati[j] = appoRis;
                            string appoSqd = appo[i];
                            appo[i] = appo[j];
                            appo[j] = appoSqd;
                        }
                    }
                }
                for (int i = 0; i < appo.Count; i++)
                {
                    appo2 += $"{appo[i]}: {risultati[i]}\n";
                }
                lbl[gir - 65].Text = appo2;
            }
        }

        public bool scambia(string sq1, string sq2)
        {
            int goal1 = 0;
            int goal2 = 0;
            int subiti1 = 0;
            int subiti2 = 0;
            for (int i = 0; i < match.Count; i++)
            {
                if (match[i].squadra1.nome == sq1 || match[i].squadra2.nome == sq1)
                {
                    goal1 += match[i].goalsegnati(sq1);
                    subiti1 += match[i].goalsubiti(sq1);
                }
                else if (match[i].squadra1.nome == sq2 || match[i].squadra2.nome == sq2)
                {
                    goal2 += match[i].goalsegnati(sq2);
                    subiti2 += match[i].goalsubiti(sq2);
                }
            }
            if(goal1 > goal2)
                return true;
            else if(goal1 < goal2)
                return false;
            else
            {
                if (subiti1 < subiti2)
                    return false;
                else if (subiti2 < subiti1)
                    return true;
                else
                {
                    Random r = new Random();
                    if (r.Next(0, 2) == 1)
                        return true;
                    else
                        return false;
                }
            }
        }

        public void caricaDate()
        {
            List<DateTime> date = new List<DateTime>();
            for (int i = 0; i < match.Count; i++)
            {
                bool presente = false;
                for (int j = 0; j < date.Count; j++)
                {
                    if (match[i].giorno.ToShortDateString() == date[j].ToShortDateString())
                    {
                        presente = true;
                    }
                }
                if (!presente)
                {
                    date.Add(match[i].giorno);
                    comboBox2.Items.Add(match[i].giorno.ToShortTimeString());
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            caricaPartite();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button6.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel5.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            giornate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel5.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            caricaDate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel5.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel5.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            classifica();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gironi();
        }
    }
}
