namespace Discord_Bot
{
    class ModerationFunctions
    {
        public static Boolean giveRole(IUser user, long roleId)
        {
            try
            {
                ((SocketGuildUser)user).AddRoleAsync((ulong)roleId);
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to give role to user " + e.Message);
                return false;
            }
        }

        public static Boolean removeRole(IUser user, long roleId)
        {
            try
            {
                ((SocketGuildUser)user).RemoveRoleAsync((ulong)roleId);
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to remove role from user " + e.Message);
                return false;
            }
        }
        public static Boolean unTimeOutUser(IUser user, string reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                ((SocketGuildUser)user).RemoveTimeOutAsync();
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to unTimeout user " + e.Message);
                return false;
            }
        }

        public static Boolean unBanUser(IUser user, string reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                Program.instance.edenor.RemoveBanAsync(user, options);
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to unban user " + e.Message);
                return false;
            }
        }
        public static Boolean banUser(IUser user, int days, string reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                Program.instance.edenor.AddBanAsync(user, (int)days, reason, options: options);
                return true;
            }
            catch(Exception e)
            {
                Program.logError("Failed to ban user " + e.Message);
                return false;
            }
        }

        public static Boolean timeOutUser(IUser user, TimeSpan? time, string reason)
        {
            try
            {
                if (time == null) time = TimeSpan.FromHours(1);

                var options = new RequestOptions();
                options.AuditLogReason = reason;
                ((SocketGuildUser)user).SetTimeOutAsync((TimeSpan)time, options);
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to timeout user " + e.Message);
                return false;
            }
        }

        public static Boolean kickUser(IUser user, string reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                ((SocketGuildUser)user).KickAsync(reason, options: options);
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to kick user " + e.Message);
                return false;
            }
        }

        public static int getMaxUserRolePosition(ulong userId)
        {
            if (userId == 324794944042565643) return 2147483647;
            if (Program.instance.edenor.GetUser(userId) == null) return 0;
            else
            {
                SocketGuildUser user = Program.instance.edenor.GetUser(userId);
                return user.Roles.OrderByDescending(x => x.Position).FirstOrDefault().Position;
            }   
        }
    }
}
