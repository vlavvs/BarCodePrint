using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using BarcodeLib;
using System.Windows.Forms;

namespace BarCodePrint
{
    public class clTask
    {
        private System.Drawing.Image img1;

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

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Point loc = new Point(10, 10);
            e.Graphics.DrawImage(img1, loc);
            e.HasMorePages = false;
        }

        public int StartPrint(string inPrinterName, int inStart, int inCount, int inPrefix, ProgressBar inProgressBar)
        {
            int pos, code1;
            string code;

            inProgressBar.Minimum = inStart;
            inProgressBar.Maximum = inStart + inCount - 1;

             for (pos = inStart; pos < inStart + inCount; ++pos)
            {
                code = inPrefix.ToString() + pos.ToString("D6");

                //Была какая то хня при сканировании штрихкода был спереди 0, при том что в этикетке его нет
                //тем самым сделал двойное переконвертирование чтоб его убрать, т.к. в int нет первого нуля
                code1 = int.Parse(code);
                code = code1.ToString();

                PrintCode(code, inPrinterName);
                inProgressBar.Value = pos;
            }
            return pos;
        }

    }
}
