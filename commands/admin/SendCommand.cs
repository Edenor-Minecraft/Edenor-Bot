namespace Discord_Bot.commands.admin
{
    public class SendCommand : BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            if (command.User.Id == 324794944042565643)
            {
                Program.instance.rcon.SendCommand(command.Data.Options.ToList()[0].Value.ToString());
                await command.RespondAsync("Успешно использовали команду!");
            }
            else
            {
                await command.RespondAsync("Недостаточно прав для использования команды!");
            }
        }
    }
}
