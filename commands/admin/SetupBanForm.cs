using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    internal class SetupBanForm : BaseCommandClass
    {
        public SetupBanForm() {
            var setupbanform = new SlashCommandBuilder();
            setupbanform.WithName("setupbanform");
            setupbanform.WithDefaultMemberPermissions(GuildPermission.Administrator);
            setupbanform.WithDescription("null");

            commandProperties = setupbanform.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != commandProperties.Name.Value) return;
            embed.Title = "Подать форму на нарушение";
            embed.Description = "Для подачи формы на нарушение нажмите кнопку ниже";
            var menuBuilder1 = new ButtonBuilder();
            menuBuilder1.CustomId = "ban_form_btn";
            menuBuilder1.Label = "Подать форму";
            menuBuilder1.Style = ButtonStyle.Primary;
            var ppbuilder1 = new ComponentBuilder().WithButton(menuBuilder1);
            await command.Channel.SendMessageAsync(embed: embed.Build(), components: ppbuilder1.Build());
            await command.DeleteOriginalResponseAsync();
        }
    }
}
