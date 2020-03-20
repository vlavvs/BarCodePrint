
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Threading;

namespace BarCodePrint
{
    public partial class Main : Form
    {
        
        static public lab Config = new lab();

        public Main()
        {
            InitializeComponent();

            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                comboBox2.Items.Add(strPrinter);

            Config = Config.LoadFromXML("config.xml");

            foreach (string sur in Config.GetNameSurvey())
                comboBox1.Items.Add(sur);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            survey sur = new survey();
            sur = Config.GetSurvey(comboBox1.SelectedItem.ToString());

            

            Thread myThread = new Thread(print);
            myThread.Start(sur);

            //sur.position = Task.StartPrint(comboBox2.Text, sur.position, int.Parse(textBox1.Text), sur.prefix, progressBar1);
           
            comboBox1_SelectedIndexChanged(this, e);
        }

        public static void print()
        {
            clTask Task = new clTask();

            sur.position = Task.StartPrint(comboBox2.Text, sur.position, int.Parse(textBox1.Text), sur.prefix, progressBar1);

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
