using System.Text;

namespace Discord_Bot
{
    internal class ChannelSaver
    {
        //Maybe save in other formats
        public async static Task channelToHTMLCommand(SocketSlashCommand cmd)
        {
            await cmd.RespondAsync("Сохраняем канал в html");
            await Task.Run(async () =>
            {
                Stream stream = await channelToHTML((ISocketMessageChannel)cmd.Data.Options.ToList()[0].Value);

                var list = new List<FileAttachment>().Append(new FileAttachment(stream, $"Save-of-{((IGuildChannel)cmd.Data.Options.ToList()[0].Value).Id}.html"));
                var optional = new Optional<IEnumerable<FileAttachment>>(list.ToAsyncEnumerable().ToEnumerable());
                await cmd.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Сообщения канала успешно сохранены!";
                    x.Attachments = optional;
                });
                return Task.CompletedTask;
            });      
        }
        //Created with ChatGPT, lol
        public async static Task<Stream> channelToHTML(ISocketMessageChannel channel)
        {
            var messages = channel.GetMessagesAsync(2147483647).FlattenAsync().GetAwaiter().GetResult();

            var html = new StringBuilder();

            await Task.Run(async () =>
            {        
                html.Append("<html><body>");
                html.AppendFormat("\n<style>body {{ background-color: {0}; }} p {{ color: white; }} .username {{ color: {1}; }} img {{ border-radius: 50%; width: 25px; height: 25px; margin-right: 5px; }} .embed {{ border: 1px solid {2}; background-color: {3}; margin-left: 30px; padding: 5px; }} .embed-image {{ max-width: 100%; max-height: 100%; margin: 5px 0; }}</style>\"", new Color(49, 51, 56), new Color(255, 255, 255), Color.Blue.ToString(), Color.LightGrey.ToString());
                html.Append("\n</head><body>");
                foreach (var message in messages.Reverse())
                {
                    var user = message.Author as SocketGuildUser;
                    var maxRole = user?.Roles.OrderByDescending(r => r.Position).FirstOrDefault();
                    var usernameColor = maxRole != null ? maxRole.Color : new Color(255, 255, 255);
                    var usernameClass = maxRole != null ? "username" : "";
                    var avatarUrl = message.Author.GetAvatarUrl(ImageFormat.Auto, 25);

                    html.AppendFormat("\n<p><img src=\"{0}\" /><strong class=\"{1}\" style=\"color: {2};\">{3}:</strong> {4}</p>", avatarUrl, usernameClass, usernameColor.ToString(), message.Author.Username, message.Content);

                    foreach (var embed in message.Embeds)
                    {
                        html.Append("<div class=\"embed\">");
                        if (!string.IsNullOrWhiteSpace(embed.Title))
                        {
                            html.AppendFormat("<h3 style=\"color: {0};\">{1}</h3>", embed.Color.HasValue ? embed.Color.Value.ToString() : new Color(255, 255, 255).ToString(), embed.Title);
                        }

                        if (!string.IsNullOrWhiteSpace(embed.Description))
                        {
                            html.AppendFormat("<p>{0}</p>", embed.Description);
                        }

                        foreach (var field in embed.Fields)
                        {
                            html.AppendFormat("<p><strong style=\"color: {0};\">{1}:</strong> {2}</p>", embed.Color.HasValue ? embed.Color.Value.ToString() : new Color(255, 255, 255).ToString(), field.Name, field.Value);
                        }

                        if (!string.IsNullOrWhiteSpace(embed.Footer?.Text))
                        {
                            html.AppendFormat("<p style=\"font-size: 0.8em;\">{0}</p>", embed.Footer?.Text);
                        }

                        if (embed.Image.HasValue)
                        {
                            html.AppendFormat("<img class=\"embed-image\" src=\"{0}\" />", embed.Image.Value.Url);
                        }

                        html.Append("</div>");
                    }
                }
                html.Append("\n</body></html>");
            });

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(html.ToString());
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
