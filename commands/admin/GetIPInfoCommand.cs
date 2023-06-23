using Discord_Bot.handlers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.commands.admin
{
    internal class GetIPInfoCommand : BaseCommandClass
    {
        public GetIPInfoCommand() {
            var getIPInfo = new SlashCommandBuilder();
            locale.Add("ru", "получитьинфуоip");
            getIPInfo.WithNameLocalizations(locale);
            getIPInfo.WithName("getipinfo");
            getIPInfo.WithDefaultMemberPermissions(GuildPermission.Administrator);
            getIPInfo.AddOption("айпи", ApplicationCommandOptionType.String, "Айпи, информацию о котором надо получить", true);
            getIPInfo.WithDescription("Получает информацию об IP адрессе");
            locale.Clear();

            commandProperties = getIPInfo.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        static string baseURl = $"http://ip-api.com/json/";
        static string responseParams = "?fields=status,message,continent,continentCode,country,countryCode,region,regionName,city,district,zip,lat,lon,timezone,offset,currency,isp,org,asname,reverse,mobile,proxy,hosting,query&lang=ru";

        public static HttpClient client = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            UseCookies = false,
            UseProxy = false
        });

        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != commandProperties.Name.Value) return;

            string ip = command.Data.Options.ToList()[0].Value.ToString();

            await command.ModifyOriginalResponseAsync(x => {
                x.Content = $"Собираем информацию об IP {ip}";
            });

            await command.ModifyOriginalResponseAsync(x => {
                x.Content = doTask(ip).Result;
            });
        }

        static async Task<string> doTask(string ip)
        {
            string responseAdress = baseURl+ ip + responseParams;

            var response = await client.SendAsync(new HttpRequestMessage { 
                RequestUri = new Uri(responseAdress),
                Method = HttpMethod.Get
            });

            var respJSON = JsonSerializer.Deserialize<ResponseJson>(await response.Content.ReadAsStringAsync());

            if (respJSON != null)
            {
                return $"Айпи: {respJSON.query}" +
                    $"\nСтатус: {respJSON.status}" +
                    $"\nКонтинент: {respJSON.continent}" +
                    $"\nКод континета: {respJSON.continentCode}" +
                    $"\nСтрана: {respJSON.country}" +
                    $"\nКод страны: {respJSON.countryCode}" +
                    $"\nРегион: {respJSON.region}" +
                    $"\nНазвание региона: {respJSON.regionName}" +
                    $"\nГород: {respJSON.city}" +
                    $"\nРайон: {respJSON.district}" +
                    $"\nИндекс: {respJSON.zip}" +
                    $"\nШирота: {respJSON.lat}" +
                    $"\nДолгота: {respJSON.lon}" +
                    $"\nЧасовой пояс: {respJSON.timezone}" +
                    $"\nСмещение часового пояса в секундах: {respJSON.offset}" +
                    $"\nВалюта: {respJSON.currency}" +
                    $"\nИмя провайдера: {respJSON.isp}" +
                    $"\nИмя организации: {respJSON.org}" +
                    $"\nОбратный DNS: {respJSON.reverse}" +
                    $"\nМобильная связь: {respJSON.mobile}" +
                    $"\nХостинг: {respJSON.hosting}" +
                    $"\nИспользует прокси: {respJSON.proxy}";
            }
            else
            {
                return "Не удалось выполнить запрос!";
            }
        }
    }

    internal class ResponseJson
    {
        public string query { get; set; }
        public string status { get; set; }
        public string continent { get; set; }
        public string continentCode { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string zip { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string timezone { get; set; }
        public int offset  { get; set; }
        public string currency { get; set; }
        public string isp { get; set; }
        public string org { get; set; }      
        public string asname { get; set; }
        public string reverse { get; set; }
        public bool mobile { get; set; }
        public bool proxy { get; set; }
        public bool hosting { get; set; }
    }
}
