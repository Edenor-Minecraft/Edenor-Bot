namespace Discord_Bot.commands.admin
{
    public class MoveRoleCommand :BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            try
            {
                foreach (var role in Program.instance.edenor.Roles)
                {
                    if (role.Id == ((SocketRole)command.Data.Options.ToList()[0].Value).Id)
                    {
                        role.ModifyAsync(x =>
                        {
                            x.Position = Convert.ToInt32(command.Data.Options.ToList()[1].Value);
                        });
                        await command.RespondAsync("Успешно изменили порядок ролей!");
                    }
                }
            }
            catch (Exception ex) { await command.RespondAsync("Не удалось изменить порядок ролей!"); Program.logError(ex.Message); }
        }
    }
}
