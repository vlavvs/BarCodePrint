using System.Collections.Generic;

namespace BarCodePrint
{
    public class clActivePrinter
    {
        private List<string> ListActivePrinter = new List<string>();

        public int AddPrinterBusy(string strPrinterName)
        {
            foreach (string strPrinter in ListActivePrinter)
            {
                if (strPrinter == strPrinterName)
                {
                    return 1;
                }
            }
            ListActivePrinter.Add(strPrinterName);
            return 0;
        }

        public int DelPrinterBusy(string strPrinterName)
        {
            foreach (string strPrinter in ListActivePrinter)
            {
                if (strPrinter == strPrinterName)
                {
                    ListActivePrinter.Remove(strPrinterName);
                    return 0;
                }
            }
            return 1;
        }

        public int GetCountPrinterBusy()
        {
            return ListActivePrinter.Count;
        }
    }
}
