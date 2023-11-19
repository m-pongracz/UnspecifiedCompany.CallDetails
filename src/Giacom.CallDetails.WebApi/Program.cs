namespace Giacom.CallDetails.WebApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        // We have to set Environment Variable due to migration command process
        return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder =>
            {
                builder.AddConsole();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}