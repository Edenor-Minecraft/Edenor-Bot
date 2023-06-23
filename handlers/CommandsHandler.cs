using Discord;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Channels;
using Discord_Bot.commands;
using Discord_Bot.commands.admin;
using Discord_Bot.commands.other;
using Discord_Bot.commands.moderation;
using Discord_Bot.commands.funny;

namespace Discord_Bot.handlers
{
    class CommandsHandler
    {
        public static event Func<SocketSlashCommand, Task> OnCommand;
        public static async Task setupCommands()
        {
            List<ApplicationCommandProperties> applicationCommandProperties = new();
            try
            {
                applicationCommandProperties.Add(new SetNumberCommand().commandProperties);

                applicationCommandProperties.Add(new BanCommand().commandProperties);

                applicationCommandProperties.Add(new UnbanCommand().commandProperties);

                applicationCommandProperties.Add(new KickCommand().commandProperties);

                applicationCommandProperties.Add(new TimeoutCommand().commandProperties);

                applicationCommandProperties.Add(new UnTimeoutCommand().commandProperties);
                
                applicationCommandProperties.Add(new GiveRoleCommand().commandProperties);
                
                applicationCommandProperties.Add(new SelfBanCommand().commandProperties);

                applicationCommandProperties.Add(new RefreshGoogleSheetDataCommand().commandProperties);

                applicationCommandProperties.Add(new SetupPPRoleCommand().commandProperties);

                applicationCommandProperties.Add(new MoveRoleCommand().commandProperties);

                applicationCommandProperties.Add(new ChangeRoleColorsCommand().commandProperties);
  
                applicationCommandProperties.Add(new SendCommand().commandProperties);

                applicationCommandProperties.Add(new SetupSrvAccessCommand().commandProperties);

                applicationCommandProperties.Add(new SetupBanForm().commandProperties);

                applicationCommandProperties.Add(new GetIPInfoCommand().commandProperties);

                applicationCommandProperties.Add(new CloseTicketCommand().commandProperties);

                /*var warnUser = new SlashCommandBuilder();
                locale.Add("ru", "варн");
                warnUser.WithNameLocalizations(locale);
                warnUser.WithName("warnuser");
                warnUser.WithDefaultMemberPermissions(GuildPermission.BanMembers);
                warnUser.WithDescription("Выдать предупреждение пользователю");
                warnUser.AddOption("пользователь", ApplicationCommandOptionType.User, "Пользователь, который должен получить предупреждение!", true);
                warnUser.AddOption("причина", ApplicationCommandOptionType.String, "Причина выдачи варна!", false);
                warnUser.AddOption("показатьпричину", ApplicationCommandOptionType.Boolean, "Отправить причину варна нарушителю?!", false);
                locale.Clear();
                applicationCommandProperties.Add(warnUser.Build());*/

                await Program.instance.edenor.BulkOverwriteApplicationCommandAsync(applicationCommandProperties.ToArray());
            }
            catch (Exception e)
            {
                Program.logError("Error while setting up commands " + e.Message);
            }
        }
        public static async Task onCommand(SocketSlashCommand command)
        {
            await command.RespondAsync("Выполняю команду");
            await OnCommand?.Invoke(command);
        }
    }
}
