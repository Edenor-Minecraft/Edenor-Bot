namespace Discord_Bot.handlers
{
    class NumberCountingModule
    {

        public static long lastNumber = 0;
        public static long lastUser = 0;
        static Emoji numberReact = new Emoji("\u2705");
        static string dir = Environment.CurrentDirectory + "/savedNumber.txt";

        public static async Task loadAll()
        {
            if (lastNumber == 0)
            {
                try
                {
                    lastNumber = getLastNumber();
                }
                catch (Exception e)
                {
                    Program.logError("Error while setting up last number " + e.Message);
                    lastNumber = 0;
                }
            }

            if (lastUser == 0)
            {
                try
                {
                    lastUser = getLastUser();
                }
                catch (Exception e)
                {
                    Program.logError("Error while setting up last user " + e.Message);
                    lastUser = 0;
                }
            }
        }

        public static Task onMessageDeleted(IMessage msg, IMessageChannel channel)
        {
            if (Convert.ToInt64(msg.Content) == lastNumber)
            {
                ((SocketTextChannel)Program.instance.edenor.GetChannel(channel.Id)).SendMessageAsync($"Число {msg.Content}, отправленное {msg.Author.Username}, было удалено. Следующее число - {Convert.ToInt64(msg.Content) + 1}");
                lastUser = (long)msg.Author.Id;
                WriteSetting(lastNumber, (long)msg.Author.Id);
            }
            return Task.CompletedTask;
        }

        public static Task doWork(SocketMessage msg)
        {
            if (!msg.Content.StartsWith("Число") && msg.Author.Id != 710401785663193158)
            {
                try
                {
                    if (Convert.ToInt64(msg.Content) == lastNumber + 1 && lastUser != Convert.ToInt64(msg.Author.Id.ToString()))
                    {
                        lastNumber += 1;
                        lastUser = Convert.ToInt64(msg.Author.Id.ToString());
                        WriteSetting(lastNumber, lastUser);
                        msg.AddReactionAsync(numberReact);
                        giveRole(Convert.ToInt64(msg.Content), msg.Author);
                        return Task.CompletedTask;
                    }
                    else
                    {
                        if (msg.Author.Id != 710401785663193158)
                        {
                            msg.DeleteAsync();
                        }
                        return Task.CompletedTask;
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Input string was not in a correct format.")) return Task.CompletedTask;
                    if (msg.Author.Id != 710401785663193158)
                    {
                        msg.DeleteAsync();
                    }
                    Program.logError(e.Message);
                    return Task.CompletedTask;
                }
            }
            else { return Task.CompletedTask; }
        }

        private static void giveRole(long number, SocketUser user)
        {
            var guildUser = (SocketGuildUser)user;
            //Check everything, lol
            if (number % 100 == 0)
            {
                guildUser.AddRoleAsync(745004854128279804);
            }

            if (number % 500 == 0)
            {
                guildUser.AddRoleAsync(994658634699124756);
            }

            if (number % 1000 == 0)
            {
                guildUser.AddRoleAsync(994659152716628019);
            }
        }
        public static void WriteSetting(long number, long user)
        {
            File.WriteAllText(dir, number.ToString() + ":" + user.ToString());
        }

        public static long getLastNumber()
        {
            if (File.Exists(dir))
            {
                long retVal = 0;
                StreamReader sr = new StreamReader(dir);
                string text = sr.ReadToEnd();
                sr.Close();
                retVal = Convert.ToInt64(text.Split(":")[0]);
                return retVal;
            }
            else
            {
                return 0;
            }
        }

        public static long getLastUser()
        {
            if (File.Exists(dir))
            {
                long retVal = 0;
                StreamReader sr = new StreamReader(dir);
                string text = sr.ReadToEnd();
                sr.Close();
                retVal = Convert.ToInt64(text.Split(":")[1]);
                return retVal;
            }
            else
            {
                return 0;
            }
        }
    }
}
