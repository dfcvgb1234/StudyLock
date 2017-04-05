using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using SheetsQuickstart;
using System.Management;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer1;
        string min;
        string hour;
        int minutes;
        int hours;
        static int progress;
        static int threads = 4;
        public IList<IList<object>> closeProcess ;
        bool hasExited;
        static string[] procInL = new string[Program.processList.Length];
        public static bool butPressed = false;
        



        public Form1()
        {
            closeProcess = new List<IList<object>>();
            InitializeComponent();
            Thread rP1 = new Thread(ReadProcesses1);
            Thread rP2 = new Thread(ReadProcesses2);
            Thread rp3 = new Thread(ReadProcesses3);     
            rP1.Start();
            rP2.Start();
            rp3.Start();          
        }

        static void ReadProcesses1()
        {
            Console.WriteLine("Thread 'rP1' has been run");
            while (true)
            {
                if (butPressed == true)
                {
                    break;
                }
            }
            int stop = Process.GetProcesses().Length / threads;
            StopProcesses(Program.processList, threads, 0, stop);
            
        }
        static void ReadProcesses2()
        {
            Console.WriteLine("Thread 'rP2' has been run");
            while (true)
            {
                if (butPressed == true)
                {
                    break;
                }
            }
            int stop = Process.GetProcesses().Length / threads;
            stop = stop * 2;
            StopProcesses(Program.processList, threads, 1, stop);
        }
        static void ReadProcesses3()
        {
            Console.WriteLine("Thread 'rP3' has been run");
            while (true)
            {
                if (butPressed == true)
                {
                    break;
                }
            }
            int stop = Process.GetProcesses().Length / threads;
            stop = stop * 3;
            StopProcesses(Program.processList, threads, 2, stop);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            hour = textBox1.Text;
            info.Text = textBox1.Text + " Hours, " + textBox2.Text + " Minues";
            try
            {
                hours = Int32.Parse(textBox1.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);
                if (string.IsNullOrWhiteSpace(textBox1.Text) == false)
                {
                    MessageBox.Show("ONLY NUMBERS!");
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            min = textBox2.Text;
            info.Text = textBox1.Text + " Hours, " + textBox2.Text + " Minutes";
            try
            {
                minutes = Int32.Parse(textBox2.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);
                if (string.IsNullOrWhiteSpace(textBox2.Text) == false)
                {
                    MessageBox.Show("ONLY NUMBERS!");
                }
            }
            if (minutes > 60)
            {
                hours++;
                minutes = minutes - 60;
                textBox2.Text = "" + minutes;
                textBox1.Text = "" + hours;
            }

        }

        private void info_Click(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = Process.GetProcesses().Length;
            if(string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("You can not leave any fields empty!");
            }
            else
            {
                textBox1.Visible = false;
                textBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                button1.Visible = false;
                info.Visible = false;
                
                MessageBox.Show("Remember to save everything before starting");
                butPressed = true;
                StopProcesses(Program.processList, threads, 3, Process.GetProcesses().Length);
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000; // 1 second
                timer1.Start();
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            minutes--;
            Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            if (minutes == 0 && hours != 0)
            {
                hours--;
                minutes = 60;
            }
            if (minutes == 0 && hours == 0)
            {
                timer1.Stop();
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            }
        }
        public static void StopProcesses(object[] Processes, int increment, int startValue, int endValue)
        {
            Console.WriteLine("StopProcesses has been run");
            Process[] process = Process.GetProcesses();
            string[] proc = Processes.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            Console.WriteLine(proc.Length);
            for (int j = startValue; j < endValue; j = j + increment)
            {
                progress++;
                for (int i = 0; i < proc.Length; i++)
                {
                    try
                    {
                        Console.WriteLine(process[j].ProcessName.ToLower());
                        if(process[j].ProcessName.ToLower() == proc[i].ToLower())
                        {
                            Console.WriteLine("Din mor");
                            process[j].Kill();
                        }
                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                    }
                }
            }
            Console.WriteLine("Finished");
        }

        public static void StopProcesses(object[] Processes, string Proces)
        {
            bool found = false;
            Console.WriteLine("StopProcesses has been run");
            Process[] process = Process.GetProcesses();
            string[] proc = Processes.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            Console.WriteLine(proc.Length);
            for (int i = 0; i < proc.Length; i++)
            {
                try
                {
                    if (Proces.ToLower() == proc[i].ToLower())
                    {
                        Console.WriteLine("Process Killed: {0}", proc[i].ToLower());
                        found = true;
                    }
                }
                catch (Exception c)
                {
                    Console.WriteLine(c.Message);
                }
                if (found == true)
                {
                    for (int j = 0; j < process.Length; j++)
                    {
                        if (Proces.ToLower() == process[j].ProcessName.ToLower())
                        {
                            try
                            {
                                Thread.Sleep(1000);
                                process[j].Kill();
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }   
            Console.WriteLine("Closed");
        }
        void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                hasExited = true;
                killProg();
            }

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                hasExited = true;
                killProg();
            }
        }
        public bool killProg()
        {
            if(hasExited)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Values_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
