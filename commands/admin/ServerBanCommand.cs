using Discord_Bot.handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.commands.admin
{
    internal class ServerBanCommand : BaseCommandClass
    {
        public ServerBanCommand() {
            var banCommand = new SlashCommandBuilder();
            locale.Add("ru", "переместитьроль");
            banCommand.WithNameLocalizations(locale);
            banCommand.WithName("srvban");
            banCommand.WithDescription("Забанить человека на сервере");
            banCommand.AddOption("nick", ApplicationCommandOptionType.String, "Ник человека для бана", true);
            banCommand.AddOption("time", ApplicationCommandOptionType.String, "Время бана (0, если вечный)", true);
            banCommand.AddOption("reason", ApplicationCommandOptionType.String, "Причина бана", true);
            banCommand.WithDefaultMemberPermissions(GuildPermission.Administrator);
            locale.Clear();

            commandProperties = banCommand.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "srvban") return;
            var options = command.Data.Options.ToList();
            if (Convert.ToInt32(options[1].Value) == 0)
            {
                if (Program.instance.rcon != null)
                {
                    string response = Program.instance.rcon.SendCommand($"ban {options[0].Value.ToString()} {options[2].Value.ToString()}");
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = response;
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
                if (Program.instance.rcon != null)
                {
                    string response = Program.instance.rcon.SendCommand($"tempban {options[0].Value.ToString()} {options[1].Value.ToString()} {options[2].Value.ToString()}");
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = response;
                    });
                }
                else
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Rcon не инициализирован!";
                    });
            }
        }
    }
}
