using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace L4
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<List<Sandelys>> Sandeliai = new List<List<Sandelys>>();
            double max = 0;
            List<Uzsakymas> Uzsakymai = new List<Uzsakymas>();
            try {
                MaxNustatymas(TextBox1.Text, ref max);
            }catch(Exception ex){
                Label2.Text = "Klaida: " + ex;
                return;
            }
            
            Skaitymas(Sandeliai, Uzsakymai);
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/rezultatai.txt"), true);
            Label3.Text = "Pradiniai duomenys:";
            Label4.Text = "Prekės";
            sw.WriteLine(Label3.Text);
            sw.WriteLine(Label4.Text);
            Spausdinti(Table1, Sandeliai, sw);
            Label6.Text = "Užsakymas: ";
            sw.WriteLine(Label6.Text);
            Spausdinti(Table2, Uzsakymai, sw);
            List<Sandelys> Prekes = new List<Sandelys>();
            double kaina = 0;
            Vykdymas(Sandeliai, Uzsakymai, out kaina, out Prekes);
            try
            {
                PrekiuSutikrinimas(Prekes, max, ref kaina);
            }
            catch (Exception ex)
            {
                Label2.Text = "Klaida: " + ex;
                return;
            }
            
            Prekes.Sort();
            Label7.Text = "Rezultatai";
            sw.WriteLine(Label7.Text);
            Spausdinti(Table3, Prekes, sw);
            KainosSutvarkymas(Prekes, out kaina);
            Label5.Text = "Viso mokėti: " + kaina;
            sw.WriteLine(Label5.Text);
            sw.Close();
        }
        /// <summary>
        /// Maksimalios kainos nustatymas
        /// </summary>
        /// <param name="text">Įrašytas kiekis</param>
        /// <param name="max">Grąžinama suma</param>
        void MaxNustatymas(string text, ref double max)
        {
            max = double.Parse(TextBox1.Text);
            if (max < 0) throw new Exception("Suma, per maža");
        }
        /// <summary>
        /// Kainų sutvarkymo metodas
        /// </summary>
        /// <param name="prekes">Prekių sąrašas</param>
        /// <param name="kaina">Prekių kaina</param>
        void KainosSutvarkymas(List<Sandelys> prekes, out double kaina)
        {
            kaina = 0;
            foreach(var preke in prekes)
            {
                kaina += preke.Kaina*preke.Kiekis;
            }
        }
        /// <summary>
        /// Prekių mažinimas
        /// </summary>
        /// <param name="Prekes">Prekių sąrašas</param>
        /// <param name="Preke">Paduota prekė</param>
        /// <param name="kiek">Kiekis</param>
        void SumazintiKieki(ref List<Sandelys> Prekes, Sandelys Preke, int kiek)
        {
            foreach(var preke in Prekes.ToList())
            {
                if (preke.Equals(Preke))
                {
                    if (preke.Kiekis >= kiek)
                    {
                        preke.Kiekis -= kiek;
                        if(preke.Kiekis == 0)
                        {
                            try
                            {
                                Prekes.Remove(preke);
                            }catch(Exception ex)
                            {
                                Label2.Text = "Klaida: " + ex;
                                return;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Kiekis per didelis");
                    }
                }
            }
        }
        /// <summary>
        /// Pigiausios prekės trinimo metodas
        /// </summary>
        /// <param name="Prekes">Prekių sąrašas</param>
        /// <param name="max">Didžiausia suma</param>
        /// <param name="kaina">Kaina</param>
        void TrintiPigiausia(List<Sandelys> Prekes, double max, double kaina)
        {
            Sandelys pigiausia = new Sandelys();
            foreach(var preke in Prekes)
            {
                if (pigiausia.Kaina == 0 || preke.Kaina < pigiausia.Kaina) {
                    pigiausia = preke;
                }
            }

            kaina = kaina - pigiausia.Kaina;
            try
            {
                SumazintiKieki(ref Prekes, pigiausia, 1);
                PrekiuSutikrinimas(Prekes, max, ref kaina);
            }
            catch(Exception ex)
            {
                Label2.Text = "Klaida: " + ex;
                return;
            }
        }
        /// <summary>
        /// Prekių sutikrinimo metodas
        /// </summary>
        /// <param name="Prekes">Prekių sąrašas</param>
        /// <param name="max">Didžiausias suma</param>
        /// <param name="kaina">Kaina</param>
        void PrekiuSutikrinimas(List<Sandelys> Prekes, double max, ref double kaina)
        {
            if (Prekes.Count != 0)
            {
                if (kaina > max)
                {
                    TrintiPigiausia(Prekes, max, kaina);
                }
            }
            else
            {
                kaina = 0;
                throw new Exception("Prekių nėra");
            }
        }
        /// <summary>
        /// Spausdinimo metodas
        /// </summary>
        /// <param name="table">Table klasės objektas</param>
        /// <param name="sandeliai">Sandelių sąrašas</param>
        /// <param name="sw">StreamWriter objektas</param>
        void Spausdinti(Table table, List<Uzsakymas> sandeliai, StreamWriter sw)
        {
            sw.WriteLine("{0,-20} {1,-20}", "Pavadinimas", "Kiekis");
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "Pavadinimas";
            row.Cells.Add(cell);
            TableCell cell2 = new TableCell();
            cell2.Text = "Kiekis";
            row.Cells.Add(cell2);
            table.Rows.Add(row);
            foreach (var sandelys in sandeliai)
            {
                sw.WriteLine(sandelys);
                TableRow row1 = new TableRow();
                TableCell cell11 = new TableCell();
                cell11.Text = sandelys.Pavadinimas;
                row1.Cells.Add(cell11);
                TableCell cell22 = new TableCell();
                cell22.Text = sandelys.Kiekis.ToString();
                row1.Cells.Add(cell22);
                table.Rows.Add(row1);
            }
        }
        /// <summary>
        /// Spausdinimo metodas
        /// </summary>
        /// <param name="table">Table klasės objektas</param>
        /// <param name="sandeliai">Sandelių sąrašas</param>
        /// <param name="sw">StreamWriter objektas</param>
        void Spausdinti(Table table, List<Sandelys> sandeliai, StreamWriter sw)
        {
            sw.WriteLine("{0,-20} {1,-20} {2, -20} {3, -20}", "Numeris", "Pavadinimas", "Kiekis", "Kaina");
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "Pavadinimas";
            row.Cells.Add(cell);
            TableCell cell2 = new TableCell();
            cell2.Text = "Kiekis";
            row.Cells.Add(cell2);
            TableCell cell3 = new TableCell();
            cell3.Text = "Kaina";
            row.Cells.Add(cell3);
            table.Rows.Add(row);
            foreach (var sandelys in sandeliai)
            {
                sw.WriteLine(sandelys);
                TableRow row1 = new TableRow();
                TableCell cell11 = new TableCell();
                cell11.Text = sandelys.Vardas;
                row1.Cells.Add(cell11);
                TableCell cell22 = new TableCell();
                cell22.Text = sandelys.Kiekis.ToString();
                row1.Cells.Add(cell22);
                TableCell cell33 = new TableCell();
                cell33.Text = sandelys.Kaina.ToString();
                row1.Cells.Add(cell33);
                table.Rows.Add(row1);
            }
        }
        /// <summary>
        /// Spausdinimo metodas
        /// </summary>
        /// <param name="table">Table klasės objektas</param>
        /// <param name="sandeliai">Sandelių sąrašas</param>
        /// <param name="sw">StreamWriter objektas</param>
        void Spausdinti(Table table, List<List<Sandelys>> sandeliai, StreamWriter sw)
        {
            sw.WriteLine("{0,-20} {1,-20} {2, -20} {3, -20}", "Numeris", "Pavadinimas", "Kiekis", "Kaina");
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "Pavadinimas";
            row.Cells.Add(cell);
            TableCell cell2 = new TableCell();
            cell2.Text = "Kiekis";
            row.Cells.Add(cell2);
            TableCell cell3 = new TableCell();
            cell3.Text = "Kaina";
            row.Cells.Add(cell3);
            table.Rows.Add(row);
            foreach (var Sandelys in sandeliai)
            {
                foreach (var sandelys in Sandelys)
                {
                    sw.WriteLine(sandelys);
                    TableRow row1 = new TableRow();
                    TableCell cell11 = new TableCell();
                    cell11.Text = sandelys.Vardas;
                    row1.Cells.Add(cell11);
                    TableCell cell22 = new TableCell();
                    cell22.Text = sandelys.Kiekis.ToString();
                    row1.Cells.Add(cell22);
                    TableCell cell33 = new TableCell();
                    cell33.Text = sandelys.Kaina.ToString();
                    row1.Cells.Add(cell33);
                    table.Rows.Add(row1);
                }
            }
        }
        /// <summary>
        /// Pigiausios prekės objektas
        /// </summary>
        /// <param name="Sandeliai">Sandelių sąrašas</param>
        /// <param name="ieskoma">Ieskoma preke</param>
        /// <returns></returns>
        public Sandelys PigiausiaPreke(List<List<Sandelys>> Sandeliai, Uzsakymas ieskoma)
        {
            Sandelys pig = new Sandelys();
            foreach (var Sandelys in Sandeliai)
            {
                foreach (var sandelys in Sandelys)
                {
                    if (sandelys.Vardas.Equals(ieskoma.Pavadinimas))
                    {
                        if (pig.Kaina == 0) pig = sandelys;
                        else
                        {
                            if (pig.Kaina > sandelys.Kaina) pig = sandelys;
                        }
                    }
                }
            }
            pig.Kiekis = ieskoma.Kiekis;
            return pig;
        }
        /// <summary>
        /// Pagrindinis vykdymo metodas
        /// </summary>
        /// <param name="Sandeliai">Sandelių sąrašas</param>
        /// <param name="Uzsakymai">Užsakymo sąrašas</param>
        /// <param name="kaina">Kaina</param>
        /// <param name="Uzsakymas">Užsakymas</param>
        public void Vykdymas(List<List<Sandelys>> Sandeliai, List<Uzsakymas> Uzsakymai, out double kaina, out List<Sandelys> Uzsakymas)
        {
            kaina = 0;
            Uzsakymas = new List<Sandelys>();
            foreach (var uzsakymas in Uzsakymai) {
                Uzsakymas.Add(PigiausiaPreke(Sandeliai, uzsakymas));
                kaina += PigiausiaPreke(Sandeliai, uzsakymas).Kaina*uzsakymas.Kiekis;
            }
        }
        /// <summary>
        /// Skaitymo metodas
        /// </summary>
        /// <param name="Sandeliai">Sandelių sąrašas</param>
        /// <param name="Uzsakymai">Užsakymų sąrašas</param>
        public void Skaitymas(List<List<Sandelys>> Sandeliai, List<Uzsakymas> Uzsakymai)
        {
            string path = Server.MapPath("~/App_Data/");
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        if (!Path.GetFileName(file).Equals("Uzsakymas.txt"))
                        {
                            List<Sandelys> sandelys = new List<Sandelys>();
                            string eil = null;
                            int nr = int.Parse(reader.ReadLine());
                            while (null != (eil = reader.ReadLine()))
                            {
                                try
                                {
                                    string[] duom = eil.Split(';');

                                    Sandelys sand = new Sandelys(nr, duom[0], int.Parse(duom[1]), double.Parse(duom[2]));
                                    
                                    sandelys.Add(sand);
                                }
                                catch (Exception ex)
                                {
                                    Label2.Text = "Klaida: " + ex;
                                    return;
                                }
                            }
                            Sandeliai.Add(sandelys);
                        }
                        else
                        {
                            string eil = null;
                            while (null != (eil = reader.ReadLine()))
                            {
                                try
                                {
                                    string[] duom = eil.Split(';');
                                    Uzsakymas uzsakymas = new Uzsakymas(duom[0], int.Parse(duom[1]));
                                    Uzsakymai.Add(uzsakymas);
                                }
                                catch (Exception ex)
                                {
                                    Label2.Text = "Klaida: " + ex;
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Label2.Text = "Klaida: " + ex;
                    return;
                }
            }
        }
    }
}