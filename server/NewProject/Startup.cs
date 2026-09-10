using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System.IO;
using IncidentManagement.Data;
using IncidentManagement.Services;

namespace IncidentManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            bool bypassAuth = Configuration.GetValue<bool>("BypassAuth");

            // Incidents on local SQL Express
            services.AddDbContext<IncidentDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("IncidentConnection")));
            
            services.AddScoped<IStatsService, StatsService>();
            services.AddScoped<ISearchService,   SearchService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<ILoanService,     LoanService>();
            services.AddScoped<IIssueService,    IssueService>(); // Add real IssueService when ready
            services.AddScoped<IIncidentNoteService, IncidentNoteService>();
            services.AddSignalR();
            services.AddScoped<ICallService, CallService>();
            services.AddScoped<IBranchService, BranchService>();

            if (!bypassAuth)
            {
                services.AddAuthentication()
                    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));
            }

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = 
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore);

                        services.AddCors(options =>
                        {
                            options.AddPolicy("DevCors", builder =>
                                builder.WithOrigins("http://localhost:5173")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials());
                        });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevCors");
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            bool bypassAuth = Configuration.GetValue<bool>("BypassAuth");
            if (!bypassAuth)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<IncidentManagement.Hubs.CallHub>("/hubs/call");
            });
        }
    }
}