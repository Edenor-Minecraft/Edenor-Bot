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
        static readonly string SpreadsheetId = "1LJw2eAYBN_hx9z1klto758D_D-UM3vR8Rlx-YZtqN08";
        static readonly string sheet = "Ответы на форму (1)";
        static SheetsService service;

        static bool ready = false;
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

                ready = true;
            }
            catch (Exception e)
            {
                Program.logError(e.Message);
            }
        }

        public static async Task reloadInfos()
        {
            if (ready)
            {
                Program.logInfo("Refreshing discord accounts infos");
                ReadEntries();
            }
            else
            {
                Program.logWarn("GoogleSheets module not initialized!");
            }
        }

        public static void timer(object stateInfo)
        {
            reloadInfos();
        }

        static IDictionary<string, bool> discordAccountsList = new Dictionary<string, bool>();
        private static void ReadEntries()
        {
           try
           {
                discordAccountsList.Clear();
                var range = $"{sheet}!A:F";
                SpreadsheetsResource.GetRequest sheetData = service.Spreadsheets.Get(SpreadsheetId);
                sheetData.IncludeGridData = true;

                var execSheetData = sheetData.Execute();
                GridData discordColumn = null;
                if (execSheetData != null)
                {
                    foreach (var grid in execSheetData.Sheets.First().Data)
                    {
                        if (grid.RowData.First().Values[2].UserEnteredValue.StringValue == "Твой Discord (Nickname#0000 или @nickname)")
                        {
                            discordColumn = grid;
                        }
                    }
                }

                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

                var response = request.Execute();
                IList<IList<object>> values = response.Values;
                var gridRow = 1;
                if (values != null && values.Count > 0)
                {
                    foreach (var row in values)
                    {
                        if (values.IndexOf(row) == 0) continue;
                        string nick = "";
                        if (row.Count > 3)
                            nick = row[3].ToString().Trim();
                        if (gridRow > values.Count - 1) { break; }
                        bool accepted = false;
                        try
                        {
                            if (discordColumn.RowData.ElementAtOrDefault(gridRow) != null) //Giga null checking
                            {
                                if (discordColumn.RowData.ElementAtOrDefault(gridRow).Values != null && discordColumn.RowData.ElementAtOrDefault(gridRow).Values.Count > 2)
                                {
                                    if (discordColumn.RowData.ElementAtOrDefault(gridRow).Values[2].UserEnteredFormat != null)
                                    {
                                        var colorStyle = discordColumn.RowData.ElementAtOrDefault(gridRow).Values[2].UserEnteredFormat.BackgroundColorStyle;
                                        float red = colorStyle.RgbColor.Red == null ? 1 : colorStyle.RgbColor.Red.Value;
                                        float blue = colorStyle.RgbColor.Blue == null ? 1 : colorStyle.RgbColor.Blue.Value;
                                        float green = colorStyle.RgbColor.Green == null ? 1 : colorStyle.RgbColor.Green.Value;

                                        accepted = checkColor(red, blue, green);
                                    }
                                }
                            }
                        }
                        catch(Exception ex) {
                            Program.logError(ex.Message + ex.StackTrace);
                        }
                                                                
                        if (discordAccountsList.ContainsKey(nick)) {
                            gridRow += 1;
                        }
                        else
                        {
                            discordAccountsList.Add(nick, accepted);
                            gridRow += 1;
                        }   
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
            if (Math.Round(red, 1, MidpointRounding.AwayFromZero) == 0.4 
                && Math.Round(blue, 1, MidpointRounding.AwayFromZero) == 0.3 
                && Math.Round(green, 1, MidpointRounding.AwayFromZero) == 0.7)
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
                if (discordAccountsList.ContainsKey (minecraftNick))
                {
                    bool def = false;
                    return discordAccountsList.TryGetValue(minecraftNick.Trim(), out def);
                }
                return false;
            }
            catch (Exception e)
            {
                Program.logError(e.Message + e.StackTrace);
                return false;
            }
        }
    }
}
