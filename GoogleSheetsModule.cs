using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

namespace Discord_Bot
{
    class GoogleSheetsHelper
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Edenor Bot";
        static readonly string SpreadsheetId = "1XYpSVHGBwe4PMlp1sWZBFW2ciIORqUjhxAAlqymicj4";
        static readonly string sheet = "Ответы на форму (1)";
        static SheetsService service;
        public static void setupHelper()
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(Scopes);
                }

                // Create Google Sheets API service.
                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception e)
            {
                Program.logError(e.Message);
            }
        }

        public static void timer(object stateInfo)
        {
            Program.logInfo("Refreshed discord accounts infos");
            ReadEntries();
        }

        static IDictionary<string, bool> discordAccountsList = new Dictionary<string, bool>();
        private static void ReadEntries()
        {
           try
           {
                discordAccountsList.Clear();
                var range = $"{sheet}!A:F";
                SpreadsheetsResource.GetRequest sheetData =
                        service.Spreadsheets.Get(SpreadsheetId);
                sheetData.IncludeGridData = true;

                var execSheetData = sheetData.Execute();
                GridData discordColumn = null;
                if (execSheetData != null)
                {
                    foreach (var grid in execSheetData.Sheets.First().Data)
                    {
                        if (grid.RowData.First().Values[2].UserEnteredValue.StringValue == "Твой Discord id (ник # 0000)")
                        {
                            discordColumn = grid;
                        }
                    }
                }

                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(SpreadsheetId, range);

                var response = request.Execute();
                IList<IList<object>> values = response.Values;
                var gridRow = 1;
                if (values != null && values.Count > 0)
                {
                    foreach (var row in values)
                    {
                        string nick = "";
                        if (row.Count > 3)
                            nick = row[3].ToString();
                        if (gridRow > values.Count - 1) { break; }
                        bool accepted = false;
                        if (discordColumn.RowData.ElementAt(gridRow) != null) //Giga null checking
                        {
                            if (discordColumn.RowData.ElementAt(gridRow).Values != null && discordColumn.RowData.ElementAt(gridRow).Values.Count > 2)
                            {
                                if (discordColumn.RowData.ElementAt(gridRow).Values[2].UserEnteredFormat != null)
                                {
                                    accepted = checkColor(discordColumn.RowData.ElementAt(gridRow).Values[2].UserEnteredFormat.BackgroundColorStyle.RgbColor.Red.GetValueOrDefault(1), discordColumn.RowData.ElementAt(gridRow).Values[2].UserEnteredFormat.BackgroundColorStyle.RgbColor.Blue.GetValueOrDefault(1), discordColumn.RowData.ElementAt(gridRow).Values[2].UserEnteredFormat.BackgroundColorStyle.RgbColor.Green.GetValueOrDefault(1));
                                }
                            }                           
                        }                                         
                        if (discordAccountsList.ContainsKey(nick)) {gridRow += 1; continue; }

                        discordAccountsList.Add(nick, accepted);
                        gridRow += 1;
                    }
                }
                else
                {
                    Program.logError("No data found.");
                }
           }
           catch(Exception e)
           {
               Program.logError(e.Message + e.StackTrace);
           }
        }

        private static Boolean checkColor(float red, float blue, float green)
        {
            if (Math.Round(red, 1, MidpointRounding.AwayFromZero) == 0.4 && Math.Round(blue, 1, MidpointRounding.AwayFromZero) == 0.3 && Math.Round(green, 1, MidpointRounding.AwayFromZero) == 0.7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean checkAccepted(string minecraftNick)
        {
            try
            {
                return discordAccountsList[minecraftNick];
            }
            catch (Exception e)
            {
                Program.logError(e.Message);
                return false;
            }
        }
    }
}
