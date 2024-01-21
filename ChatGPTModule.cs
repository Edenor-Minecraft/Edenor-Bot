using Discord;
using OpenAI_API.Chat;
using System.Reflection;

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
                    await channel.SendMessageAsync("Модуль ИИ не работает в данный момент! \n:( ", messageReference: msg.Reference);
                    return;
                }
                else
                {
                    var msgContent = msg.Content.Replace($"<@{Program.instance.client.CurrentUser.Id}>", "");
                    chat.AppendUserInput(msgContent);
                    var responce = await chat.GetResponseFromChatbot();
                    await channel.SendMessageAsync($"{msg.Author.Mention} {responce}", messageReference: msg.Reference);
                    /*try
                    {
                        if (chat.GetType().GetProperty("_Messages", BindingFlags.Instance | BindingFlags.NonPublic) != null)
                        {
                            List<ChatMessage> _messages = chat.GetType().GetProperty("_Messages", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(chat) as List<ChatMessage>;
                            if (_messages.Count > 3)
                            {
                                _messages.RemoveAt(0);
                                chat.GetType().GetProperty("_Messages", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(chat, _messages);
                            }
                        }
                    }   
                    catch(Exception ex)
                    {
                        Logger.logError("Failed to remove first messages!" + ex.Message + ex.StackTrace);
                    }*/
                    
                    return;
                }
            }
        }
    }
}
