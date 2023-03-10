using Discord;

namespace Discord_Bot.commands.moderation
{
    public class UnTimeoutCommand : BaseCommandClass
    {
        public static async new Task onCommand(SocketSlashCommand command)
        {
            IUser userToUntimeout = command.Data.Options.First().Value as IUser;
            string reason = $"{command.User.Username}";
            if (command.Data.Options.ElementAtOrDefault(1).Value != null) 
            {
                reason = command.Data.Options.ElementAtOrDefault(1).Value.ToString() + $"\n{command.User.Username}";
            }
            if (ModerationFunctions.unTimeOutUser(userToUntimeout, reason))
            { 
               await command.RespondAsync("Успешно убрали тайм-аут с пользователя " + userToUntimeout.Username); 
            }
            else {
               await command.RespondAsync("Не удалось убрать тайм-аут с  пользователя " + userToUntimeout.Username); 
            }
        }
    }
}
