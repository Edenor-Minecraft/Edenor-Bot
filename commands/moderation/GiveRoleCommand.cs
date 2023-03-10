namespace Discord_Bot.commands.moderation
{
    public class GiveRoleCommand : BaseCommandClass
    {
        public static async new Task onCommand(SocketSlashCommand command)
        {
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
