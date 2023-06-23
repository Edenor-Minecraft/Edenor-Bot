namespace Discord_Bot.commands.moderation
{
    internal class WarnCommand : BaseCommandClass
    {
        public override async Task onCommand(SocketSlashCommand command)
        {
            IUser userToWarn = command.Data.Options.ToList()[0].Value as IUser;
            string reason = $"{command.User.Username}";
            bool showReason = true;
            foreach (var option in command.Data.Options)
            {
                var val = option.Value;
                if (val is string)
                {
                    reason = (string)val + $"\n{command.User.Username}";
                }
                else if (val is bool)
                {
                    showReason = (bool)val;
                }
            }
            if (ModerationFunctions.getMaxUserRolePosition(command.User.Id) > ModerationFunctions.getMaxUserRolePosition(userToWarn.Id))
            {
                if (ModerationFunctions.warnUser(userToWarn, reason, showReason))
                {
                    await command.RespondAsync("Пользователь " + userToWarn.Username + " успешно получил предупреждение!");
                }
                else
                {
                    await command.RespondAsync("Не удалось выдать предупреждение пользователю" + userToWarn.Username);
                }
            }
            else
            {
                await command.RespondAsync("Вы не можете выдать предупреждение данному пользователю!");
            }
        }
    }
}
