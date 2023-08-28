namespace Discord_Bot.handlers
{
    internal class OnUserUpdated
    {
        internal static Task onUpdate(Cacheable<SocketGuildUser, ulong> cacheable, SocketGuildUser user)
        {
            if (!cacheable.Value.Roles.Equals(user.Roles)) {
                var roleChanges = cacheable.Value.Roles.Except(user.Roles);
                var data = Program.instance.userDatabase.GetUserData(user.Id).Result;

                foreach (var role in roleChanges)
                {
                    data.UserRoles.Add(role.Id);
                }

                Program.instance.userDatabase.ModifyUserData(user.Id, data);
            }   

            return Task.CompletedTask;
        }
    }
}