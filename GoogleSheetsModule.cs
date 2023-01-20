﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;

namespace Discord_Bot
{
    class GoogleSheetsHelper
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Current Legislators";
        static readonly string SpreadsheetId = "1XYpSVHGBwe4PMlp1sWZBFW2ciIORqUjhxAAlqymicj4";
        static readonly string sheet = "Ответы на форму (1)";
        static SheetsService service;
        public static void setupHelper()
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

        public static void timer(object stateInfo)
        {
            Program.instance.logTrace("Refreshed discord accounts infos");
            ReadEntries();
        }

        static IDictionary<string, bool> discordAccountsList = new Dictionary<string, bool>();
        static IDictionary<string, string> minecraftAccountsList = new Dictionary<string, string>();
        private static void ReadEntries()
        {
            discordAccountsList.Clear();
            minecraftAccountsList.Clear();
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
                    if (gridRow > values.Count - 1) { break; }
                    bool accepted = false;
                    if (discordColumn.RowData.ElementAt(gridRow).Values[2].UserEnteredFormat.BackgroundColorStyle != null)
                    {
                        if (discordColumn.RowData.ElementAt(gridRow).Values.First().UserEnteredFormat.BackgroundColorStyle.RgbColor.Blue == 0.30980393
                            && discordColumn.RowData.ElementAt(gridRow).Values.First().UserEnteredFormat.BackgroundColorStyle.RgbColor.Green == 0.65882355
                            && discordColumn.RowData.ElementAt(gridRow).Values.First().UserEnteredFormat.BackgroundColorStyle.RgbColor.Red == 0.41568628)
                        {
                            accepted = true;
                        }
                    }
                    try { if (discordAccountsList.ContainsKey(row[2].ToString())) { gridRow += 1; continue; } }catch(Exception e) { gridRow += 1; Program.instance.logTrace(e.Message); continue; }

                    discordAccountsList.Add(row[2].ToString(), accepted);
                    minecraftAccountsList.Add(row[2].ToString(), row[3].ToString());
                }
            }
            else
            {
                Program.instance.logTrace("No data found.");
            }
        }

        public static Boolean checkAccepted(string discordNick, string minecraftNick)
        {
            try
            {
                return discordAccountsList[discordNick] && minecraftAccountsList[discordNick].Equals(minecraftNick);
            }
            catch (Exception e)
            {
                Program.instance.logTrace(e.Message);
                return false;
            }
        }
    }
}
