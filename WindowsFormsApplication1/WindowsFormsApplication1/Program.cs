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
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static public object[] processList = new object[5000];
        static void Main(string[] args)
        {
                UserCredential credential;

            ManagementEventWatcher startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));

            startWatch.EventArrived += new EventArrivedEventHandler(StartWatch_EventArrived);
            startWatch.Start();

            
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

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1isflrahjYmtYmV07fWW7vv_TNOIaqoPeHiG1TeCHQgI";
            String range = "Ark1!B1:B999";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                int i = 0;
                using (CsvFileWriter writer = new CsvFileWriter("WriteTest.csv"))
                {
                    Console.WriteLine("Name, Major");
                    foreach (var row in values)
                    {
                        
                        
                        // Print columns A and E, which correspond to indices 0 and 4.
                        try
                        {
                            
                            processList[i] = values[i][0];
                            Console.WriteLine(processList[i]);
                            CsvRow rows = new CsvRow();
                            rows.Add(row[0].ToString());
                            writer.WriteRow(rows);
                            i++;
                        }
                        catch
                        {
                            Console.WriteLine("Out of range");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            
            using (CsvFileReader reader = new CsvFileReader("WriteTest.csv"))
            {
               // string[] processList;
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                           
                    }
                    //Console.WriteLine();
                }
                
            }
            var main_form = new Form1();
            main_form.Show();
            Application.Run();
            
                if (main_form.killProg())
                {
                    Application.Exit();
                }
        }
        static void StartWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string process = e.NewEvent.Properties["ProcessName"].Value.ToString();
            // Console.WriteLine("Process opened: " + sender);
            string[] rProcess = Regex.Split(process, ".exe");
            for (int i = 0; i < rProcess.Length; i++)
            {
                if (rProcess[i].ToLower() != ".exe" && string.IsNullOrWhiteSpace(rProcess[i]) == false)
                {
                    Console.WriteLine(rProcess[i]);
                    Thread.Sleep(1000);
                    Form1.StopProcesses(Program.processList, rProcess[i]);
                }
            }   
        }
    }
}