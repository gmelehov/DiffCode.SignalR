using DiffCode.SignalR.Data;
using DiffCode.SignalR.Models;

using Microsoft.AspNetCore.SignalR;



namespace DiffCode.SignalR.Services
{
  /// <summary>
  /// <para>Реализация сервиса для работы с контекстом текущего клиентского подключения.</para>
  /// </summary>
  public class HubContextService : IHubContextService
  {
    private readonly ApplicationDbContext dbctx;




    public HubContextService(ApplicationDbContext applicationDbContext)
    {
      dbctx = applicationDbContext;
    }







    public Connection GetCurrentConnection(HubCallerContext ctx) => dbctx.Connections.FirstOrDefault(f => f.Cid == ctx.ConnectionId);


    public User GetCurrentCaller(HubCallerContext ctx) => GetCurrentConnection(ctx)?.Caller;


    public string GetCurrentGroup(HubCallerContext ctx) => GetCurrentConnection(ctx)?.Group ?? "";


    public string GetCurrentUser(HubCallerContext ctx) => ctx.User.Identity.IsAuthenticated ? ctx.User.Identity.Name : "anonymous";

  }
}
