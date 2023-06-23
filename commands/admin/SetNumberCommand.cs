using Discord.Rest;
using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    internal class SetNumberCommand : BaseCommandClass
    {
        public SetNumberCommand() {
            var setNumber = new SlashCommandBuilder();
            locale.Add("ru", "установитьчисло");
            setNumber.WithName("setnumber");
            setNumber.WithNameLocalizations(locale);
            setNumber.WithDescription("Устанавливает число, с которого начнётся отсчёт");
            setNumber.WithDefaultMemberPermissions(GuildPermission.Administrator);
            setNumber.AddOption("number", ApplicationCommandOptionType.Integer, "Число, с которого начнётся отстчёт", true);
            locale.Clear();
            commandProperties = setNumber.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "setnumber") { return; }
            if (Convert.ToInt64(command.Data.Options.First().Value.ToString()) < 0) { command.RespondAsync("Начальное число не может быть меньше 0!"); return; }
            long val = 0;
            if (Convert.ToInt64(command.Data.Options.First().Value.ToString()) != 0) { val = Convert.ToInt64(command.Data.Options.First().Value.ToString()) - 1; }
            NumberCountingModule.WriteSetting(val, 0);
            NumberCountingModule.lastNumber = val;
            NumberCountingModule.lastUser = 0;
            await command.ModifyOriginalResponseAsync(x =>
            {
                x.Content = "Теперь отсчёт начнётся с " + command.Data.Options.First().Value.ToString() + "!";
            });
        }
    }
}
