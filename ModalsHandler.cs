namespace Discord_Bot
{
    class ModalsHandler
    {
        public static async Task onModal(SocketModal modal)
        {
            List<SocketMessageComponentData> components = modal.Data.Components.ToList();

            switch (modal.Data.CustomId)
            {
                case "pp_role":
                    var ppEmbed = new EmbedBuilder();
                    ppEmbed.WithTitle("Получение роли И.П");
                    ppEmbed.WithFooter(new EmbedFooterBuilder().WithText($"**Ваш ник в Minecraft** \n{components.First(x => x.CustomId == "minecraft_nick").Value}"));
                    ppEmbed.WithFooter(new EmbedFooterBuilder().WithText($"**Покупная или по заявке** \n{components.First(x => x.CustomId == "paid_or_free").Value}"));
                    ppEmbed.WithFooter(new EmbedFooterBuilder().WithText($"**Ваш вк (если есть)** \n{components.First(x => x.CustomId == "vk").Value}"));
                    ppEmbed.Author.WithName(modal.User.Mention).WithIconUrl(modal.User.GetAvatarUrl());    
                    GoogleSheetsHelper.checkAccepted(components.First(x => x.CustomId == "minecraft_nick").Value);
                    await ((SocketTextChannel)Program.instance.edenor.GetChannel(1055783105916571658)).SendMessageAsync(embed: ppEmbed.Build());
                    break;
            }
        }
    }
}
