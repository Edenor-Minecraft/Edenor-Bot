namespace Discord_Bot.commands.moderation
{
    public class BanCommand : BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            IUser userToBan = command.Data.Options.ToList()[0].Value as IUser;
            int days = 0;
            string reason = $"{command.User.Username}";
            bool showReason = true;
            foreach (var option in command.Data.Options)
            {
                var val = option.Value;
                if (val is int)
                {
                    days = (int)val;
                }
                else if (val is string) 
                {
                    reason = (string)val + $"\n{command.User.Username}";
                }
                else if (val is bool)
                {
                    showReason = (bool)val;
                }
            }
            if (ModerationFunctions.getMaxUserRolePosition(command.User.Id) > ModerationFunctions.getMaxUserRolePosition(userToBan.Id))
            {
                if (ModerationFunctions.banUser(userToBan, days, reason, showReason))
                {
                    await command.RespondAsync("Пользователь " + userToBan.Username + " успешно забанен!");
                }
                else 
                { 
                    await command.RespondAsync("Не удалось забанить пользователя " + userToBan.Username); 
                }
            }
            else
            {
                await command.RespondAsync("Вы не можете забанить данного пользователя!");
            }
        }
    }
}
