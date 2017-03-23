using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApplication1;
using System.Windows.Forms;

namespace SheetsQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static void Main(string[] args)

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

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1isflrahjYmtYmV07fWW7vv_TNOIaqoPeHiG1TeCHQgI";
            String range = "Ark1!A1:A999";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            StringBuilder sb = new StringBuilder();
            string delimiter = ",";
            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    try
                    {
                        // Console.WriteLine("{0}, {1}", row[0], row[1]);
                        Console.WriteLine(row[0]);
                        sb.AppendLine(string.Join(delimiter, row[0]));
                    }
                    catch
                    {
                        Console.WriteLine("Out of range");
                    }
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            string filePath = @"C:\Users\Mc IceBerg\AppData\Roaming\StudyLock\Process.csv";
            File.WriteAllText(filePath, sb.ToString());
            var main_form = new Form1();
            main_form.Show();
            Application.Run();
            if(main_form.killProg())
            {
                Application.Exit();
            }
        }
    }
}