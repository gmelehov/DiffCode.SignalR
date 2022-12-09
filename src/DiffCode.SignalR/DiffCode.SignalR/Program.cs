using DiffCode.SignalR.Data;
using DiffCode.SignalR.Hubs;
using DiffCode.SignalR.Models;
using DiffCode.SignalR.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;




namespace DiffCode.SignalR
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);


      builder.Services.Configure<CookiePolicyOptions>(options =>
      {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
        options.Secure = CookieSecurePolicy.Always;
      });


      var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


      builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
      builder.Services
        .AddIdentity<User, IdentityRole<int>>(options =>
        {
          options.SignIn.RequireConfirmedEmail = false;
          options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        ;



      builder.Services.AddAuthorization();
      builder.Services.AddAuthentication();

      builder.Services.AddTransient<IHubContextService, HubContextService>();

      builder.Services
        .AddCors()
        .AddMvc()
        .AddNewtonsoftJson(opts =>
        {
          opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          opts.SerializerSettings.MaxDepth = 2;
          opts.UseMemberCasing();
        })
        ;





      builder.Services.AddRazorPages();
      builder.Services.AddSignalR(opts =>
      {
        opts.EnableDetailedErrors = true;
        opts.MaximumReceiveMessageSize = 1048576;
        opts.KeepAliveInterval = TimeSpan.FromSeconds(10);
        opts.ClientTimeoutInterval = TimeSpan.FromSeconds(20);
      })
        .AddNewtonsoftJsonProtocol(opts =>
        {
          opts.PayloadSerializerSettings.Formatting = Formatting.None;
          opts.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = null };
        });


      builder.Services.AddDatabaseDeveloperPageExceptionFilter();

  
      builder.Services.AddControllersWithViews();

      

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }



      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseCookiePolicy();
      app.UseAuthentication();
      app.UseAuthorization();


      app.UseCors(opts =>
      {
        opts.AllowAnyOrigin();
        opts.AllowAnyHeader();
        opts.AllowAnyOrigin();
      });



      app.MapControllers();
      app.MapRazorPages();
      app.MapHub<SignalRHub>("/SRHub", conn =>
      {
        conn.ApplicationMaxBufferSize = 2097152;
        conn.TransportMaxBufferSize = 2097152;
      });


      //app.Services.GetService<ApplicationDbContext>()?.CloseHangedConnections();


      app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");



      using (var scope = app.Services.CreateScope())
      {
        var services = scope.ServiceProvider;

        try
        {
          var uman = services.GetRequiredService<UserManager<User>>();
          var roleman = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
          //await UsersInitializer.InitializeAsync(uman, roleman);

          var user00 = new User
          {
            UserName = "anonymous",
            Email = "some@email.com",
          };

          IdentityResult result = null;

          if (await uman.FindByNameAsync(user00.UserName) == null)
          {
            result = await uman.CreateAsync(user00);
          };

        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "Error seeding database");
        };

      };



      app.Lifetime.ApplicationStarted.Register(() =>
      {
        using (var scope = app.Services.CreateScope())
        {
          var services = scope.ServiceProvider;
          var dbctx = services.GetRequiredService<ApplicationDbContext>();
          dbctx.CloseHangedConnections();
        };
      });

      app.Lifetime.ApplicationStopped.Register(() =>
      {
        using (var scope = app.Services.CreateScope())
        {
          var services = scope.ServiceProvider;
          var dbctx = services.GetRequiredService<ApplicationDbContext>();
          dbctx.CloseHangedConnections();
        };
      });

      app.Run();
      
    }
  }
}