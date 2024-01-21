using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Linq;
using Discord;

namespace Discord_Bot.handlers
{
    class GoogleSheetsHelper
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Edenor Bot";
        static readonly string SpreadsheetId = "1iD0heGU3wCvTOVrgc3U5kU4d7u2Hu3cSjaVzZzzaLC4";
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

                Program.googleTimer = new Timer(timer, new AutoResetEvent(true), 1800000, 1800000);
                reloadInfos();
            }
            catch (Exception e)
            {
                _ = Logger.logError(e.Message);
            }
        }

        public static async Task reloadInfos()
        {
            _ = Logger.logInfo("Refreshing discord accounts infos");
            ReadEntries();
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
                            if (grid.RowData.IndexOf(row) != 0)
                            {
                                var nick = normalizeNick(row.Values[3].UserEnteredValue.StringValue);
                                if (!discordAccountsList.ContainsKey(nick))
                                {
                                    if (row.Values[3].UserEnteredFormat == null || row.Values[3].UserEnteredFormat.BackgroundColorStyle == null)
                                    {
                                        ((SocketTextChannel)Program.instance.edenor.GetChannel(1121791250312478731)).SendMessageAsync("Null color style for " + nick +
                                            "\n Automatically establish that the user is not in the whitelist");
                                        discordAccountsList.Add(nick, false);
                                        continue;
                                    }
                                    else
                                        discordAccountsList.Add(nick, checkColor(row.Values[3].UserEnteredFormat.BackgroundColorStyle.RgbColor));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _ = Logger.logError(e.Message + e.StackTrace);
            }
        }

        private static string normalizeNick(string rawNick)
        {
            string nick = rawNick;

            if (rawNick == null)
            {
                nick = string.Empty; //Что тут блять произошло?
                return nick;
            }               

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
                _ = Logger.logError(e.Message + e.StackTrace);
                return false;
            }
        }
    }
}
