using System.Text.Json.Serialization;

namespace Discord_Bot.handlers
{
    internal class UserDatabase
    {
        static string file = (Environment.CurrentDirectory + "/serversDatas/edenorData.json");

        static UserDatabase instance = null;

        public ServerData edenorData;

        public UserDatabase(ulong serverId)
        {
            instance = this;

            edenorData = new ServerData(serverId);
            Program.logInfo($"Successfully created base for {edenorData.Id}");
        }

        public async Task initDatabase()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "/serversDatas"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/serversDatas");
            }

            if (File.Exists(file)) 
            {
                string stream = File.ReadAllText(file);
                var options = new JsonSerializerOptions { IncludeFields= true };

                var tempData = JsonSerializer.Deserialize<ServerDataJson>(stream, options);

                edenorData.Id = tempData.Id;

                foreach (var user in tempData.Users)
                {
                    UserData tempUserData = new UserData(user);

                    var tempWarnEnds = new List<WarnDataTimes>();
                    foreach (var data in user.WarnData.WarnEnds)
                    {
                        tempWarnEnds.Add(new WarnDataTimes(data.warnStart));
                    }

                    tempUserData.WarnData.WarnEnds = tempWarnEnds;
                    tempUserData.WarnData.WarnCount = user.WarnData.WarnCount;

                    tempUserData.BanData.BanEnd = user.BanData.BanEnd;
                    tempUserData.BanData.IsBanned = user.BanData.IsBanned;

                    var tempRolesData = new List<ulong>();
                    foreach (var role in user.UserRoles)
                    {
                        tempRolesData.Add(role);
                    }
                    tempUserData.UserRoles = tempRolesData;

                    edenorData.Users.Add(tempUserData);
                }
            }
            else 
            {
                Program.logInfo("Creating new server data!");

                var userList = await Program.instance.client.GetGuild(edenorData.Id).GetUsersAsync().ToListAsync();

                foreach (var user in userList)
                {
                    foreach (var i in user)
                    {
                        UserData tempData = new UserData(i);

                        edenorData.Users.Add(tempData);
                    }
                }
            }
        }

        public async Task<UserData> GetUserData(ulong userId, ulong ?serverId = 677860751695806515)
        {
            foreach(var user in edenorData.Users)
            {
                if (user.UserId == userId)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<bool> ModifyUserData(ulong userId, UserData newData)
        {
            foreach(var user in edenorData.Users)
            {
                if (user.UserId == userId)
                {
                    edenorData.Users.Remove(user);
                    edenorData.Users.Add(newData);
                    await saveData();
                    return true;
                }
            }
            return false;
        }

        public static void timer(object stateInfo)
        {
            if (instance.edenorData != null)
            {
                instance.saveData();
            }  
        }
        public async Task saveData()
        {
            try
            {
                if (edenorData != null)
                {
                    var options = new JsonSerializerOptions { IncludeFields = true, WriteIndented = true };
                    string stream = JsonSerializer.Serialize<ServerData>(edenorData, options);
                    await File.WriteAllTextAsync(file, stream);
                }
            }
            catch (Exception e)
            {
                Program.logError(e.Message + e.StackTrace);
            }
        }
    }

    class ServerData
    {
        public ServerData(ulong serverId)
        {
            Id = serverId;
            Users = new List<UserData>();
        }
        public ulong Id { get; set; }

        [JsonInclude]
        public List<UserData> Users { get; set; }
    }

    class UserData
    {
        public UserData(IGuildUser user)
        {
            UserId = user.Id;
            BanData = new UserBanData();
            WarnData = new UserWarnData();
            UserRoles = new();

            foreach (var role in user.RoleIds)
            {
                UserRoles.Add(role);
            }
        }

        public UserData(UserDataJson user)
        {
            UserId = user.UserId;
            BanData = new UserBanData();
            WarnData = new UserWarnData();
            UserRoles = new();
        }

        public ulong UserId { get; set; }

        [JsonInclude]
        public UserBanData BanData { get; set; }

        [JsonInclude]
        public UserWarnData WarnData { get; set; }

        [JsonInclude]
        public List<ulong> UserRoles { get; set; }
    }

    class UserBanData
    {
        public UserBanData()
        {
            IsBanned = false;
            BanEnd = null;
        }
        public bool IsBanned { get; set; }

        public DateTime ?BanEnd { get; set; }
    }

    class UserWarnData
    {
        public UserWarnData()
        {
            WarnCount = 0;
            WarnEnds = new List<WarnDataTimes>();
        }
        public int WarnCount { get; set; }

        public List<WarnDataTimes> WarnEnds { get; set; }
    }

    class WarnDataTimes
    {
        public WarnDataTimes (DateTime warnTime)
        {
            warnStart = warnTime;
            warnEnd = warnTime.AddHours(24);
        }
        public DateTime warnStart { get; set; }

        public DateTime warnEnd { get; set; }
    }


    class ServerDataJson
    {
        public ulong Id { get; set; }

        [JsonInclude]
        public List<UserDataJson> Users { get; set; }
    }

    class UserDataJson
    {
        public ulong UserId { get; set; }

        [JsonInclude]
        public UserBanDataJson BanData { get; set; }

        [JsonInclude]
        public UserWarnDataJson WarnData { get; set; }

        [JsonInclude]
        public List<ulong> UserRoles { get; set; }
    }

    class UserBanDataJson
    {
        public bool IsBanned { get; set; }

        public DateTime? BanEnd { get; set; }
    }

    class UserWarnDataJson
    {
        public int WarnCount { get; set; }

        public List<WarnDataTimesJson> WarnEnds { get; set; }
    }

    class WarnDataTimesJson
    {
        public DateTime warnStart { get; set; }

        public DateTime warnEnd { get; set; }
    }
}
