using Discord;
using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    public class TimeoutCommand : BaseCommandClass
    {
        public static TimeSpan defaultInterval = TimeSpan.FromHours(1);

        public TimeoutCommand() {
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

            commandProperties = timeout.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "timeout") return;
            IUser user = (IUser)command.Data.Options.ToList()[0].Value;
            string time = "1h";
            string reason = $"{command.User.Username}";
            bool showReason = true;
            foreach (var option in command.Data.Options)
            {
                if (option.Value is string)
                {
                    string val = (string)option.Value;
                    if (val.EndsWith("h") || val.EndsWith("s") || val.EndsWith("d") || val.EndsWith("m"))
                        time = val;
                    else
                        reason = val + $"\nBy {command.User.Username}";
                }
                else if (option.Value is bool) {
                    showReason = (bool)option.Value;
                }
            }

            TimeSpan interval = getTimeSpan(time);
            
            if (ModerationFunctions.timeOutUser(user, interval, reason, showReason)) 
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Пользователь " + ((IUser)command.Data.Options.First().Value).Username + " успешно отправлен подумать о своём поведении!";
                });
            }
            else 
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Не удалось отправить пользователя " + ((IUser)command.Data.Options.First().Value).Username + " думать о своём поведении";
                }); 
            }
        }

        static TimeSpan getTimeSpan(string value)
        {
            TimeSpan interval;
            if (value.EndsWith("s"))
            {
                interval = TimeSpan.FromSeconds((double)(int.Parse(value.Replace("s", ""))));
            }
            else if (value.EndsWith("d"))
            {
                interval = TimeSpan.FromDays((double)(int.Parse(value.Replace("d", ""))));
            }
            else if (value.EndsWith("m"))
            {
                interval = TimeSpan.FromMinutes((double)(int.Parse(value.Replace("m", ""))));
            }
            else if (value.EndsWith("h"))
            {
                interval = TimeSpan.FromHours((double)(int.Parse(value.Replace("h", ""))));
            }
            else
            {
                interval = TimeoutCommand.defaultInterval;
            }

            return interval;
        }
    }
}
