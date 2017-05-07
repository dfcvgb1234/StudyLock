using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using SheetsQuickstart;
using IWshRuntimeLibrary;
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
        int tmpmin;
        int tmphour;

        bool onceBut = false;

        public bool isElevated;

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
            FormClosed += Form1_FormClosed;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!butPressed)
                {
                    Environment.Exit(1);
                }
                else
                {

                }
            }

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                Environment.Exit(1);
            }
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
                if (check[i] == "TRUE")
                {
                    checkedPrograms[i] = programArray[i];
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            hour = textBox1.Text;
            // opdatere info label, med den antal tid der er
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
                    MessageBox.Show(this, "Du kan kun indsætte tal her", "ADVARSEL", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
            }
        }

        // denne textbox metoder gør det samme som den forrige 
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            min = textBox2.Text;
            try
            {
                minutes = Int32.Parse(textBox2.Text);
            }
            catch (FormatException c)
            {
                Console.WriteLine(c.Message);
                if (string.IsNullOrWhiteSpace(textBox2.Text) == false)
                {
                    MessageBox.Show(this, "Du kan kun indsætte tal her", "ADVARSEL", MessageBoxButtons.OK,MessageBoxIcon.Stop);
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

        // indeholder koden til timeren
        private void timer1_Tick(object sender, EventArgs e)
        {
            // trækker et minut fra hver gang den kører
            minutes--;

            // skriver det til value label
            Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";

            System.IO.File.WriteAllText(@"C:\Windows\System32\drivers\etc\Info.begeba", hours + ";" + minutes);

            // checker om minutter er 0 og trækker 1 fra timer.
            if (minutes <= 0 && hours != 0)
            {
                hours--;
                minutes = 60;
            }

            if (tmphour == tmphour - 2)
            {
                tmphour = tmphour - 2;
                roundButton1.PerformClick();
            }
            // stopper timeren
            if (minutes <= 0 && hours == 0)
            {
                timer1.Stop();
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
                System.IO.File.Delete(@"C:\Windows\System32\drivers\etc\hosts");
                System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),"StudyLock.lnk"));
                Environment.Exit(1);
            }
        }

        // metoden der sørger for at processerne bliver checket og lukket hvis de skal lukkes
        public static void StopProcesses(object[] Processes, int increment, int startValue, int endValue)
        {
            // omdanner den liste som vi fik fra google sheets til en string array
            string[] proc = Processes.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            foreach (string n in proc)
            {
                Process[] hej = Process.GetProcessesByName(n.ToLower());
                foreach (Process j in hej)
                {
                    try
                    {
                        j.Kill();
                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                    }
                }
            }
        }
        private void Values_TextChanged(object sender, EventArgs e)
        {

        }

        // overload metode til process metoden
        public static void StopProcesses(object[] Processes, string Proces)
        {
            // definerer en variable som der fortæller om den har fundet et match i vores liste
            bool found = false;

            // får igen alle processer

            // omdanner igen vores liste til en string array
            string[] proc = Processes.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            if (Proces.ToLower() == "taskmgr")
            {
                Process[] hej = Process.GetProcessesByName("taskmgr");
                foreach (Process j in hej)
                {
                    try
                    {
                        j.Kill();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                // checker hele vores list of sammenligner med den den angivne process
                foreach (string j in proc)
                {
                    if (j.ToLower() == Proces.ToLower())
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                    }
                }

                if(found == true)
                {
                    Process[] hej = Process.GetProcessesByName(Proces.ToLower());
                    foreach(Process j in hej)
                    {
                        try
                        {
                            j.Kill();
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }


            }
            Console.WriteLine("Closed");
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
            
            if (System.IO.File.Exists(@"C:\Windows\System32\drivers\etc\Info.begeba"))
            {
                if (!String.IsNullOrWhiteSpace(System.IO.File.ReadAllText(@"C:\Windows\System32\drivers\etc\Info.begeba")))
                {
                    string[] splitText = System.IO.File.ReadAllText(@"C:\Windows\System32\drivers\etc\Info.begeba").Split(';');
                    if (splitText[1] != "0" && splitText[0] != "0")
                    {
                        textBox1.Text = splitText[0];
                        textBox2.Text = splitText[1];
                        Thread.Sleep(10000);
                        roundButton2.PerformClick();
                    }
                }
            }
            CreateProgramArray(Program.checkedState, Program.processList);
            string[] check = checkedPrograms.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            Console.WriteLine(check.Length);
            for (int i = 0; i < check.Length; i++)
            {
                Console.WriteLine(check[i]);
            }
            if (!System.IO.File.Exists(@"C:\Windows\System32\drivers\etc\host.begeba"))
            {
                System.IO.File.Create(@"C:\Windows\System32\drivers\etc\host.begeba").Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public static void UpdateHostFile(string updateText)
        {
            string path = @"C:\Windows\System32\drivers\etc\hosts";
            using (StreamWriter w = System.IO.File.AppendText(path))
            {
                if(!updateText.Contains("www."))
                {
                    w.WriteLine("127.0.0.1 " + "www." + updateText);
                }
                else
                {
                    w.WriteLine("127.0.0.1 " + updateText);
                }
            }
        }

        private void progs_Click(object sender, EventArgs e)
        {
            var list_form = new ListOfProcesses();
            list_form.Show();
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            var list_web = new ListOfWebsites();
            list_web.Show();
            Console.WriteLine(System.IO.File.ReadAllText(@"C:\Windows\System32\drivers\etc\host.begeba"));
            string fileText = System.IO.File.ReadAllText(@"C:\Windows\System32\drivers\etc\host.begeba");
            websites = fileText.Split(',');
            Console.WriteLine(websites.Length);
        }

        private void roundButton2_Click(object sender, EventArgs e)
        {
            //rP1.Start();
            //rP2.Start();
            //rp3.Start();
            string runningFilePath = @"C:\Windows\System32\drivers\etc\Info.begeba";
            string hostPath = @"C:\Windows\System32\drivers\etc\host.begeba";
            string filetext = System.IO.File.ReadAllText(hostPath);
            string[] websites = filetext.Split(';');
            if (onceBut == false)
            {
                System.IO.File.Create(runningFilePath).Close();
            }
            // checker om der er nogen kasser der tomme.
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // åbner en messagebox for at lade burgeren vide han har lavet en fejl
                MessageBox.Show(this, "Du skal skrive noget før du kan starte!", "ADVARSEL", MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            else
            {
                // skjuler alt bortset fra timeren.
                textBox1.Visible = false;
                textBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                roundButton1.Visible = false;
                roundButton2.Visible = false;
                progs.Visible = false;

                if (onceBut == false)
                {
                    // minder brugeren om at han skal huske at gemme alt inden han starter
                    MessageBox.Show(this, "Husk at gemme alt hvad du har åbent", "ADVARSEL", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    CreateShortcut("StudyLock",Environment.GetFolderPath(Environment.SpecialFolder.Startup),Environment.CurrentDirectory + @"\WindowsFormsApplication1.exe",Environment.CurrentDirectory);
                }
                butPressed = true;

                // starter check af processer
                StopProcesses(checkedPrograms, threads, 3, Process.GetProcesses().Length);

                if (onceBut == false)
                {
                    // definerer en ny timer og starter den.
                    timer1 = new System.Windows.Forms.Timer();
                    timer1.Tick += new EventHandler(timer1_Tick);
                    timer1.Interval = 1000; // 1 second
                    timer1.Start();
                }
                
                if(System.IO.File.Exists(@"C:\Windows\System32\drivers\etc\hosts"))
                {
                    System.IO.File.Delete(@"C:\Windows\System32\drivers\etc\hosts");
                }
                System.IO.File.Create(@"C:\Windows\System32\drivers\etc\hosts").Close();
                foreach (string s in websites)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        UpdateHostFile(s);
                    }
                }

                //rP1.Abort();
                //rP2.Abort();
                //rp3.Abort();
                onceBut = true;
                // skriver det til value label
                Values.Text = hours.ToString() + " Hours, " + minutes + " minutes";
            }

        }
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string filefolder)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.TargetPath = targetFileLocation;           // The path of the file that will launch when the shortcut is run
            shortcut.WorkingDirectory = filefolder;
            shortcut.Save();                                    // Save the shortcut
        }

        private void Host_Renamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine(string.Format("Renamed: {0} {1}", e.OldName, e.ChangeType));
            if(e.OldName == "host.begeba")
            {
                System.IO.File.Move(e.FullPath, e.OldFullPath);
            }
            if (e.OldName == "hosts")
            {
                System.IO.File.Move(e.FullPath, e.OldFullPath);
            }
            if (e.OldName == "Info.begeba")
            {
                System.IO.File.Move(e.FullPath, e.OldFullPath);
            }
            if (e.OldName == "Programs.begeba")
            {
                System.IO.File.Move(e.FullPath, e.OldFullPath);
            }
        }

        private void Host_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(string.Format("Deleted: {0} {1}", e.Name, e.ChangeType));
        }

        private void Host_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(string.Format("Changed: {0} {1}", e.Name, e.ChangeType));
        }

        private void Host_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(string.Format("Created: {0} {1}", e.Name, e.ChangeType));
        }
    }
}
