using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarcodeLib;
using System.Drawing.Printing;

namespace BarCodePrint
{
    public partial class Main : Form
    {
        private System.Drawing.Image img1;
        public lab Config = new lab();

        public Main()
        {
            InitializeComponent();

            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                comboBox2.Items.Add(strPrinter);
            
            Config = Config.LoadFromXML("config.xml");

            foreach (string sur in Config.GetNameSurvey())
                comboBox1.Items.Add(sur);
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Point loc = new Point(10, 10);
            e.Graphics.DrawImage(img1, loc);
            e.HasMorePages = false;
        }

        private void PrintCode(string code, string printer)
        {
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.CENTER,
                Width = 100,
                Height = 50,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = printer;
            img1 = barcode.Encode(TYPE.CODE128C, code);
            pd.PrintPage += PrintPage;
            pd.PrintController = new StandardPrintController(); //чтоб не показывалось окно "идет печать"
            pd.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Config.AddSurvey("Моча", 11, 999999);
            //Config.AddSurvey("Кал", 12, 999999);

            survey sur = new survey();
            sur = Config.GetSurvey(comboBox1.SelectedItem.ToString());

            int pos;
            string code;
            int code1;

            progressBar1.Minimum = sur.position;
            progressBar1.Maximum = sur.position + int.Parse(textBox1.Text);
            progressBar1.Step = 1;

            for (pos = sur.position; pos < sur.position + int.Parse(textBox1.Text); ++pos)
            {
                code = sur.prefix.ToString() + pos.ToString("D6");

                //Была какая то хня при сканировании штрихкода был спереди 0, при том что в этикетке его нет
                //тем самым сделал двойное переконвертирование чтоб его убрать, т.к. в int нет первого нуля
                code1 = int.Parse(code);
                code = code1.ToString();

                PrintCode(code, comboBox2.Text);
                progressBar1.Value = pos;
            }
            sur.position = pos;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Config.SaveToXML("config.xml");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            survey sur = new survey();            
            sur = Config.GetSurvey(comboBox1.SelectedItem.ToString());

            label3.Text = "Наименование: " + sur.name;
            label2.Text = "Префикс: " + sur.prefix;
            label4.Text = "Текущая позиция:" + sur.position;            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NewSurvey newform = new NewSurvey();
            newform.ShowDialog();
        }
    }
}
