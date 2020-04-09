using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BarCodePrintSolo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                comboBox1.Items.Add(strPrinter);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Не выбран принтер!");
                return;
            }

            int IntParse;

            if (int.TryParse(textBox1.Text, out IntParse) == false)
            {
                MessageBox.Show("Не верно указанно количество!");
                return;
            }

            clTask Task = new clTask(comboBox1.Text, int.Parse(textBox1.Text));
            Task.StartPrint();
            textBox1.Clear();
            textBox1.Focus();

        }
    }
}
