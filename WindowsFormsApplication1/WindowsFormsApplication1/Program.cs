using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WindowsFormsApplication1;
using System.Windows.Forms;
using ReadWriteCsv;
using System.Management;
namespace SheetsQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        // Genererer en ny array, med objecter til processlisten  
        // *husk at gøre arrayen større hvis der er brug for mere plads*
        static public object[] processList = new object[5000];

        static public object[] gamesList = new object[5000];

        static public object[] checkedState = new object[5000];

        // Main start
        static void Main(string[] args)
        {
            // Gemmer mine user credentials så man kan logge ind
            UserCredential credential;

            // Eventviewer der checker hvornår en process den starter.
            ManagementEventWatcher startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));

            // subscriber startwatch til StartWatch_EventArrived metoden.
            startWatch.EventArrived += new EventArrivedEventHandler(StartWatch_EventArrived);
            // Starter startwatch
            startWatch.Start();

            // googles API: gemmer mine credentials i en Json fil
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

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
            String spreadsheetId = "1decwKsk9kd8FqJP5nAVwAcKoaUYvwe9DFAZNEE5gjPQ";
            String getRange = "Ark1!A2:C999";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, getRange);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            // checker om den celle den der i ikke er tom
            if (values != null && values.Count > 0)
            {
                int i = 0;
                // laver en todimensionelle liste om til en enkelt dimensionel array
                foreach (var row in values)
                {
                    try
                    {
                        checkedState[i] = values[i][2]; 
                        processList[i] = values[i][1];
                        gamesList[i] = values[i][0];
                        Console.WriteLine(processList[i]);       
                        i++;
                    }
                    catch(Exception c)
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

        public static void UpdateSheet(List<object> check)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

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
            String range2 = "Ark1!C2:C999";  // Hele C rækken
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
    }
}