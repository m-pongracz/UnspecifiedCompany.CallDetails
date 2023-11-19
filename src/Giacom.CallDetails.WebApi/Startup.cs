using Giacom.CallDetails.Application;
using Giacom.CallDetails.Persistence;

namespace Giacom.CallDetails.WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddPersistence(_configuration);
        services.AddApplicationServices();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
    }
}