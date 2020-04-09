using BarcodeLib;
using System.Drawing;
using System.Drawing.Printing;

namespace BarCodePrintSolo
{
    public class clTask
    {
        private string locPrinterName;
        private int locCode;

        private System.Drawing.Image img1;

        public clTask(string inPrinterName, int inCode)
        {
            this.locPrinterName = inPrinterName;
            this.locCode = inCode;
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
            img1 = barcode.Encode(TYPE.CODE128C, code); //используем 128С т.к. в С кодируются только цифры в результате чего в этикетку влазит большее количество
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

        public void StartPrint()
        {
            string BarCode = locCode.ToString("D6");
            locCode = int.Parse(BarCode);
            PrintCode(locCode.ToString(), locPrinterName);
        }
    }
}
