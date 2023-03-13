using Discord;
using Discord.Interactions;

namespace Discord_Bot.handlers
{
    class ButtonsHandler
    {
        public static async Task onButton(SocketMessageComponent component)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(new Color(0, 255, 255));
            var author = new EmbedAuthorBuilder();
            author.Name = "Эденор";
            author.IconUrl = "https://sun7-16.userapi.com/impg/Rjb4mZYT_1Je6qzdgGm3gvCNDLCTmuCLj3qxQA/gJsPNJDwC2Y.jpg?size=1000x1000&quality=96&sign=a3c8cd1edbf9776f4fe989dea40534dd&type=album";
            author.Url = "https://edenor.ru/";
            embed.WithAuthor(author);
            switch (component.Data.CustomId)
            {
                case "accept_srvaccess": 
                    embed.Title = "Здравствуйте, ваша заявка на сервер была одобрена!";
                    embed.Description = "Советую ознакомиться с правилами на сайте! \nАйпи сервера: \nJava - mc.edenor.ru или play.edenor.ru \nBedrock - pe.edenor.ru(порт 25565)"; 
                    var footer = new EmbedFooterBuilder();
                    footer.Text = "Приятной игры!";
                    footer.IconUrl = "https://sun7-16.userapi.com/impg/Rjb4mZYT_1Je6qzdgGm3gvCNDLCTmuCLj3qxQA/gJsPNJDwC2Y.jpg?size=1000x1000&quality=96&sign=a3c8cd1edbf9776f4fe989dea40534dd&type=album";
                    embed.WithFooter(footer);
                    var minecraftNick = "";
                    foreach (var embed1 in component.Message.Embeds)
                    {
                        foreach (var field in embed1.Fields)
                        {
                            if (field.Name == "Ник в Minecraft")
                                minecraftNick = field.Value;
                        }
                    }
                    if (minecraftNick == "") return;
                    var msgEmbed = component.Message.Embeds.FirstOrDefault().ToEmbedBuilder();
                    msgEmbed.Color = new Color(124, 252, 0);
                    component.UpdateAsync(msg =>
                    {
                        msg.Embed = msgEmbed.Build();
                        msg.Components = null;
                    });
                    Program.instance.rcon.SendCommand($"easywhitelist add {minecraftNick}");
                    Program.instance.edenor.GetUser(component.Message.MentionedUsers.First().Id).SendMessageAsync(embed: embed.Build());
                    break;

                case "decline_srvaccess":
                    embed.Title = "Здравствуйте, ваша заявка на сервер была отлонена!";
                    embed.Description = "Вы можете купить проходу на сервер на нашем сайте https://edenor.ru/";
                    var msgEmbed1 = component.Message.Embeds.FirstOrDefault().ToEmbedBuilder();
                    msgEmbed1.Color = new Color(220, 20, 60);
                    component.UpdateAsync(msg =>
                    {
                        msg.Embed = msgEmbed1.Build();
                        msg.Components = null;
                    });
                    Program.instance.edenor.GetUser(component.Message.MentionedUsers.First().Id).SendMessageAsync(embed: embed.Build());
                    break;
                case "ban_form_btn":
                    var mb2 = new ModalBuilder()
                    .WithTitle("Форма выдачи наказания")
                    .WithCustomId("ban_form")
                    .AddTextInput("Ник нарушителя", "ban_form_nick", placeholder: "Steve")
                    .AddTextInput("Место (дискорд, вк, майнкрафт, тг)", "ban_form_where", placeholder: "Дискорд")
                    .AddTextInput("Причина", "ban_form_reason", placeholder: "2.4")
                    .AddTextInput("Док-ва (ссылками)", "ban_form_proofs", placeholder: "Something");
                    component.RespondWithModalAsync(mb2.Build());
                    break;
                case "srvaccess_btn":
                    var mb1 = new ModalBuilder()
                    .WithTitle("Подача заявки на сервер")
                    .WithCustomId("srv_access")
                    .AddTextInput("Ссылка на ваш вк", "srv_access_vk", placeholder: "https://vk.com/feed")
                    .AddTextInput("Ваш ник в Minecraft", "srv_access_nick", placeholder: "Steve")
                    .AddTextInput("Ваш возраст", "srv_access_age", placeholder: "14")
                    .AddTextInput("Откуда вы узнали про сервер?", "srv_access_how", placeholder: "Нашёл")
                    .AddTextInput("Прочёл правила?", "srv_access_read_rules", placeholder: "Нет");
                    component.RespondWithModalAsync(mb1.Build());
                    break;
                case "selfBanButton":
                    if (component.Message.MentionedUsers.First().Id == component.User.Id)
                    {
                        ModerationFunctions.banUser(component.User, 0, "Самобан");
                        component.UpdateAsync(msg =>
                        {
                            msg.Content = $"Пользователь {component.User.Username} успешно забанен!";
                            msg.Components = null;
                        });
                    }                 
                    break;
                case "pp_button":
                    var mb = new ModalBuilder()
                    .WithTitle("Получение роли игрока приватки")
                    .WithCustomId("pp_role")
                    .AddTextInput("Ваш ник в Minecraft", "minecraft_nick", placeholder: "Steve")
                    .AddTextInput("Покупная или по заявке", "paid_or_free", placeholder: "Покупная/По заявке")
                    .AddTextInput("Ваш вк (если есть)", "vk", placeholder: "https://vk.com/feed");
                    component.RespondWithModalAsync(mb.Build());
                    break;
            }
        }
    }
}
