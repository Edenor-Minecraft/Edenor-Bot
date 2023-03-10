using Discord;

namespace Discord_Bot.commands.moderation
{
    public class KickCommand : BaseCommandClass
    {
        public new static async Task onCommand(SocketSlashCommand command)
        {
            IUser userToKick = command.Data.Options.ToList()[0].Value as IUser;
            string reason = $"{command.User.Username}";
            if (command.Data.Options.ElementAtOrDefault(1).Value != null)
            {
                reason = command.Data.Options.ElementAtOrDefault(1).Value.ToString() + $"\n{command.User.Username}";
            }
            if (ModerationFunctions.getMaxUserRolePosition(command.User.Id) > ModerationFunctions.getMaxUserRolePosition(userToKick.Id))
            {
                if (ModerationFunctions.kickUser(userToKick, reason)) 
                { 
                    await command.RespondAsync("Пользователь " + userToKick.Username + " успешно выгнан!"); 
                }
                else 
                {
                    await command.RespondAsync("Не удалось выгнать пользователя " + userToKick.Username); 
                }
            }
            else
            {
                await command.RespondAsync("Вы не можете выгнать данного пользователя!");
            }
        }
    }
}
