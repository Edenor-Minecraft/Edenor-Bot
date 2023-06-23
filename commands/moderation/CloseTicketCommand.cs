using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    internal class CloseTicketCommand : BaseCommandClass
    {
        public CloseTicketCommand() {
            var closeTicket = new SlashCommandBuilder();
            locale.Add("ru", "закрытьтикет");
            closeTicket.WithNameLocalizations(locale);
            closeTicket.WithName("closeticket");
            closeTicket.WithDefaultMemberPermissions(GuildPermission.ManageThreads);
            closeTicket.WithDescription("Закрывает тикет, в котором прописана команда");
            locale.Clear();

            commandProperties = closeTicket.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != commandProperties.Name.Value) return;
            if (command.Channel is not SocketThreadChannel)
            {
                await command.ModifyOriginalResponseAsync(x => {
                    x.Content = "Невозможно использовать эту команду вне топика форума!";
                });
                return;
            }
            
            if ((command.Channel as SocketThreadChannel).ParentChannel.Id != 1099416318140223588)
            {
                await command.ModifyOriginalResponseAsync(x => {
                    x.Content = "Невозможно использовать эту команду вне форума жалоб!";
                });
                return;
            }

            if ((command.Channel as SocketThreadChannel).IsLocked)
            {
                await command.ModifyOriginalResponseAsync(x => {
                    x.Content = "Эта жалоба уже закрыта!";
                });
                return;
            }

            await command.ModifyOriginalResponseAsync(x => {
               x.Content = "Закрываем жалобу!";
            });

            try
            {
               SocketThreadChannel channel = command.Channel as SocketThreadChannel;

               await channel.ModifyAsync(x =>
               {
                   x.Locked = true;
               });
               await command.ModifyOriginalResponseAsync(x =>
               {
                   x.Content = "Жалоба закрыта!";
               });
            }
            catch (Exception e)
            {
               await command.ModifyOriginalResponseAsync(x =>
               {
                   x.Content = "Не удалось закрыть жалобу!";
               });

               Program.logError(e.Message + e.StackTrace);
            }
        }
    }
}
