using Discord;

namespace Discord_Bot.commands.moderation
{
    public class TimeoutCommand : BaseCommandClass
    {
        public static TimeSpan defaultInterval = TimeSpan.FromHours(1);
        public static new async Task onCommand(SocketSlashCommand command)
        {
            IUser user = (IUser)command.Data.Options.ToList()[0].Value;
            string time = "1h";
            string reason = $"{command.User.Username}";
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
            }

            TimeSpan interval = getTimeSpan(time);
            
            if (ModerationFunctions.timeOutUser(user, interval, reason)) 
            { 
                await command.RespondAsync("Пользователь " + ((IUser)command.Data.Options.First().Value).Username + " успешно отправлен подумать о своём поведении!"); 
            }
            else 
            { 
                await command.RespondAsync("Не удалось отправить пользователя " + ((IUser)command.Data.Options.First().Value).Username + " думать о своём поведении"); 
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
