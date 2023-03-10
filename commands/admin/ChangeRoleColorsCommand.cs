namespace Discord_Bot.commands.admin
{
    public class ChangeRoleColorsCommand:BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            var color = System.Drawing.ColorTranslator.FromHtml($"#{command.Data.Options.ToList()[1].Value.ToString()}");
            foreach (var role in Program.instance.edenor.Roles)
            {
                if (role.Id == ((SocketRole)command.Data.Options.ToList()[0].Value).Id)
                {
                    role.ModifyAsync(x =>
                    {
                        x.Color = new Color(color.R, color.G, color.B);
                    });
                    await command.RespondAsync("Успешно изменили цвет роли!");
                }
            }
        }
    }
}
