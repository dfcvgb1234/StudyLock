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
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using SheetsQuickstart;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        String[] Pvs = File.ReadAllText(@"C:\Users\Mc IceBerg\AppData\Roaming\StudyLock\Process.csv").Split(',');
        Timer timer1;
        int minutes;
        int hours;
        bool hasExited;
        Process[] process;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            info.Text = textBox1.Text + " Hours, " + textBox2.Text + " Minues";
            try
            {
                hours = Int32.Parse(textBox1.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            info.Text = textBox1.Text + " Hours, " + textBox2.Text + " Minutes";
            try
            {
                minutes = Int32.Parse(textBox2.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);
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

        private void button1_Click(object sender, EventArgs e)
        {
            
            textBox1.Visible = false;
            textBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            foreach (String i in Pvs)
            {
                Console.WriteLine(i);
            }
            MessageBox.Show("Remember to save everything before starting");
            KillPrograms(Pvs);
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
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
        public void KillPrograms(String[] processes)
        {
            process = Process.GetProcesses();
            Console.WriteLine(process[4]);
            Console.WriteLine(process.Length);
        // Kører igennem alle processer
        for(int i = 0; i < processes.Length; i++)
            {
                Console.WriteLine("Runned through all process");
                // Checker hver gang loopet kører igennem
                foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcesses())
                {
                    int j = 0;
                    System.Threading.Thread.Sleep(100);
                    Console.WriteLine(j);
                    j++;
                    // "omdøber" process navnene til lower case så det kan tjekkes
                    if(process.ProcessName.ToLower() == processes[i].ToLower())
                    {
                        process.Kill();
                        Console.WriteLine("Killed");
                    }
                    
                }
                
            }
            // lortet virker ikke, hvis du kan regne den ud kan du se i debug hvorfor
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
    }
}
