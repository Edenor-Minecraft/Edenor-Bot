namespace Discord_Bot.commands.admin
{
    public class SetupPPRoleCommand : BaseCommandClass
    {
        public static async new Task onCommand(SocketSlashCommand command)
        {
            embed.Title = "Получение роли Игрока Приватки";
            embed.Description = "Чтобы получить роль, нажмить на кнопку и заполните заявку. Роль выдаётся в течение 24 часов.";
            var menuBuilder = new SelectMenuBuilder()
            .WithPlaceholder("Выберите тип проходки")
            .WithCustomId("pp_select_menu")
            .WithMinValues(1)
            .WithMaxValues(1)
            .AddOption("По заявке", "by_request", "Выберите это, если вас приняли по заявке!")
            .AddOption("Купленная", "purchased", "Выберите это, если вы купили проходку на сайте!");
            var ppbuilder = new ComponentBuilder().WithSelectMenu(menuBuilder);
            await command.Channel.SendMessageAsync(embed: embed.Build(), components: ppbuilder.Build());
            await command.RespondAsync("Установили сообщение для подачи заявок на роль игрока приватки!", ephemeral: true);
        }
    }
}
