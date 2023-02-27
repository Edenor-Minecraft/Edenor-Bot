namespace Discord_Bot
{
    class NumberCountingModule
    {

        static long lastNumber = 0;
        private static long lastUser = 0;
        static Emoji numberReact = new Emoji("\u2705");
        static string dir = (Environment.CurrentDirectory + "/savedNumber.txt");

        public static async Task onCommand(SocketSlashCommand command)
        {
            switch (command.CommandName)
            {
                case "setnumber":
                    if (Convert.ToInt64(command.Data.Options.First().Value.ToString()) < 0) { command.RespondAsync("Начальное число не может быть меньше 0!"); break;}
                    long val = 0;
                    if (Convert.ToInt64(command.Data.Options.First().Value.ToString()) != 0) { val = Convert.ToInt64(command.Data.Options.First().Value.ToString()) - 1; }
                    WriteSetting(val, 0);
                    lastNumber = val;
                    lastUser = 0;
                    command.RespondAsync("Теперь отсчёт начнётся с " + command.Data.Options.First().Value.ToString() + "!");
                    break;
                default:
                    break;
            }
        }

        public static Task onMessageDeleted(IMessage msg, IMessageChannel channel)
        {
            if (Convert.ToInt64(msg.Content) == lastNumber)
            {
                ((SocketTextChannel)Program.instance.edenor.GetChannel(channel.Id)).SendMessageAsync($"Число {msg.Content}, отправленное {msg.Author.Username}, было удалено. Следующее число - {Convert.ToInt64(msg.Content) + 1}");
            }
            return Task.CompletedTask;        
        }

        public static Task doWork(SocketMessage msg)
        {
            if (lastNumber == 0)
            {
                try
                {
                    lastNumber = getLastNumber();
                }
                catch (Exception e)
                {
                    Program.instance.logError("Error while setting up last number " + e.Message);
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
                    Program.instance.logError("Error while setting up last user " + e.Message);
                    lastUser = 0;
                }
            }

            try
            {
                if (Convert.ToInt64(msg.Content) == (lastNumber + 1) && lastUser != Convert.ToInt64(msg.Author.Id.ToString()))
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
                if (msg.Author.Id != 710401785663193158)
                {
                    msg.DeleteAsync();
                }
                Program.instance.logError(e.Message);
                return Task.CompletedTask;
            }
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
        private static void WriteSetting(long number, long user)
        {
            File.WriteAllText(dir, number.ToString() + ":" + user.ToString());
        }

        private static long getLastNumber()
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

        private static long getLastUser()
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
