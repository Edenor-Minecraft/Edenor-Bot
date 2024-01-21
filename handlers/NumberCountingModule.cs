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
            if (lastNumber == 0 && lastUser == 0)
            {
                try
                {
                    if (File.Exists(dir))
                    {
                        StreamReader sr = new StreamReader(dir);
                        string text = sr.ReadToEnd();
                        sr.Close();
                        lastNumber = Convert.ToInt64(text.Split(":")[0]);
                        lastUser = Convert.ToInt64(text.Split(":")[1]);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    await Logger.logError("Error while setting up last number and user" + e.Message);
                    lastNumber = 0;
                    lastUser = 0;
                }
            }
        }

        public static Task onMessageDeleted(IMessage msg, IMessageChannel channel)
        {
            long msgC = checkAndConvertNumber(msg.Content);
            if (msgC != -1) {
                if (msgC == lastNumber)
                {
                    ((SocketTextChannel)Program.instance.edenor.GetChannel(channel.Id)).SendMessageAsync($"Число {msg.Content}, отправленное {msg.Author.Username}, было удалено. Следующее число - {Convert.ToInt64(msg.Content) + 1}");
                    lastUser = (long)msg.Author.Id;
                    WriteSetting(lastNumber, (long)msg.Author.Id);
                }
            }
            
            return Task.CompletedTask;
        }

        public static Task doWork(SocketMessage msg)
        {
            if (!msg.Content.StartsWith("Число") && msg.Author.Id != 710401785663193158)
            {
                long msgC = checkAndConvertNumber(msg.Content);
                long authorId = Convert.ToInt64(msg.Author.Id.ToString());
                if (msgC != -1)
                {
                    if (msgC == lastNumber + 1 && lastUser != authorId)
                    {
                        lastNumber += 1;
                        lastUser = authorId;
                        WriteSetting(lastNumber, lastUser);
                        msg.AddReactionAsync(numberReact);
                        giveRole(lastNumber, msg.Author);
                        return Task.CompletedTask;
                    }
                    else
                    {
                        removeMsg(msg);
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    removeMsg(msg);
                    return Task.CompletedTask;
                }
            }
            else { return Task.CompletedTask; }
        }

        private static void removeMsg(SocketMessage msg)
        {
            if (msg.Author.Id != 710401785663193158)
            {
                msg.DeleteAsync();
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
        public static void WriteSetting(long number, long user)
        {
            File.WriteAllText(dir, number.ToString() + ":" + user.ToString());
        }

        private static long checkAndConvertNumber(string str)
        {
            if (str.All(char.IsDigit))
            {
                return Convert.ToInt64(str);
            }

            return -1;
        }
    }
}
