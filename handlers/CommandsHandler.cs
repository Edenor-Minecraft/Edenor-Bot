using Discord;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Channels;
using Discord_Bot.commands;
using Discord_Bot.commands.admin;
using Discord_Bot.commands.other;
using Discord_Bot.commands.moderation;
using Discord_Bot.commands.funny;

namespace Discord_Bot.handlers
{
    class CommandsHandler
    {
        public static async Task setupCommands()
        {
            List<ApplicationCommandProperties> applicationCommandProperties = new();
            IDictionary<string, string> locale = new Dictionary<string, string>();
            try
            {
                var setNumber = new SlashCommandBuilder();
                locale.Add("ru", "установитьчисло");
                setNumber.WithName("setnumber");
                setNumber.WithNameLocalizations(locale);
                setNumber.WithDescription("Устанавливает число, с которого начнётся отсчёт");
                setNumber.WithDefaultMemberPermissions(GuildPermission.Administrator);
                setNumber.AddOption("number", ApplicationCommandOptionType.Integer, "Число, с которого начнётся отстчёт", true);
                locale.Clear();
                applicationCommandProperties.Add(setNumber.Build());

                var ban = new SlashCommandBuilder();
                locale.Add("ru", "бан");
                ban.WithName("ban");
                ban.WithNameLocalizations(locale);
                ban.WithDescription("Банит участника сервера");
                ban.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                ban.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет забанен", true);
                ban.AddOption("days", ApplicationCommandOptionType.Integer, "Количество дней для удаления сообщений от этого пользователя", false);
                ban.AddOption("reason", ApplicationCommandOptionType.String, "Причина бана", false);
                ban.AddOption("sendreason", ApplicationCommandOptionType.Boolean, "Отправить причину бана нарушителю?", false);
                locale.Clear();
                applicationCommandProperties.Add(ban.Build());

                var unban = new SlashCommandBuilder();
                locale.Add("ru", "разбан");
                unban.WithName("unban");
                unban.WithNameLocalizations(locale);
                unban.WithDescription("Разбанивает человека");
                unban.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                unban.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет разбанен", true);
                unban.AddOption("reason", ApplicationCommandOptionType.String, "Причина разбана", false);
                locale.Clear();
                applicationCommandProperties.Add(unban.Build());

                var kick = new SlashCommandBuilder();
                locale.Add("ru", "кик");
                kick.WithName("kick");
                kick.WithNameLocalizations(locale);
                kick.WithDescription("Выгоняет участника сервера");
                kick.WithDefaultMemberPermissions(GuildPermission.KickMembers);
                kick.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет выгнан", true);
                kick.AddOption("reason", ApplicationCommandOptionType.String, "Причина кика", false);
                kick.AddOption("sendreason", ApplicationCommandOptionType.Boolean, "Отправить причину кика нарушителю?", false);
                locale.Clear();
                applicationCommandProperties.Add(kick.Build());

                var timeout = new SlashCommandBuilder();
                locale.Add("ru", "таймаут");
                timeout.WithName("timeout");
                timeout.WithNameLocalizations(locale);
                timeout.WithDescription("Отправляет пользователя подумать о певедении");
                timeout.WithDefaultMemberPermissions(GuildPermission.ModerateMembers);
                timeout.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет отправлен думать о своём поведении", true);
                timeout.AddOption("time", ApplicationCommandOptionType.String, "Время тайм-аута плюс (s, m, h, d) в конце", false);
                timeout.AddOption("reason", ApplicationCommandOptionType.String, "Причина выдачи тайм-аута", false);
                timeout.AddOption("sendreason", ApplicationCommandOptionType.Boolean, "Отправить причину таймаута нарушителю?", false);
                locale.Clear();
                applicationCommandProperties.Add(timeout.Build());

                var unTimeout = new SlashCommandBuilder();
                locale.Add("ru", "убратьтаймаут");
                unTimeout.WithName("untimeout");
                unTimeout.WithNameLocalizations(locale);
                unTimeout.WithDescription("Убирает тайм-аут с пользователя");
                unTimeout.WithDefaultMemberPermissions(GuildPermission.ModerateMembers);
                unTimeout.AddOption("user", ApplicationCommandOptionType.User, "Пользователь, с которого надо убрать тайм-аут", true);
                unTimeout.AddOption("reason", ApplicationCommandOptionType.String, "Причина снятия тайм-аута", false);
                locale.Clear();
                applicationCommandProperties.Add(unTimeout.Build());

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

                var setupPPRole = new SlashCommandBuilder();
                setupPPRole.WithName("setuppprole");
                setupPPRole.WithDefaultMemberPermissions(GuildPermission.Administrator);
                setupPPRole.WithDescription("null");
                applicationCommandProperties.Add(setupPPRole.Build());

                var crashBot = new SlashCommandBuilder();
                crashBot.WithName("crashbot");
                crashBot.WithDescription("null");
                crashBot.WithDefaultMemberPermissions(GuildPermission.Administrator);
                applicationCommandProperties.Add(crashBot.Build());

                var ping = new SlashCommandBuilder();
                ping.WithName("ping");
                ping.WithDescription("null");
                ping.WithDefaultMemberPermissions(GuildPermission.Administrator);
                applicationCommandProperties.Add(ping.Build());

                var moveRole = new SlashCommandBuilder();
                locale.Add("ru", "переместитьроль");
                moveRole.WithNameLocalizations(locale);
                moveRole.WithName("moverole");
                moveRole.WithDescription("Перемещает указанную роль на указанное место в списке ролей");
                moveRole.AddOption("роль", ApplicationCommandOptionType.Role, "Роль, которую надо переместить", true);
                moveRole.AddOption("позиция", ApplicationCommandOptionType.Integer, "Новая позиция роли", true);
                moveRole.WithDefaultMemberPermissions(GuildPermission.Administrator);
                locale.Clear();
                applicationCommandProperties.Add(moveRole.Build());

                var changeRoleColor = new SlashCommandBuilder();
                locale.Add("ru", "изменитьцветроли");
                changeRoleColor.WithNameLocalizations(locale);
                changeRoleColor.WithName("changerolecolor");
                changeRoleColor.WithDescription("Изменяет цвет указанной роли");
                changeRoleColor.AddOption("роль", ApplicationCommandOptionType.Role, "Роль, цвет которой надо изменить", true);
                changeRoleColor.AddOption("цвет", ApplicationCommandOptionType.String, "Новый цвет роли в hex (без #!)", true);
                changeRoleColor.WithDefaultMemberPermissions(GuildPermission.Administrator);
                locale.Clear();
                applicationCommandProperties.Add(changeRoleColor.Build());

                var saveChannelMessages = new SlashCommandBuilder();
                locale.Add("ru", "сохранитьсообщенияканала");
                saveChannelMessages.WithNameLocalizations(locale);
                saveChannelMessages.WithName("savechannelmessages");
                saveChannelMessages.WithDescription("Сохраняет все сообщения указанного канала в html файл");
                saveChannelMessages.AddOption("канал", ApplicationCommandOptionType.Channel, "Канал, который надо сохранить", true);
                saveChannelMessages.WithDefaultMemberPermissions(GuildPermission.Administrator);
                locale.Clear();
                //applicationCommandProperties.Add(saveChannelMessages.Build());

                var sendCommand = new SlashCommandBuilder();
                sendCommand.WithName("sendcommand");
                sendCommand.WithDescription("TEST");
                sendCommand.AddOption("command", ApplicationCommandOptionType.String, "command", true);
                sendCommand.WithDefaultMemberPermissions(GuildPermission.Administrator);
                applicationCommandProperties.Add(sendCommand.Build());

                var setupSrvAccess = new SlashCommandBuilder();
                setupSrvAccess.WithName("setupsrvaccess");
                setupSrvAccess.WithDefaultMemberPermissions(GuildPermission.Administrator);
                setupSrvAccess.WithDescription("null");
                applicationCommandProperties.Add(setupSrvAccess.Build());

                var setupbanform = new SlashCommandBuilder();
                setupbanform.WithName("setupbanform");
                setupbanform.WithDefaultMemberPermissions(GuildPermission.Administrator);
                setupbanform.WithDescription("null");
                applicationCommandProperties.Add(setupbanform.Build());

                await Program.instance.edenor.BulkOverwriteApplicationCommandAsync(applicationCommandProperties.ToArray());
            }
            catch (Exception e)
            {
                Program.logError("Error while setting up commands " + e.Message);
            }
        }
        public static async Task onCommand(SocketSlashCommand command)
        {    
            switch (command.CommandName)
            {
                case "setupbanform":
                    await SetupBanForm.onCommand(command);
                    break;
                case "setupsrvaccess":
                    await SetupSrvAccessCommand.onCommand(command);
                    break;
                case "sendcommand":
                    await SendCommand.onCommand(command);
                    break;
                case "savechannelmessages":
                    await SaveChannelMessagesCommand.onCommand(command);
                    break;
                case "changerolecolor":
                    await ChangeRoleColorsCommand.onCommand(command);
                    break;
                case "crashbot":
                    if (command.User.Id == 324794944042565643)
                    {
                        await command.RespondAsync("Выключаем бота!");
                        await Program.instance.client.LogoutAsync();
                    }
                    else
                    {
                        await command.RespondAsync("Ты думал тут что-то будет?");
                    }
                    break;
                case "moverole":
                    await MoveRoleCommand.onCommand(command);
                    break;
                case "setuppprole":
                    await SetupPPRoleCommand.onCommand(command);
                    break;

                case "giverole":
                    await GiveRoleCommand.onCommand(command);
                    break;
                case "refreshgooglesheetdata":
                    await RefreshGoogleSheetDataCommand.onCommand(command);
                    break;
                case "untimeout":
                    await UnTimeoutCommand.onCommand(command);
                    break;
                
                case "selfban":
                    var builder = new ComponentBuilder().WithButton("Подтвердить", "selfBanButton");
                    await command.RespondAsync(MentionUtils.MentionUser(command.User.Id) + ", вы уверенны, что хотите забанить себя?", components: builder.Build());           
                    break;

                case "setnumber":
                    await NumberCountingModule.onCommand(command);
                    break;

                case "timeout":
                    await TimeoutCommand.onCommand(command);
                    break;

                case "kick":
                    await KickCommand.onCommand(command);
                    break;

                case "ban":
                    await BanCommand.onCommand(command);
                    break;

                case "unban":
                    await UnbanCommand.onCommand(command);
                    break;

                case "drain":
                    await BaseFunnyCommand.onCommand(command, "Держи слив!", "https://sun9-31.userapi.com/impg/5XDja0msmDXKIcTjCUO-q0upvW2-Q71Yjh3JRA/T9otFwXmqZI.jpg?size=889x509&quality=96&sign=3b9173beca1bdca211a583f993396ffa&type=album");
                    break;
                case "dontlove":
                    await BaseFunnyCommand.onCommand(command, "Разбиватель сердец уже тут!", "https://sun9-66.userapi.com/impg/Lh3ROcsv3IKMcPkdSbm8apbe3vpLV90hkPsppA/1pxGZ1XOVew.jpg?size=1080x537&quality=96&sign=7f5377687cd8c7d4e129b3e3e0badc63&type=album");
                    break;
                case "whore":
                    await BaseFunnyCommand.onCommand(command, "Шлюха на месте!", "https://sun9-86.userapi.com/impg/spuIZhwADqv_2M4onFvBByajW5dqv3cUfrtZWg/a5AIF0Ve45Q.jpg?size=720x562&quality=96&sign=39491b6089ed691019a43d05b48a4de4&type=album");
                    break;
                case "rollback":
                    await BaseFunnyCommand.onCommand(command, "Откат рп", "https://media.discordapp.net/attachments/695958979330965624/1064490501803356240/IMG_8888.png");
                    break;
                case "gay":
                    await BaseFunnyCommand.onCommand(command, "Лови гея", "https://sun9-77.userapi.com/impg/JbpoRqn-xlIGK0lfTrT1-dAx1RlUkLilNm8I1w/_0c0I-NhNrY.jpg?size=518x162&quality=96&sign=327b5827ba9b9d809113f555d3029e92&type=album");
                    break;

                case "ping":
                    await PingCommand.onCommand(command);
                    break;

                default:
                    break;
            }
        }

        
    }
}
