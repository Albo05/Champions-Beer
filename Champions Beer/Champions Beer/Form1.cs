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
                match.Add(match[i]);
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
                for (int j = 0; j < appo.Count; j++)
                {
                    if (match[i].vincitore() == appo[j])
                        risultati[j] += 3;
                    else if (match[i].vincitore() == "pareggio")
                        risultati[j]++;
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
                }
            }
            for (int i = 0; i < appo.Count; i++)
            {
                appo2 += $"{appo[i]}: {risultati[i]}\n";
            }

            label13.Text = appo2;
        }

        public void classifica()
        {
            List <string> appo = new List <string>();
            for (int i = 0; i < match.Count; i++)
            {
                appo.Add(match[i].squadra1.ToString());
                appo.Add(match[i].squadra2.ToString());
            }
            string[] appo2 = appo.ToArray();
            appo.Clear();
            for (int i = 0; i < appo2.Length; i++)
            {
                bool giaPresente = false;
                for (int j = 0; j < appo.Count; j++)
                {
                    if (appo2[i] == appo[j])
                        giaPresente = true;
                }
                if (!giaPresente)
                {
                    appo.Add(appo2[i]);
                }
            }
            string[] squadre = appo.ToArray();
            appo.Clear();
            int[] vittorie = new int[squadre.Length];
            for (int i = 0; i < match.Count; i++)
            {
                for (int j = 0; j < squadre.Length; j++)
                {
                    if (match[i].vincitore() == squadre[j])
                        vittorie[j] += 3;
                    else if (match[i].vincitore() == "pareggio")
                        vittorie[j]++;
                }
            }
            for (int i = 0; i < vittorie.Length; i++)
            {
                for (int j = 0; j < vittorie.Length; j++)
                {
                    if(vittorie[i] > vittorie[j])
                    {
                        int appo3 = vittorie[i];
                        vittorie[i] = vittorie[j];
                        vittorie[j] = appo3;
                        string appo4 = squadre[i];
                        squadre[i] = squadre[j];
                        squadre[j] = appo4;
                    }
                }
            }
            string appo5 = "";
            for (int i = 0; i < squadre.Length; i++)
            {
                appo5 += $"{squadre[i]}: {vittorie[i]}\n";
            }
            label2.Text = squadre[0];
            label3.Text = squadre[1];
            label2.Text = squadre[2];
            label14.Text = appo5;
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
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            button7.Enabled = true;
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

        private void button8_Click(object sender, EventArgs e)
        {
            gironi();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button8.Enabled = true;
        }
    }
}
