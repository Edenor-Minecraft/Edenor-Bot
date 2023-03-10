namespace Discord_Bot.handlers
{
    class SelectMenuHandler
    {
        public static async Task onSelect(SocketMessageComponent arg)
        {
            switch (arg.Data.CustomId)
           {
                case "pp_select_menu":
                    if(string.Join(", ", arg.Data.Values) == "by_request")
                    {
                        var mb = new ModalBuilder()
                        .WithTitle("Получение роли игрока приватки")
                        .WithCustomId("pp_role_by_request")
                        .AddTextInput("Ваш ник в Minecraft", "minecraft_nick", placeholder: "Steve")
                        .AddTextInput("Ваш вк (если есть)", "vk", placeholder: "https://vk.com/feed");
                        arg.RespondWithModalAsync(mb.Build());
                    }
                    else
                    {
                        var mb = new ModalBuilder()
                        .WithTitle("Получение роли игрока приватки")
                        .WithCustomId("pp_role_purchased")
                        .AddTextInput("Ваш ник в Minecraft", "minecraft_nick", placeholder: "Steve")
                        .AddTextInput("Ваш вк (если есть)", "vk", placeholder: "https://vk.com/feed");
                        arg.RespondWithModalAsync(mb.Build());
                    }
                    break;
           }
        }
    }
}
