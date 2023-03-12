using Discord;

namespace Discord_Bot.commands.moderation
{
    public class KickCommand : BaseCommandClass
    {
        public new static async Task onCommand(SocketSlashCommand command)
        {
            IUser userToKick = command.Data.Options.ToList()[0].Value as IUser;
            string reason = $"{command.User.Username}";
            bool showReason = true;
            foreach (var option in command.Data.Options)
            {
                if (option.Value is string)
                    reason = option.Value.ToString();
                else if (option.Value is bool) 
                    showReason = (bool)option.Value;
            }
            if (ModerationFunctions.getMaxUserRolePosition(command.User.Id) > ModerationFunctions.getMaxUserRolePosition(userToKick.Id))
            {
                if (ModerationFunctions.kickUser(userToKick, reason, showReason)) 
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
