namespace Discord_Bot.commands.admin
{
    public class SetupSrvAccessCommand : BaseCommandClass
    {
        public async new static Task onCommand(SocketSlashCommand command)
        {
            embed.Title = "Подать заявку на сервер";
            embed.Description = "Для подачи заявки на сервер нажмите на кнопку ниже и заполните все поля. Заявки проверяются в течение 48 часов.";
            var menuBuilder1 = new ButtonBuilder();
            menuBuilder1.CustomId = "srvaccess_btn";
            menuBuilder1.Label = "Подать заявку на сервер";
            menuBuilder1.Style = ButtonStyle.Primary;
            var ppbuilder1 = new ComponentBuilder().WithButton(menuBuilder1);
            await command.Channel.SendMessageAsync(embed: embed.Build(), components: ppbuilder1.Build());
        }
    }
}
