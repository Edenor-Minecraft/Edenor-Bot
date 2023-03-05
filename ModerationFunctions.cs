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
                Program.instance.logError("Failed to give role to user " + e.Message);
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
                Program.instance.logError("Failed to remove role from user " + e.Message);
                return false;
            }
        }
        public static Boolean unTimeOutUser(IUser user, SocketSlashCommandDataOption? reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason == null ? string.Empty : reason.Value.ToString();
                ((SocketGuildUser)user).RemoveTimeOutAsync();
                return true;
            }
            catch (Exception e)
            {
                Program.instance.logError("Failed to kick user " + e.Message);
                return false;
            }
        }

        public static Boolean unBanUser(IUser user, SocketSlashCommandDataOption? reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason == null ? string.Empty : reason.Value.ToString();
                Program.instance.edenor.RemoveBanAsync(user, options);
                return true;
            }
            catch (Exception e)
            {
                Program.instance.logError("Failed to ban user " + e.Message);
                return false;
            }
        }
        public static Boolean banUser(IUser user, SocketSlashCommandDataOption? daysOpt, object? reasonOpt)
        {
            try
            {
                string reason = string.Empty;
                if (reasonOpt != null)
                {
                    if (reasonOpt.GetType() == typeof(SocketSlashCommandDataOption)) { reason = ((SocketSlashCommandDataOption)reasonOpt).Value.ToString(); }
                    else if (reasonOpt.GetType() == typeof(string)) { reason = (string)reasonOpt; }
                }
                int days = 0;
                if (daysOpt != null) days = Convert.ToInt32(daysOpt.Value);

                var options = new RequestOptions();
                options.AuditLogReason = reason;
                Program.instance.edenor.AddBanAsync(user, (int)days, reason, options: options);
                return true;
            }
            catch(Exception e)
            {
                Program.instance.logError("Failed to ban user " + e.Message);
                return false;
            }
        }

        public static Boolean timeOutUser(IUser user, TimeSpan? time, SocketSlashCommandDataOption? reason)
        {
            try
            {
                if (time == null) time = TimeSpan.FromHours(1);

                var options = new RequestOptions();
                options.AuditLogReason = reason == null ? string.Empty : reason.Value.ToString();
                ((SocketGuildUser)user).SetTimeOutAsync((TimeSpan)time, options);
                return true;
            }
            catch (Exception e)
            {
                Program.instance.logError("Failed to kick user " + e.Message);
                return false;
            }
        }

        public static Boolean kickUser(IUser user, SocketSlashCommandDataOption? reason)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason == null ? string.Empty : reason.Value.ToString();
                ((SocketGuildUser)user).KickAsync(reason == null ? string.Empty : reason.Value.ToString(), options: options);
                return true;
            }
            catch (Exception e)
            {
                Program.instance.logError("Failed to kick user " + e.Message);
                return false;
            }
        }

        public static int getMaxUserRolePosition(SocketGuildUser user)
        {
            int max = 0;
            foreach (var role in user.Roles)
            {
                if (role.Position > max) max = role.Position;
            }
            return max;
        }
    }
}
