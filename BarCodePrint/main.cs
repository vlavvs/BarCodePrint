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
        public System.Drawing.Image img1;

        public Main()
        {
            InitializeComponent();
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Point loc = new Point(10, 10);
            e.Graphics.DrawImage(img1, loc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.CENTER,
                Width = 90,
                Height = 50,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };

           // img1 = barcode.Encode(TYPE.CODE128C, "1234567890");
           // img.Save("123456789.png");

            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = comboBox2.Text;

            img1 = barcode.Encode(TYPE.CODE128C, "100000002");
            pd.PrintPage += PrintPage;

            pd.Print();

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Main_Shown(object sender, EventArgs e)
        {
            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                comboBox2.Items.Add(strPrinter);
        }
    }
}
