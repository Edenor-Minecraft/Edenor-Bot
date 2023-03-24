using OpenAI_API.Chat;

namespace Discord_Bot
{
    internal class ChatGPTModule
    {
        public static bool ready = false;
        public static Conversation chat = null;

        public static async Task HandleMessage(SocketMessage msg)
        {
            if (msg.Content.Contains($"<@{Program.instance.client.CurrentUser.Id}>"))
            {
                var channel = msg.Channel as ISocketMessageChannel;
                if (!ready)
                {
                    await channel.SendMessageAsync("Модуль ChatGPT не работает в данный момент! \n:( ", messageReference: msg.Reference);
                    return;
                }
                else
                {
                    var msgContent = msg.Content.Replace($"<@{Program.instance.client.CurrentUser.Id}>", "");
                    chat.AppendUserInput(msgContent);
                    var responce = await chat.GetResponseFromChatbot();
                    await channel.SendMessageAsync($"{msg.Author.Mention} {responce}", messageReference: msg.Reference);
                    return;
                }
            }
            
        }
    }
}
