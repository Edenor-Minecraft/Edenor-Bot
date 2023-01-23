namespace Discord_Bot
{
    class ButtonsHandler
    {
        public static async Task onButton(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "selfBanButton":
                    if (component.Message.MentionedUsers.First().Id == component.User.Id)
                    {
                        ModerationFunctions.banUser(component.User, 0, "Самобан");
                        component.UpdateAsync(msg =>
                        {
                            msg.Content = $"Пользователь {component.User.Username} успешно забанен!";
                            msg.Components = null;
                        });
                    }                 
                    break;
            }
        }
    }
}
