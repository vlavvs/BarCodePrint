using BarcodeLib;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BarCodePrint
{
    public class clTaskComplete
    {
        public string Printer;
        public ProgressBar cProgressBar;
        public Label label;

        public clTaskComplete(ProgressBar cProgressBar, string Printer, Label label)
        {
            this.cProgressBar = cProgressBar;
            this.Printer = Printer;
            this.label = label;
        }
    }

    public class clTaskAction //класс для передачи текущей позиции в прогресс бар из потока
    {
        public ProgressBar cProgressBar;
        public int pos;

        public clTaskAction(ProgressBar cProgressBar)
        {
            this.cProgressBar = cProgressBar;
        }
    }

    public class clTaskConfig //класс для передачи параметров в прогресс бар из потока
    {
        public ProgressBar cProgressBar;
        public int start, count;

        public clTaskConfig(ProgressBar cProgressBar, int start, int count)
        {
            this.cProgressBar = cProgressBar;
            this.start = start;
            this.count = count;
        }
    }

    public class clTask
    {
        private string inPrinterName;
        private int inStart;
        private int inCount;
        private int inPrefix;
        private ProgressBar inProgressBar;
        private Label locLabel;

        private System.Drawing.Image img1;

        public clTask(string inPrinterName, int inStart, int inCount, int inPrefix, ProgressBar inProgressBar, Label inLabel)
        {
            this.inPrinterName = inPrinterName;
            this.inStart = inStart;
            this.inCount = inCount;
            this.inPrefix = inPrefix;
            this.inProgressBar = inProgressBar;
            this.locLabel = inLabel;
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
            int pos, code1;
            string code;

            clTaskConfig TaskConfig = new clTaskConfig(inProgressBar, inStart, inCount);
            clTaskAction TaskAction = new clTaskAction(inProgressBar);

            TaskConfig.start = inStart;
            TaskConfig.count = inCount;

            //Передаем настройки в прогресс бар
            ProgressBarConfig(TaskConfig);

            for (pos = inStart; pos < inStart + inCount; ++pos)
            {
                code = inPrefix.ToString() + pos.ToString("D6");

                //Была какая то хня при сканировании штрихкода был спереди 0, при том что в этикетке его нет
                //тем самым сделал двойное переконвертирование чтоб его убрать, т.к. в int нет первого нуля
                code1 = int.Parse(code);
                code = code1.ToString();

                TaskAction.pos = pos;

                //передаем прогресс
                ProgressBarAction(TaskAction);

                PrintCode(code, inPrinterName);
            }

            clTaskComplete taskComplete = new clTaskComplete(inProgressBar, inPrinterName, locLabel);
            TaskComplete(taskComplete);
        }

        public event Action<clTaskAction> ProgressBarAction;
        public event Action<clTaskConfig> ProgressBarConfig;
        public event Action<clTaskComplete> TaskComplete;
    }
}
