using Discord;
using System.Reflection.Emit;

namespace Discord_Bot.handlers
{
    class ModalsHandler
    {
        public static async Task onModal(SocketModal modal)
        {
            List<SocketMessageComponentData> components = modal.Data.Components.ToList();

            switch (modal.Data.CustomId)
            {
                case "srv_access":
                    var srvAccessEmbed = new EmbedBuilder();
                    srvAccessEmbed.WithColor(new Color(0, 255, 255));
                    srvAccessEmbed.Title = $"Новая заявка на сервер {DateTime.Now:yyyy-MM-dd HH:mm:ss}!";
                    srvAccessEmbed.AddField(new EmbedFieldBuilder().WithName("Ник в Minecraft").WithValue(components.First(x => x.CustomId == "srv_access_nick").Value));
                    srvAccessEmbed.AddField(new EmbedFieldBuilder().WithName("Возраст").WithValue(components.First(x => x.CustomId == "srv_access_age").Value));
                    srvAccessEmbed.AddField(new EmbedFieldBuilder().WithName("Откуда узнал про сервер").WithValue(components.First(x => x.CustomId == "srv_access_how").Value));
                    srvAccessEmbed.AddField(new EmbedFieldBuilder().WithName("Прочёл правила?").WithValue(components.First(x => x.CustomId == "srv_access_read_rules").Value));
                    srvAccessEmbed.AddField(new EmbedFieldBuilder().WithName("Вк").WithValue(components.First(x => x.CustomId == "srv_access_vk").Value));
                    srvAccessEmbed.AddField(new EmbedFieldBuilder().WithName("Дискорд").WithValue(modal.User.Mention));
                    var accept = new ButtonBuilder();
                    accept.CustomId = "accept_srvaccess";
                    accept.Label = "Принять";
                    accept.Style = ButtonStyle.Success;

                    var decline = new ButtonBuilder();
                    decline.CustomId = "decline_srvaccess";
                    decline.Label = "Отклонить";
                    decline.Style = ButtonStyle.Danger;

                    ((SocketTextChannel)Program.instance.edenor.GetChannel(1083416534736711680)).SendMessageAsync(modal.User.Mention, embed: srvAccessEmbed.Build(), components: new ComponentBuilder().WithButton(accept).WithButton(decline).Build());
                    modal.RespondAsync(embed: srvAccessEmbed.Build(), ephemeral: true);
                    break;
                case "pp_role_by_request": case "pp_role_purchased":
                    var ppEmbed = new EmbedBuilder();
                    ppEmbed.WithTitle("Получение роли И.П");
                    ppEmbed.AddField("**Ваш ник в Minecraft**", components.First(x => x.CustomId == "minecraft_nick").Value, true);
                    if (modal.Data.CustomId == "pp_role_by_request")
                    {
                        ppEmbed.AddField("**Покупная или по заявке**", "По заявке", true);
                    }
                    else
                    {
                        ppEmbed.AddField("**Покупная или по заявке**", "Покупная", true);
                    }
                    ppEmbed.AddField("**Ваш вк (если есть)**", components.First(x => x.CustomId == "vk").Value, true);
                    ppEmbed.WithFooter(new EmbedFooterBuilder().WithText(modal.User.Id.ToString()));
                    ppEmbed.WithCurrentTimestamp();
                    ppEmbed.WithColor(new Color(0, 255, 255));
                   
                    var author = new EmbedAuthorBuilder();
                    author.Name = modal.User.Username + "#" + modal.User.Discriminator;
                    author.IconUrl = modal.User.GetAvatarUrl();

                    ppEmbed.WithAuthor(author); 
                   
                    var msg = await ((SocketTextChannel)Program.instance.edenor.GetChannel(1055783105916571658)).SendMessageAsync(modal.User.Mention, embed: ppEmbed.Build());

                    if (modal.Data.CustomId == "pp_role_by_request")
                    {
                        if (GoogleSheetsHelper.checkAccepted(components.First(x => x.CustomId == "minecraft_nick").Value))
                        {
                            Program.instance.edenor.GetUser(Convert.ToUInt64(modal.User.Id)).AddRoleAsync(802248363503648899);
                            msg.AddReactionAsync(new Emoji("\u2705"));
                        }
                        else
                        {
                            msg.AddReactionAsync(new Emoji("\u274C"));
                        }
                    }               

                    modal.RespondAsync(embed: ppEmbed.Build(), ephemeral: true);
                    break;
            }
        }
    }
}
