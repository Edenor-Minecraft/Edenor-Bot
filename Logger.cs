using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot
{
    internal class Logger
    {
        public static Task Log(LogMessage arg)
        {
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    logCritical(arg.ToString(), arg.Source);
                    break;
                case LogSeverity.Error:
                    logError(arg.ToString(), arg.Source);
                    break;
                case LogSeverity.Warning:
                    logWarn(arg.ToString(), arg.Source);
                    break;
                case LogSeverity.Info:
                    logInfo(arg.ToString(), arg.Source);
                    break;
                default:
                    logTrace(arg.ToString(), arg.Source);
                    break;
            }
            return Task.CompletedTask;
        }

        private static string Timestamp => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        public static async Task logTrace(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (Program.instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }
            else
            {
                await Program.instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }
        }

        public static async Task logError(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (Program.instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
            else
            {
                await Program.instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
        }

        public static async Task logInfo(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (Program.instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
            else
            {
                await Program.instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
        }

        public static async Task logWarn(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (Program.instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
            else
            {
                await Program.instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
        }

        public static async Task logCritical(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (Program.instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
            else
            {
                await Program.instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
        }
    }
}
