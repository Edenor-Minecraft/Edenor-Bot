using Discord;
using System.Collections.Generic;

namespace Discord_Bot
{
    class ModerationFunctions
    {
        public static Boolean warnUser(IUser user, string reason, bool sendReason)
        {
            try
            {
                var userData = Program.instance.userDatabase.GetUserData(user.Id).Result;
                if (userData != null)
                {
                    userData.WarnData.WarnCount += 1;
                    if (userData.WarnData.WarnCount >= 3)
                    {
                        userData.WarnData.WarnCount = 0;
                        userData.WarnData.WarnEnds.Clear();
                        banUser(user, 0, "Превышено максимально количество предупреждений за нарушения!", true);
                    }
                    else
                    {
                        userData.WarnData.WarnEnds.Add(new handlers.WarnDataTimes(DateTime.Now));
                        if (sendReason && reason != null)
                        {
                            Program.instance.edenor.GetUser(user.Id).SendMessageAsync("Вы получили предупреждение по причине: " + reason);
                        }
                    }
                    Program.instance.userDatabase.ModifyUserData(user.Id, userData);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Program.logError("Failed to warn user " + e.Message);
                return false;
            }
        }
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
        public static Boolean banUser(IUser iuser, int days, string reason, bool sendReason = true)
        {
            try
            {
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                if (Program.instance.edenor.GetUser(iuser.Id).SendMessageAsync($"Вы были забанены на сервере {Program.instance.edenor.Name}{(sendReason ? $" по причине {reason}" : "")}!").Result != null)
                {
                    Program.instance.edenor.AddBanAsync(iuser, (int)days, reason, options: options);
                }
                else
                {
                    Program.logError("Не удалось отправить причину бана пользователю!");
                    Program.instance.edenor.AddBanAsync(iuser, (int)days, reason, options: options);
                }
                return true;
            }
            catch(Exception e)
            {
                Program.logError("Failed to ban user " + e.Message);
                return false;
            }
        }

        public static Boolean timeOutUser(IUser iuser, TimeSpan? time, string reason, bool sendReason = true)
        {
            try
            {
                var user = iuser as SocketGuildUser;
                if (time == null) time = TimeSpan.FromHours(1);

                var options = new RequestOptions();
                options.AuditLogReason = reason;
                user.SendMessageAsync($"Вы были замучены на сервере {Program.instance.edenor.Name}{(sendReason ? $" по причине {reason}" : "")}!");
                user.SetTimeOutAsync((TimeSpan)time, options);
                return true;
            }
            catch (Exception e)
            {
                Program.logError("Failed to timeout user " + e.Message);
                return false;
            }
        }

        public static Boolean kickUser(IUser iuser, string reason, bool sendReason = true)
        {
            try
            {
                var user = iuser as SocketGuildUser;
                var options = new RequestOptions();
                options.AuditLogReason = reason;
                if (user.SendMessageAsync($"Вы были кикнуты с сервера {Program.instance.edenor.Name}{(sendReason ? $" по причине {reason}" : "")}!").Result != null)
                {
                    user.KickAsync(reason, options: options);
                }
                else
                {
                    Program.logError("Не удалось отправить причину кика пользователю!");
                    user.KickAsync(reason, options: options);
                }
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
