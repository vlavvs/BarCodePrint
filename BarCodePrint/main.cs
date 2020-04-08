
using System;
using System.Drawing.Printing;
using System.Threading;
using System.Windows.Forms;

namespace BarCodePrint
{
    public partial class Main : Form
    {

        static public lab Config = new lab();
        static public clActivePrinter ActivePrinter = new clActivePrinter();

        public void TaskComplete(clTaskComplete taskComplete)
        {
            Action action = () =>
            {
                ActivePrinter.DelPrinterBusy(taskComplete.Printer);
                comboBox2.Items.Add(taskComplete.Printer);

                taskComplete.cProgressBar.Visible = false;
                taskComplete.label.Visible = false;
            };

            Invoke(action);
        }

        public void ProgressBarPosition(clTaskAction TaskAction)
        {
            Action action = () =>
            {
                TaskAction.cProgressBar.Value = TaskAction.pos;
            };

            Invoke(action);
        }

        public void ProgressBarConfig(clTaskConfig TaskConfig)
        {
            Action action = () =>
            {
                TaskConfig.cProgressBar.Minimum = TaskConfig.start;
                TaskConfig.cProgressBar.Maximum = TaskConfig.start + TaskConfig.count - 1;
            };

            Invoke(action);
        }

        public Main()
        {
            InitializeComponent();

            //загружаем список установленых принтеров
            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                comboBox2.Items.Add(strPrinter);

            //загружаем настройки из файла в класс
            Config = Config.LoadFromXML("config.xml");

            //грузим перечень исследований
            foreach (string sur in Config.GetNameSurvey())
                comboBox1.Items.Add(sur);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //получаем настройки исследований
            survey sur = new survey();
            sur = Config.GetSurvey(comboBox1.SelectedItem.ToString());

            ProgressBar CurrentProgressBar = progressBar1;
            Label CurrentLabel = label7;

            if (ActivePrinter.GetCountPrinterBusy() == 4)
            {
                MessageBox.Show("Достигнуто максимальное количество заданий");
                return;
            }

            switch (ActivePrinter.GetCountPrinterBusy())
            {
                case 2:
                    CurrentProgressBar = progressBar2;
                    CurrentLabel = label8;
                    break;
                case 3:
                    CurrentProgressBar = progressBar3;
                    CurrentLabel = label9;
                    break;
                case 4:
                    CurrentProgressBar = progressBar4;
                    CurrentLabel = label10;
                    break;
            }

            //инициализируем класс для печати штрихкода
            clTask Task = new clTask(comboBox2.Text, sur.position, int.Parse(textBox1.Text), sur.prefix, CurrentProgressBar, CurrentLabel);
            sur.position = sur.position + int.Parse(textBox1.Text);

            ActivePrinter.AddPrinterBusy(comboBox2.Text);
            comboBox2.Items.RemoveAt(comboBox2.SelectedIndex);

            CurrentProgressBar.Visible = true;
            CurrentLabel.Visible = true;

            //Подписываемся на события
            Task.ProgressBarAction += ProgressBarPosition;
            Task.ProgressBarConfig += ProgressBarConfig;
            Task.TaskComplete += TaskComplete;

            //Создаем и запускаем печать в потоке
            Thread myThread = new Thread(new ThreadStart(Task.StartPrint));
            myThread.Start();

            //Обновляем информацию о исследовании
            comboBox1_SelectedIndexChanged(this, e);
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

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
