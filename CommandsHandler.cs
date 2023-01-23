namespace Discord_Bot
{
    class CommandsHandler
    {
        public static async Task setupCommands()
        {
            List<ApplicationCommandProperties> applicationCommandProperties = new();
            IDictionary<string, string> locale = new Dictionary<string, string>();
            try
            {
                var globalCommandSetNumber = new SlashCommandBuilder();
                locale.Add("ru", "установитьчисло");
                globalCommandSetNumber.WithName("setnumber");
                globalCommandSetNumber.WithNameLocalizations(locale);
                globalCommandSetNumber.WithDescription("Устанавливает число, с которого начнётся отсчёт");
                globalCommandSetNumber.WithDefaultMemberPermissions(GuildPermission.Administrator);
                globalCommandSetNumber.AddOption("number", ApplicationCommandOptionType.Integer, "Число, с которого начнётся отстчёт", true);
                locale.Clear();
                applicationCommandProperties.Add(globalCommandSetNumber.Build());

                var globalCommandBan = new SlashCommandBuilder();
                locale.Add("ru", "бан");
                globalCommandBan.WithName("ban");
                globalCommandBan.WithNameLocalizations(locale);
                globalCommandBan.WithDescription("Банит участника сервера");
                globalCommandBan.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                globalCommandBan.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет забанен", true);
                globalCommandBan.AddOption("days", ApplicationCommandOptionType.Integer, "Количество дней для удаления сообщений от этого пользователя", true);
                globalCommandBan.AddOption("reason", ApplicationCommandOptionType.String, "Причина бана", true);
                locale.Clear();
                applicationCommandProperties.Add(globalCommandBan.Build());

                var globalCommandunBan = new SlashCommandBuilder();
                locale.Add("ru", "разбан");
                globalCommandunBan.WithName("unban");
                globalCommandunBan.WithNameLocalizations(locale);
                globalCommandunBan.WithDescription("Разбанивает человека");
                globalCommandunBan.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                globalCommandunBan.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет разбанен", true);
                globalCommandunBan.AddOption("reason", ApplicationCommandOptionType.String, "Причина разбана", true);
                locale.Clear();
                applicationCommandProperties.Add(globalCommandunBan.Build());

                var globalCommandKick = new SlashCommandBuilder();
                locale.Add("ru", "кик");
                globalCommandKick.WithName("kick");
                globalCommandKick.WithNameLocalizations(locale);
                globalCommandKick.WithDescription("Выгоняет участника сервера");
                globalCommandKick.WithDefaultMemberPermissions(GuildPermission.KickMembers);
                globalCommandKick.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет выгнан", true);
                globalCommandKick.AddOption("reason", ApplicationCommandOptionType.String, "Причина кика", true);
                locale.Clear();
                applicationCommandProperties.Add(globalCommandKick.Build());

                var globalCommandTimeOut = new SlashCommandBuilder();
                locale.Add("ru", "таймаут");
                globalCommandTimeOut.WithName("timeout");
                globalCommandTimeOut.WithNameLocalizations(locale);
                globalCommandTimeOut.WithDescription("Отправляет пользователя подумать о певедении");
                globalCommandTimeOut.WithDefaultMemberPermissions(GuildPermission.ModerateMembers);
                globalCommandTimeOut.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет отправлен думать о своём поведении", true);
                globalCommandTimeOut.AddOption("time", ApplicationCommandOptionType.String, "Время тайм-аута плюс (s, m, h, d) в конце", true);
                globalCommandTimeOut.AddOption("reason", ApplicationCommandOptionType.String, "Причина выдачи тайм-аута", true);
                locale.Clear();
                applicationCommandProperties.Add(globalCommandTimeOut.Build());

                var globalCommandUnTimeOut = new SlashCommandBuilder();
                locale.Add("ru", "убратьтаймаут");
                globalCommandUnTimeOut.WithName("untimeout");
                globalCommandUnTimeOut.WithNameLocalizations(locale);
                globalCommandUnTimeOut.WithDescription("Убирает тайм-аут с пользователя");
                globalCommandUnTimeOut.WithDefaultMemberPermissions(GuildPermission.ModerateMembers);
                globalCommandUnTimeOut.AddOption("user", ApplicationCommandOptionType.User, "Пользователь, с которого надо убрать тайм-аут", true);
                globalCommandUnTimeOut.AddOption("reason", ApplicationCommandOptionType.String, "Причина снятия тайм-аута", true);
                locale.Clear();
                applicationCommandProperties.Add(globalCommandUnTimeOut.Build());

                var secretCommand = new SlashCommandBuilder();
                locale.Add("ru", "ошибка");
                secretCommand.WithName("error");
                secretCommand.WithNameLocalizations(locale);
                secretCommand.WithDescription("Превращает человека в ошибку");
                secretCommand.WithDefaultMemberPermissions(GuildPermission.Administrator);
                secretCommand.AddOption("take", ApplicationCommandOptionType.Boolean, "Забрать роль?", true);
                locale.Clear();
                //applicationCommandProperties.Add(secretCommand.Build());

                var giveRole = new SlashCommandBuilder();
                locale.Add("ru", "датьроль");
                giveRole.WithName("giverole");
                giveRole.WithNameLocalizations(locale);
                giveRole.WithDescription("Изменить роль человека");
                giveRole.WithDefaultMemberPermissions(GuildPermission.Administrator);
                giveRole.AddOption("user", ApplicationCommandOptionType.User, "Человек, которому надо дать роль", true);
                giveRole.AddOption("role", ApplicationCommandOptionType.Role, "Роль, которую надо выдать", true);
                giveRole.AddOption("take", ApplicationCommandOptionType.Boolean, "Забрать роль?", true);
                locale.Clear();
                applicationCommandProperties.Add(giveRole.Build());

                var selfban = new SlashCommandBuilder();
                locale.Add("ru", "самобан");
                selfban.WithName("selfban");
                selfban.WithNameLocalizations(locale);
                selfban.WithDescription("Банит вас!");
                locale.Clear();
                applicationCommandProperties.Add(selfban.Build());

                var gay = new SlashCommandBuilder();
                locale.Add("ru", "гей");
                gay.WithName("gay");
                gay.WithNameLocalizations(locale);
                gay.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                gay.WithDescription("Присылает гея!");
                locale.Clear();
                applicationCommandProperties.Add(gay.Build());

                var rollback = new SlashCommandBuilder();
                locale.Add("ru", "откат");
                rollback.WithName("rollback");
                rollback.WithNameLocalizations(locale);
                rollback.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                rollback.WithDescription("Откат рп!");
                locale.Clear();
                applicationCommandProperties.Add(rollback.Build());

                var whore = new SlashCommandBuilder();
                locale.Add("ru", "шлюха");
                whore.WithName("whore");
                whore.WithNameLocalizations(locale);
                whore.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                whore.WithDescription("Шлёт шлюху!");
                locale.Clear();
                applicationCommandProperties.Add(whore.Build());

                var dontLove = new SlashCommandBuilder();
                locale.Add("ru", "нелюблю");
                dontLove.WithName("dontlove");
                dontLove.WithNameLocalizations(locale);
                dontLove.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                dontLove.WithDescription("Шлёт разбивателя сердец!");
                locale.Clear();
                applicationCommandProperties.Add(dontLove.Build());

                var drain = new SlashCommandBuilder();
                locale.Add("ru", "слив");
                drain.WithName("drain");
                drain.WithNameLocalizations(locale);
                drain.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                drain.WithDescription("Шлёт слив!");
                locale.Clear();
                applicationCommandProperties.Add(drain.Build());

                var play = new SlashCommandBuilder();
                locale.Add("ru", "плей");
                play.WithName("play");
                play.WithNameLocalizations(locale);
                play.WithDefaultMemberPermissions(GuildPermission.Administrator);
                play.WithDescription("Проиграть музыку с ссылки (только ютуб)");
                play.AddOption("url", ApplicationCommandOptionType.String, "Ссылка на музыку", true);
                locale.Clear();
                //applicationCommandProperties.Add(play.Build());

                var join = new SlashCommandBuilder();
                locale.Add("ru", "войти");
                join.WithName("join");
                join.WithNameLocalizations(locale);
                join.WithDefaultMemberPermissions(GuildPermission.Administrator);
                join.WithDescription("Говорит боту зайти к вам в голосовой канал!");
                locale.Clear();
                //applicationCommandProperties.Add(join.Build());

                var refreshGoogleSheetData = new SlashCommandBuilder();
                locale.Add("ru", "перезагрузитьданныетаблицы");
                refreshGoogleSheetData.WithName("refreshgooglesheetdata");
                refreshGoogleSheetData.WithNameLocalizations(locale);
                refreshGoogleSheetData.WithDescription("Принудительно перезагружает данные таблицы вайт листа");
                refreshGoogleSheetData.WithDefaultMemberPermissions(GuildPermission.Administrator);
                locale.Clear();
                applicationCommandProperties.Add(refreshGoogleSheetData.Build());

                var acceptNotification = new SlashCommandBuilder();
                locale.Add("ru", "уведомление");
                acceptNotification.WithName("acceptnotification");
                acceptNotification.WithNameLocalizations(locale);
                acceptNotification.WithDescription("Уведомляет человека о том, что его заявка принята");
                acceptNotification.WithDefaultMemberPermissions(GuildPermission.Administrator);
                acceptNotification.AddOption("user", ApplicationCommandOptionType.User, "Человек, которому надо дать роль", true);
                locale.Clear();
                applicationCommandProperties.Add(acceptNotification.Build());

                List<ApplicationCommandProperties> mpty = new();
                await Program.instance.client.BulkOverwriteGlobalApplicationCommandsAsync(mpty.ToArray());
                await Program.instance.edenor.BulkOverwriteApplicationCommandAsync(applicationCommandProperties.ToArray());
            }
            catch (Exception e)
            {
                Program.instance.logError("Error while setting up commands " + e.Message);
            }
        }
        public static async Task onCommand(SocketSlashCommand command)
        {
            if (command.GuildId == null) {command.RespondAsync("Нельзя использовать команды в личных сообщениях!"); return;}
            var options = command.Data.Options.ToArray();
            var embed = new EmbedBuilder(); 
            Embed[] embeds = new Embed[1];
            switch (command.CommandName)
            {
                case "acceptnotification":
                    embed.Title = "Здравствуйте, ваша заявка на сервер была одобрена!";
                    embed.Description = "Советую ознакомиться с правилами на сайте! \nАйпи сервера: \nJava - mc.edenor.ru или play.edenor.ru \nBedrock - pe.edenor.ru(порт 25565)";
                    embed.WithColor(new Color(0, 255, 255));
                    var author = new EmbedAuthorBuilder();
                    author.Name = "Эденор";
                    author.IconUrl = "https://sun7-16.userapi.com/impg/Rjb4mZYT_1Je6qzdgGm3gvCNDLCTmuCLj3qxQA/gJsPNJDwC2Y.jpg?size=1000x1000&quality=96&sign=a3c8cd1edbf9776f4fe989dea40534dd&type=album";
                    author.Url = "https://edenor.ru/";
                    embed.WithAuthor(author);
                    var footer = new EmbedFooterBuilder();
                    footer.Text = "Приятной игры!";
                    footer.IconUrl = "https://sun7-16.userapi.com/impg/Rjb4mZYT_1Je6qzdgGm3gvCNDLCTmuCLj3qxQA/gJsPNJDwC2Y.jpg?size=1000x1000&quality=96&sign=a3c8cd1edbf9776f4fe989dea40534dd&type=album";
                    embed.WithFooter(footer);
                    try
                    {
                        Program.instance.client.GetUser(((IUser)options[0].Value).Id).SendMessageAsync("", false, embed.Build());
                        command.RespondAsync("Успешно уведомили участника о принятии на приватку!");
                    }
                    catch(Exception e)
                    {
                        command.RespondAsync("Не смогли уведомить человека о принятии на приватку. \nКод ошибки: " + e.Message);
                    }
                    break;
                case "giverole":
                    if (((IUser)options[0].Value).Id == 324794944042565643 && command.User.Id != 324794944042565643) { command.RespondAsync("Невозможно изменить роль великому Альтрону!"); break; }
                    if (!(bool)options[2])
                    {
                        try
                        {
                            ModerationFunctions.giveRole((IUser)options[0].Value, (long)((IRole)options[1].Value).Id);
                            command.RespondAsync("Успешно выдали роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " участнику " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                        }
                        catch(Exception e)
                        {
                            command.RespondAsync("Не удалось выдать роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " участнику " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                        }
                    }
                    else
                    {
                        try
                        {
                            ModerationFunctions.removeRole((IUser)options[0].Value, (long)((IRole)options[1].Value).Id);
                            command.RespondAsync("Успешно забрали роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " с участника " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                        }
                        catch (Exception e)
                        {
                            command.RespondAsync("Не удалось забрать роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " у участника " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                        }
                    }
                    break;
                case "refreshgooglesheetdata":
                    try 
                    { 
                        GoogleSheetsHelper.timer(null);
                        command.RespondAsync("Успешно перезагрузили данные таблицы");
                    } 
                    catch (Exception e) 
                    { 
                        Program.instance.logTrace(e.Message);
                        command.RespondAsync("Не удалось перезагрузить данные таблицы");
                    }
                    break;
                /*case "join":
                    await MusicModule.onCommand(command);
                    break;
                case "play":
                    await MusicModule.onCommand(command);
                    break;*/
                case "untimeout":
                    if (ModerationFunctions.unTimeOutUser((IUser)options[0].Value, options[1].Value.ToString())) { command.RespondAsync("Успешно убрали тайм-аут с пользователя " + ((IUser)command.Data.Options.First().Value).Username); }
                    else { command.RespondAsync("Не удалось убрать тайм-аут с  пользователя " + ((IUser)command.Data.Options.First().Value).Username); }
                    break;
                case "drain":
                    embed.WithImageUrl("https://sun9-31.userapi.com/impg/5XDja0msmDXKIcTjCUO-q0upvW2-Q71Yjh3JRA/T9otFwXmqZI.jpg?size=889x509&quality=96&sign=3b9173beca1bdca211a583f993396ffa&type=album");
                    embeds[0] = embed.Build();
                    command.RespondAsync("Держи слив!", embeds: embeds);
                    break;
                case "dontlove":
                    embed.WithImageUrl("https://sun9-66.userapi.com/impg/Lh3ROcsv3IKMcPkdSbm8apbe3vpLV90hkPsppA/1pxGZ1XOVew.jpg?size=1080x537&quality=96&sign=7f5377687cd8c7d4e129b3e3e0badc63&type=album");
                    embeds[0] = embed.Build();
                    command.RespondAsync("Разбиватель сердец уже тут!", embeds: embeds);
                    break;
                case "whore":
                    embed.WithImageUrl("https://sun9-86.userapi.com/impg/spuIZhwADqv_2M4onFvBByajW5dqv3cUfrtZWg/a5AIF0Ve45Q.jpg?size=720x562&quality=96&sign=39491b6089ed691019a43d05b48a4de4&type=album");
                    embeds[0] = embed.Build();
                    command.RespondAsync("Шлюха на месте!", embeds: embeds);
                    break;
                case "rollback":
                    embed.WithImageUrl("https://media.discordapp.net/attachments/695958979330965624/1064490501803356240/IMG_8888.png");
                    embeds[0] = embed.Build();
                    command.RespondAsync("Откат рп", embeds: embeds);
                    break;
                case "gay":
                    embed.WithImageUrl("https://sun9-77.userapi.com/impg/JbpoRqn-xlIGK0lfTrT1-dAx1RlUkLilNm8I1w/_0c0I-NhNrY.jpg?size=518x162&quality=96&sign=327b5827ba9b9d809113f555d3029e92&type=album");               
                    embeds[0] = embed.Build();
                    command.RespondAsync("Лови гея", embeds: embeds);
                    break;
                case "selfban":
                    var builder = new ComponentBuilder().WithButton("Подтвердить", "selfBanButton");
                    command.RespondAsync(MentionUtils.MentionUser(command.User.Id) + ", вы уверенны, что хотите забанить себя?", components: builder.Build());           
                    break;

                case "setnumber":
                    NumberCountingModule.onCommand(command);
                    break;
                case "timeout":
                    TimeSpan interval;
                    if (options[1].Value.ToString().EndsWith("s"))
                    {
                        interval = TimeSpan.FromSeconds((double)(int.Parse(options[1].Value.ToString().Replace("s", ""))));
                    }
                    else if (options[1].Value.ToString().EndsWith("d"))
                    {
                        interval = TimeSpan.FromDays((double)(int.Parse(options[1].Value.ToString().Replace("d", ""))));
                    }
                    else if (options[1].Value.ToString().EndsWith("m"))
                    {
                        interval = TimeSpan.FromMinutes((double)(int.Parse(options[1].Value.ToString().Replace("m", ""))));
                    }
                    else if (options[1].Value.ToString().EndsWith("h"))
                    {
                        interval = TimeSpan.FromHours((double)(int.Parse(options[1].Value.ToString().Replace("h", ""))));
                    }
                    else
                    {
                        command.RespondAsync("Неправильно указано время! Используйте s, m, h, d, y !");
                        break;
                    }
                    if (ModerationFunctions.timeOutUser((IUser)options[0].Value, interval, options[2].Value.ToString())) { command.RespondAsync("Пользователь " + ((IUser)command.Data.Options.First().Value).Username + " успешно отправлен подумать о своём поведении!"); }
                    else { command.RespondAsync("Не удалось отправить пользователя " + ((IUser)command.Data.Options.First().Value).Username + " думать о своём поведении"); }
                    break;
                case "kick":
                    if (ModerationFunctions.kickUser((IUser)options[0].Value, options[1].Value.ToString())) { command.RespondAsync("Пользователь " + ((IUser)command.Data.Options.First().Value).Username + " успешно выгнан!"); }
                    else { command.RespondAsync("Не удалось выгнать пользователя " + ((IUser)command.Data.Options.First().Value).Username); }
                    break;
                case "ban":
                    if (ModerationFunctions.banUser((IUser)options[0].Value,Convert.ToInt32(options[1].Value), options[2].Value.ToString())) { command.RespondAsync("Пользователь " + ((IUser)command.Data.Options.First().Value).Username + " успешно забанен!"); }
                    else {command.RespondAsync("Не удалось забанить пользователя " + ((IUser)command.Data.Options.First().Value).Username);}
                    break;
                case "unban":
                    if (ModerationFunctions.unBanUser((IUser)options[0].Value, options[1].Value.ToString())) { command.RespondAsync("Пользователь " + ((IUser)command.Data.Options.First().Value).Username + " успешно разбанен!"); }
                    else { command.RespondAsync("Не удалось разбанить пользователя " + ((IUser)command.Data.Options.First().Value).Username); }
                    break;
                default:
                    break;
            }
        }
    }
}
