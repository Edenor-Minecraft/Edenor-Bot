namespace Discord_Bot.handlers
{
    internal class TicketHandler
    {
       internal static async Task onNewThread(SocketThreadChannel thread)
       {
            foreach (var message in thread.CachedMessages)
            {
                if (message.Author.Id == 710401785663193158)
                    return;
            }
            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = new Color(0, 255, 255);
            embed.Description = $"Напишите жалобу по форме снизу.\nВашу жалобу скоро проверят!\nЧтобы закрыть жалобу нажмите на кнопку{new Emoji("\uD83D\uDD12")}";
            embed.Title = "Панель управления жалобой";
            embed.AddField(new EmbedFieldBuilder() { Name = "1) Ваш игровой Никнейм", Value = "Ваш ник на сервере, всё просто <3" });
            embed.AddField(new EmbedFieldBuilder() { Name = "2) Никнейм нарушителя", Value = "Ник нарушителя в Дискорде или на сервере" });
            embed.AddField(new EmbedFieldBuilder() { Name = "3) В чем его нарушение. И где это произошло (Сервер или ДС)", Value = "Здесь указывается причина подачи жалобы в кратком формате и соответственно место, где это произошло (может включать в себя беседу или группу в вк)" });
            embed.AddField(new EmbedFieldBuilder() { Name = "4) Подробное описание проблемы и комментарии", Value = "Здесь требуется подробное описание того, что нарушил игрок. Если были нарушены правила вашего города или вашей постройки, укажите это." });
            embed.AddField(new EmbedFieldBuilder() { Name = "5) Док-ва нарушения", Value = "В виду некоторых ограничений Дискорда, загружайте видеодоказательства на ютуб и прикрепляйте ссылку. Для скриншотов можно создать архив." });
            var bttn = new ButtonBuilder()
            {
                Label = $"{new Emoji("\uD83D\uDD12")} Закрыть",
                CustomId = "close_ticket_bttn",
                Style = ButtonStyle.Danger
            };

            thread.SendMessageAsync(null, embed: embed.Build(), components: new ComponentBuilder().WithButton(bttn).Build());
        }
    }
}
