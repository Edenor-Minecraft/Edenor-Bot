using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Discord_Bot.handlers
{
    internal class CallbackAPIHandler
    {
        public IWebHost host;
        public CallbackAPIHandler() {
            host = new WebHostBuilder()
            .UseUrls("http://localhost:5000")
            .Configure(app =>
            {
                app.Run(async context =>
                {
                    // Handle incoming HTTP request
                    var request = context.Request;
                    var response = context.Response;

                    // Set response content type and status code
                    response.ContentType = "text/plain";
                    response.StatusCode = 200;

                    // Send a response back to the client
                    await response.WriteAsync("Hello from C# HTTP server!");
                });
            })
            .Build();         
        }
        
        public async Task startHost()
        {
            host.Run();
        }
    }
}
