using Discord;

namespace Discord_Bot.commands.moderation
{
    public class UnbanCommand :BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            IUser usetToUnban = command.Data.Options.First().Value as IUser;
            string reason = $"{command.User.Username}";
            if (command.Data.Options.ElementAtOrDefault(1) != null)
            {
                reason = command.Data.Options.ElementAtOrDefault(1).Value.ToString() + $"\n{command.User.Username}";
            }
            if (ModerationFunctions.unBanUser(usetToUnban, reason))
            { 
                await command.RespondAsync("Пользователь " + usetToUnban.Username + " успешно разбанен!"); 
            }
            else 
            {
                await command.RespondAsync("Не удалось разбанить пользователя " + usetToUnban.Username); 
            }
        }
    }
}
