namespace Discord_Bot.handlers
{
    internal class BanHandler
    {
        internal static Task onBan(SocketUser user, SocketGuild guild)
        {
            if (guild.Id == 677860751695806515) {
                if (user.Id == 324794944042565643)
                {
                    var opt = new RequestOptions();
                    opt.AuditLogReason = "Авторазбан Альтрона";
                    guild.RemoveBanAsync(user, opt);
                    user.SendMessageAsync("Вы разбанены, хозяин! \n https://discord.com/invite/bC66Pwh");
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}