using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using WindowsFormsApplication1;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Management;
namespace SheetsQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static int increment = 2;

        public static bool isElevated;

        static string path = @"C:\Windows\System32\drivers\etc\hosts";
        static string programPath = @"C:\Windows\System32\drivers\etc\Programs.begeba";

        static string[][] values = new string[600][];
        // Genererer en ny array, med objecter til processlisten  
        // *husk at gøre arrayen større hvis der er brug for mere plads*
        static public object[] processList = new object[5000];

        static public object[] gamesList = new object[5000];

        static public object[] checkedState = new object[5000];

        static public int range = 0;
        // Main start
        static void Main(string[] args)
        {
            if (!checkForAdmin())
            {
                string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                string openPath = currentPath + @"\WindowsFormsApplication1.exe";
                //MessageBox.Show(openPath, "ADVARSEL", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                
                const int ERROR_CANCELLED = 1223; //The operation was canceled by the user.

                ProcessStartInfo info = new ProcessStartInfo(openPath);
                info.UseShellExecute = true;
                info.Verb = "runas";
                again:
                try
                {
                    Process.Start(info);
                }
                catch (Win32Exception ex)
                {
                    if(ex.NativeErrorCode == ERROR_CANCELLED)
                    {
                        goto again;
                    }
                }
                Thread.Sleep(2000);
                Environment.Exit(1);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            if (!File.Exists(programPath))
            {
                File.Create(programPath).Close();
            }
            // Eventviewer der checker hvornår en process den starter.
            ManagementEventWatcher startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            if (IsMachineUp("www.google.com"))
            {
                if (!String.IsNullOrEmpty(File.ReadAllText(programPath)))
                {
                    File.Delete(programPath);
                    Thread.Sleep(100);
                    File.Create(programPath).Close();
                }
                GetSheet("Ark1!A2:C999", "1decwKsk9kd8FqJP5nAVwAcKoaUYvwe9DFAZNEE5gjPQ");
            }
            else
            {
                if (String.IsNullOrEmpty(File.ReadAllText(programPath)))
                {
                    MessageBox.Show("Du skal have forbindelse til nettet første gang du åber programmet", "ADVARSEL!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else
                {
                    string[] fileText = File.ReadAllText(programPath).Split(';');
                    for (int i = 0; i < fileText.Length; i++)
                    {
                        values[i] = fileText[i].Split(',');
                    }
                    for (int i = 0; i < fileText.Length; i++)
                    {
                        int h = 0;
                        foreach (string j in values[i])
                        {
                            if(h == 0)
                            {
                                if(!String.IsNullOrEmpty(values[i][h]))
                                {
                                    gamesList[i] = values[i][h];
                                }
                            }
                            if(h == 1)
                            {
                                if (!String.IsNullOrEmpty(values[i][h]))
                                {
                                    processList[i] = values[i][h];
                                }
                            }
                            if(h == 2)
                            {
                                if (!String.IsNullOrEmpty(values[i][h]))
                                {
                                    checkedState[i] = values[i][h];
                                }

                            }
                            Console.WriteLine(i + "nr " + "string " + j);
                            h++;
                        }
                    }
                }
            }
            // subscriber startwatch til StartWatch_EventArrived metoden.
            startWatch.EventArrived += new EventArrivedEventHandler(StartWatch_EventArrived);
            // Starter startwatch
            startWatch.Start();

            // sørger for at programmet bliver åbnet efter at den har fundet data.
            var main_form = new Form1();
            main_form.Show();
            Application.Run();
            
            // sørger for at programmet bliver lukket når det skal og forblive åbent når det skal
            if (main_form.killProg())
            {
                Application.Exit();
            }
        }

        public static void UpdateSheet(List<object> check, string UpdateArea)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Definere det den skal opdatere i vores sheet
            String spreadsheetId2 = "1decwKsk9kd8FqJP5nAVwAcKoaUYvwe9DFAZNEE5gjPQ";
            String range2 = UpdateArea;  // Hele C rækken
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "COLUMNS";


            var oblistcheck = check;
            
            valueRange.Values = new List<IList<object>> { oblistcheck };

            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, spreadsheetId2, range2);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            UpdateValuesResponse result2 = update.Execute();

            Console.WriteLine("done!");
        }

        // StartWatch_EventArrived metoden som bliver brugt til at definere hvilken process der er blevet åbnet.
        static void StartWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (Form1.butPressed == true)
            {
                // Finder hvilken process der er blevet åbnet.
                string process = e.NewEvent.Properties["ProcessName"].Value.ToString();

                // splitter process navnet fra .exe så vi ikke får det med i navnet.
                string[] rProcess = Regex.Split(process, ".exe");

                // for loop der sørger for at vi får det rigtige navn ud af det.
                for (int i = 0; i < rProcess.Length; i++)
                {
                    if (rProcess[i].ToLower() != ".exe" && string.IsNullOrWhiteSpace(rProcess[i]) == false)
                    {
                        Console.WriteLine(rProcess[i]);
                        Thread.Sleep(1000);
                        // kører overload metoden af stopProcesses.
                        Form1.StopProcesses(Program.processList, rProcess[i]);
                    }
                }
            }  
        }


        public static void GetSheet(string getArea, string spreadsheetid)
        {

            UserCredential credential;
            using (var stream =
            new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // færdig med at gemme

            // Laver Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Definerer hvad den skal kigge efter i vores sheet
            String spreadsheetId = spreadsheetid;
            String getRange = getArea;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, getRange);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            // checker om den celle den der i ikke er tom
            if (values != null && values.Count > 0)
            {
                int i = 0;
                range = values.Count;
                // laver en todimensionelle liste om til en enkelt dimensionel array
                foreach (var row in values)
                {
                    try
                    {
                        checkedState[i] = values[i][2];
                        processList[i] = values[i][1];
                        gamesList[i] = values[i][0];
                        Console.WriteLine(processList[i]);

                        File.AppendAllText(programPath, values[i][0] + "," + values[i][1] + "," + values[i][2] + ";");

                        i++;
                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(c.Message);
                    }
                }
            }
            // udskriver hvis der er ikke er noget data i det sheet der er blevet defineret
            else
            {
                Console.WriteLine("No data found.");
            }
        }


        private static bool IsMachineUp(string hostName)
        {
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(hostName, 3000);
                if(reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        static public bool checkForAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return isElevated;
        }
    }
}