using Discord;
using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    public class KickCommand : BaseCommandClass
    {

        public KickCommand()
        {
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

            commandProperties = kick.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "kick") return;

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
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Пользователь " + userToKick.Username + " успешно выгнан!";
                    });
                }
                else 
                {
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Не удалось выгнать пользователя " + userToKick.Username;
                    });
                }
            }
            else
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Вы не можете выгнать данного пользователя!";
                });
            }
        }
    }
}
