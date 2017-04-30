using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using SheetsQuickstart;
using System.Management;
using System.Web;
using System.Text.RegularExpressions;
using NDde.Client;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // definere den timer der tæller ned.
        System.Windows.Forms.Timer timer1;

        // definere de strings og ints vi bruger til at holde styr på tiden.
        string min;
        string hour;
        int minutes;
        int hours;

        static object[] checkedPrograms = new object[Program.processList.Length];
        int range = 0;

        // "progress" bliver brugt på et andet tidspunkt
        static int progress;

        public static string[] websites;

        // definerer den mængde Threads der skal checke start processerne
        static int threads = 4;
        bool hasExited;
        public static bool butPressed = false;

        public Form1()
        {
            // definerer og starter threadsne
            InitializeComponent();
            Thread rP1 = new Thread(ReadProcesses1);
            Thread rP2 = new Thread(ReadProcesses2);
            Thread rp3 = new Thread(ReadProcesses3);     
            rP1.Start();
            rP2.Start();
            rp3.Start();          
        }

        // genererer vores fulde liste af de programmer der skal lukkes
        // via vores sheet 
        void CreateProgramArray(object[] checkedArray, object[] programArray)
        {
            string[] check = checkedArray.Where(x => x != null)
           .Select(x => x.ToString())
           .ToArray();
            for (int i = 0; i < check.Length; i++)
            {
                if(check[i] == "TRUE")
                {
                    checkedPrograms[i] = programArray[i];
                }
            }
        }


        // ReadProcesses metoden, kører samtidig med hindanen med defineret antal threads
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
            StopProcesses(checkedPrograms, threads, 0, stop);
            
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
            StopProcesses(checkedPrograms, threads, 1, stop);
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
            StopProcesses(checkedPrograms, threads, 2, stop);
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            hour = textBox1.Text;
            // opdatere info label, med den antal tid der er
            info.Text = textBox1.Text + " Timer, " + textBox2.Text + " Minutter";
            try
            {
                // prøver at parse den string der er i teksboksen til en int
                hours = Int32.Parse(textBox1.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);

                // laver en message box som der siger at man kun kan have tal
                // i tilfælde af at man prøver at skrive tal.
                if (string.IsNullOrWhiteSpace(textBox1.Text) == false)
                {
                    MessageBox.Show(this,"Du kan kun indsætte tal her","ADVARSEL",MessageBoxButtons.OK);
                }
            }
        }

        // denne textbox metoder gør det samme som den forrige 
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            min = textBox2.Text;
            info.Text = textBox1.Text + " Timer, " + textBox2.Text + " Minutter";
            try
            {
                minutes = Int32.Parse(textBox2.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);
                if (string.IsNullOrWhiteSpace(textBox2.Text) == false)
                {
                    MessageBox.Show(this, "Du kan kun indsætte tal her", "ADVARSEL", MessageBoxButtons.OK);
                }
            }
            // Sørger for at hvis minutter er over 60 så så trækker den 60 fra minutter og lægger 1 til time
            if (minutes > 60)
            {
                hours++;
                minutes = minutes - 60;
                textBox2.Text = "" + minutes;
                textBox1.Text = "" + hours;
            }

        }

        // Ikke burgt men nødvændig metode
        private void info_Click(object sender, EventArgs e)
        {

        }

        // Starter timer og Process check
        public void button1_Click(object sender, EventArgs e)
        {
            // checker om der er nogen kasser der tomme.
            if(string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // åbner en messagebox for at lade burgeren vide han har lavet en fejl
                MessageBox.Show(this,"Du skal skrive noget før du kan starte!","ADVARSEL", MessageBoxButtons.OK);
            }
            else
            {   
                // skjuler alt bortset fra timeren.
                textBox1.Visible = false;
                textBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                info.Visible = false;

                // minder brugeren om at han skal huske at gemme alt inden han starter
                MessageBox.Show(this, "Husk at gemme alt hvad du har åbent", "ADVARSEL", MessageBoxButtons.OK);
                butPressed = true;

                // starter check af processer
                StopProcesses(checkedPrograms, threads, 3, Process.GetProcesses().Length);

                // definerer en ny timer og starter den.
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000; // 1 second
                timer1.Start();

                // skriver det til value label
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            }
        }

        // indeholder koden til timeren
        private void timer1_Tick(object sender, EventArgs e)
        {
            // trækker et minut fra hver gang den kører
            minutes--;

            // skriver det til value label
            Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";

            // checker om minutter er 0 og trækker 1 fra timer.
            if (minutes == 0 && hours != 0)
            {
                hours--;
                minutes = 60;
            }

            // stopper timeren
            if (minutes == 0 && hours == 0)
            {
                timer1.Stop();
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            }
        }

        // metoden der sørger for at processerne bliver checket og lukket hvis de skal lukkes
        public static void StopProcesses(object[] Processes, int increment, int startValue, int endValue)
        {
            // definerer en ny process array som der indsamler alle åbne processer
            Process[] process = Process.GetProcesses();

            // omdanner den liste som vi fik fra google sheets til en string array
            string[] proc = Processes.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();

            // for loop der gennemgår alle åbne processer
            for (int j = startValue; j < endValue; j = j + increment)
            {   
                // progress variabel som bruges senere
                //progress++;

                // gennemgår hele vores liste og sammen ligner den med den nuværende process der bliver checket
                for (int i = 0; i < proc.Length; i++)
                {
                    // prøver at lukke programmet hvis det skal lukkes
                    try
                    {
                        if(process[j].ProcessName.ToLower() == proc[i].ToLower())
                        {
                            process[j].Kill();
                        }
                    }

                    // skriver til console hvis der opstår en fejl
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                    }
                }
            }
            Console.WriteLine("Finished");
        }

        // overload metode til process metoden
        public static void StopProcesses(object[] Processes, string Proces)
        {
            // definerer en variable som der fortæller om den har fundet et match i vores liste
            bool found = false;

            // får igen alle processer
            Process[] process = Process.GetProcesses();

            // omdanner igen vores liste til en string array
            string[] proc = Processes.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            if (Proces.ToLower() == "taskmgr")
            {
                for (int j = 0; j < process.Length; j++)
                {
                    if (Proces.ToLower() == process[j].ProcessName.ToLower())
                    {
                        process[j].Kill();
                    }
                }
            }
            else
            {
                // checker hele vores list of sammenligner med den den angivne process
                for (int i = 0; i < proc.Length; i++)
                {
                    if (Proces.ToLower() == proc[i].ToLower())
                    {
                        found = true;
                    }

                    // lukker programmet hvis det findes i åbne processer
                    if (found == true)
                    {
                        for (int j = 0; j < process.Length; j++)
                        {
                            if (Proces.ToLower() == process[j].ProcessName.ToLower())
                            {
                                // prøver at lukke programmet og giver en fejl hvis det ikke kunne lade sig gøre
                                try
                                {
                                    Thread.Sleep(1000);
                                    process[j].Kill();
                                }

                                // skriver til console hvis der opstår en fejl
                                catch (Exception c)
                                {
                                    Console.WriteLine(c.Message);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Closed");
            }
        }

        // checker hvordan programmet blev lukket
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

        // metode som vi kan bruge fra andre scripts, som fortæller om programmet er blevet lukket.
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

        // ikke brugt men nødvændig metode
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateProgramArray(Program.checkedState, Program.processList);
            string[] check = checkedPrograms.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            Console.WriteLine(check.Length);
            for (int i = 0; i < check.Length; i++)
            {
                Console.WriteLine(check[i]);
            }
        }

        // ikke brugt men nødvændig metode
        private void Values_TextChanged(object sender, EventArgs e)
        {

        }

        // ikke brugt men nødvændig metode
        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            //Console.WriteLine(GetBrowserURL("firefox"));    
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public static void UpdateHostFile(string updateText, bool Remove)
        {
            string path = @"C:\Windows\System32\drivers\etc\hosts";
            Thread.Sleep(300);
            if (!Remove)
            {
                using (StreamWriter w = File.AppendText(path))
                {
                    if (!updateText.Contains("www."))
                    {
                        w.WriteLine("127.0.0.1 " + "www." + updateText);
                    }
                    else
                    {
                        w.WriteLine("127.0.0.1 " + updateText);
                    }
                }
            }
            if(Remove)
            {
                File.Delete(path);
                Thread.Sleep(200);
                File.Create(path).Close();

                using (StreamWriter w = File.AppendText(path))
                {
                    if(!updateText.Contains("www."))
                    {
                        w.WriteLine("127.0.0.1" + "www." + updateText);
                    }
                    else
                    {
                        w.WriteLine("127.0.0.1" + updateText);
                    }
                }
            }
            ListOfWebsites.progressWeb = 100;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void progs_Click(object sender, EventArgs e)
        {
            var list_form = new ListOfProcesses();
            list_form.Show();
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            var list_websites = new ListOfWebsites();
            list_websites.Show();
        }

        private void roundButton2_Click(object sender, EventArgs e)
        {
            // checker om der er nogen kasser der tomme.
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // åbner en messagebox for at lade burgeren vide han har lavet en fejl
                MessageBox.Show(this, "Du skal skrive noget før du kan starte!", "ADVARSEL", MessageBoxButtons.OK);
            }
            else
            {
                // skjuler alt bortset fra timeren.
                textBox1.Visible = false;
                textBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                info.Visible = false;

                // minder brugeren om at han skal huske at gemme alt inden han starter
                MessageBox.Show(this, "Husk at gemme alt hvad du har åbent", "ADVARSEL", MessageBoxButtons.OK);
                butPressed = true;

                // starter check af processer
                StopProcesses(checkedPrograms, threads, 3, Process.GetProcesses().Length);

                // definerer en ny timer og starter den.
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000; // 1 second
                timer1.Start();

                // skriver det til value label
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            }
        }
    }
}
