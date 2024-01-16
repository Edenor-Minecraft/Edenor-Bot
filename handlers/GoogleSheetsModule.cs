using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Linq;

namespace Discord_Bot.handlers
{
    class GoogleSheetsHelper
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Edenor Bot";
        static readonly string SpreadsheetId = "1iD0heGU3wCvTOVrgc3U5kU4d7u2Hu3cSjaVzZzzaLC4";
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
                _ = Program.logError(e.Message);
            }
        }

        public static async Task reloadInfos()
        {
            if (ready)
            {
                _ = Program.logInfo("Refreshing discord accounts infos");
                ReadEntries();
            }
            else
            {
                _ = Program.logWarn("GoogleSheets module not initialized!");
            }
        }

        public static void timer(object stateInfo)
        {
            _ = reloadInfos();
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
                if (execSheetData != null)
                {
                    foreach (var grid in execSheetData.Sheets.First().Data)
                    {
                        foreach (var row in grid.RowData)
                        {   
                            if (!discordAccountsList.ContainsKey(normalizeNick(row.Values[3].UserEnteredValue.StringValue)))
                                discordAccountsList.Add(normalizeNick(row.Values[3].UserEnteredValue.StringValue),
                                    row.Values[3].UserEnteredFormat.BackgroundColorStyle != null 
                                    && checkColor(row.Values[3].UserEnteredFormat.BackgroundColorStyle.RgbColor));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _ = Program.logError(e.Message + e.StackTrace);
            }
        }

        private static string normalizeNick(string rawNick)
        {
            string nick = rawNick;

            if (rawNick.ElementAt(0) == '@')
            {
                nick = rawNick.Substring(1, rawNick.Length - 1);
            }

            if (nick.Split(' ').Length > 1)
            {
                nick = nick.Split(" ")[0];
            }

            return nick;
        }

        private static bool checkColor(Google.Apis.Sheets.v4.Data.Color color)
        {
            if (Math.Round(color.Red.HasValue ? color.Red.Value : 1, 1, MidpointRounding.AwayFromZero) == 0.4
                && Math.Round(color.Blue.HasValue ? color.Blue.Value : 1, 1, MidpointRounding.AwayFromZero) == 0.3
                && Math.Round(color.Green.HasValue ? color.Green.Value : 1, 1, MidpointRounding.AwayFromZero) == 0.7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkAccepted(string discordNick)
        {
            try
            {
                if (discordAccountsList.ContainsKey(discordNick))
                {
                    bool def = false;
                    return discordAccountsList.TryGetValue(discordNick.Trim(), out def);
                }
                return false;
            }
            catch (Exception e)
            {
                _ = Program.logError(e.Message + e.StackTrace);
                return false;
            }
        }
    }
}
