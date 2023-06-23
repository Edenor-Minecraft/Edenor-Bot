using Discord_Bot.handlers;

namespace Discord_Bot.commands.other
{
    internal class SelfBanCommand : BaseCommandClass
    {
        public SelfBanCommand() {
            var selfban = new SlashCommandBuilder();
            locale.Add("ru", "самобан");
            selfban.WithName("selfban");
            selfban.WithNameLocalizations(locale);
            selfban.WithDescription("Банит вас!");
            locale.Clear();

            commandProperties = selfban.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "selfban") return;

            var builder = new ComponentBuilder().WithButton("Подтвердить", "selfBanButton");
            await command.ModifyOriginalResponseAsync(x => {
                x.Content = MentionUtils.MentionUser(command.User.Id) + ", вы уверенны, что хотите забанить себя?";
                x.Components = builder.Build();
            });
        }
    }
}
