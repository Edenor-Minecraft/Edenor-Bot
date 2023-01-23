using Discord.Interactions;

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
                case "pp_button":
                    var mb = new ModalBuilder()
                    .WithTitle("Получение роли игрока приватки")
                    .WithCustomId("pp_role")
                    .AddTextInput("Ваш ник в Minecraft", "minecraft_nick", placeholder: "Steve")
                    .AddTextInput("Покупная или по заявке", "paid_or_free", placeholder: "Покупная/По заявке")
                    .AddTextInput("Ваш вк (если есть)", "vk", placeholder: "https://vk.com/feed");
                    component.RespondWithModalAsync(mb.Build());
                    break;
            }
        }
    }
}
