namespace Discord_Bot.commands.other
{
    public class PingCommand : BaseCommandClass
    {
        public override async Task onCommand(SocketSlashCommand command)
        {
            await command.RespondAsync("Понг", options: new RequestOptions()
            {
                RatelimitCallback = MyRatelimitCallback
            });
        }
        private static async Task MyRatelimitCallback(IRateLimitInfo info)
        {
            Program.logInfo($"isGlobal: {info.IsGlobal} \n Limit: {info.Limit} \n Remaining: {info.Remaining} \n RetryAfter: {info.RetryAfter} \n Reset: {info.Reset} \n ResetAfter: {info.ResetAfter} \n Bucket: {info.Bucket} \n Lag: {info.Lag} \n Endpoint: {info.Endpoint}");
        }
    }
}
