namespace Discord_Bot.handlers
{
    internal class OnGuildJoin
    {
        internal static Task onJoin(SocketGuildUser user)
        {
            if (user.Guild.Id == 677860751695806515)
            {
                if (user.Id == 324794944042565643)
                {
                    ModerationFunctions.giveRole(user, 1081851257746116698);
                    user.SendMessageAsync("Выдал вам роли, хозяин!");
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}