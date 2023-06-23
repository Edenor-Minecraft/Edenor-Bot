using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    public class SendCommand : BaseCommandClass
    {
        public SendCommand() {
            var sendCommand = new SlashCommandBuilder();
            sendCommand.WithName("sendcommand");
            sendCommand.WithDescription("TEST");
            sendCommand.AddOption("command", ApplicationCommandOptionType.String, "command", true);
            sendCommand.WithDefaultMemberPermissions(GuildPermission.Administrator);

            commandProperties = sendCommand.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "sendcommand") return;
            if (command.User.Id == 324794944042565643)
            {
                if (Program.instance.rcon != null)
                {
                    Program.instance.rcon.SendCommand(command.Data.Options.ToList()[0].Value.ToString());
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Успешно использовали команду!";
                    });
                }
                else
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Rcon не инициализирован!";
                    });
            }
            else
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Недостаточно прав для использования команды!";
                });
            }
        }
    }
}
