using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System;

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
                Console.WriteLine("Failed to give role to user " + e.Message);
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
                Console.WriteLine("Failed to remove role from user " + e.Message);
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
                Console.WriteLine("Failed to kick user " + e.Message);
                return false;
            }
        }

        public static Boolean unBanUser(IUser user, string reason = "")
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
                Console.WriteLine("Failed to ban user " + e.Message);
                return false;
            }
        }
        public static Boolean banUser(IUser user, int days = 0, string reason = "")
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                ((SocketGuildUser)user).BanAsync(days, reason, options: options);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to ban user " + e.Message);
                return false;
            }
        }

        public static Boolean timeOutUser(IUser user, TimeSpan time, string reason = "")
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                ((SocketGuildUser)user).SetTimeOutAsync(time, options);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to kick user " + e.Message);
                return false;
            }
        }

        public static Boolean kickUser(IUser user, string reason = "")
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
                Console.WriteLine("Failed to kick user " + e.Message);
                return false;
            }
        }
    }
}
