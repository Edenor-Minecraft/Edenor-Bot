using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    public class GiveRoleCommand : BaseCommandClass
    {
        public GiveRoleCommand() {
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

            commandProperties = giveRole.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "giverole") return;

            var options = command.Data.Options.ToList();
            if (((IUser)options[0].Value).Id == 324794944042565643 && command.User.Id != 324794944042565643) 
            { 
                await command.RespondAsync("Невозможно изменить роль великому Альтрону!"); 
            }
            if (!(bool)options.ElementAtOrDefault(2))
            {
                try
                {
                    ModerationFunctions.giveRole((IUser)options[0].Value, (long)((IRole)options[1].Value).Id);
                    await command.RespondAsync("Успешно выдали роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " участнику " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                }
                catch (Exception e)
                {
                    command.RespondAsync("Не удалось выдать роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " участнику " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                }
            }
            else
            {
                try
                {
                    ModerationFunctions.removeRole((IUser)options[0].Value, (long)((IRole)options[1].Value).Id);
                    await command.RespondAsync("Успешно забрали роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " с участника " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                }
                catch (Exception e)
                {
                    await command.RespondAsync("Не удалось забрать роль " + MentionUtils.MentionRole(((IRole)options[1].Value).Id) + " у участника " + MentionUtils.MentionUser(((IUser)options[0].Value).Id));
                }
            }
        }
    }
}
