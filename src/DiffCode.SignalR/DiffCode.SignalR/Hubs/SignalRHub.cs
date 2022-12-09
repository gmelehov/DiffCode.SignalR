using DiffCode.SignalR.Data;
using DiffCode.SignalR.Models;
using DiffCode.SignalR.Services;

using Microsoft.AspNetCore.SignalR;



namespace DiffCode.SignalR.Hubs
{
  /// <summary>
  /// <para>Хаб SignalR.</para>
  /// </summary>
  public class SignalRHub : Hub<IHubClient>
  {
    private readonly IHubContextService hctx;
    private readonly ApplicationDbContext dbctx;







    public SignalRHub(IHubContextService hubContextService, ApplicationDbContext applicationDbContext)
    {
      hctx = hubContextService;
      dbctx = applicationDbContext;
    }











    /// <summary>
    /// <para>Добавляет клиентское подключение в группу подключений с указанным именем.</para>
    /// </summary>
    /// <param name="connection">Идентификатор добавляемого подключения.</param>
    /// <param name="groupName">Название группы подключений.</param>
    /// <returns></returns>
    private async Task AddToGroup(string connection, string groupName) => await Groups.AddToGroupAsync(connection, groupName);

    /// <summary>
    /// <para>Удаляет клиентское подключение из группы подключений с указанным именем.</para>
    /// </summary>
    /// <param name="connection">Идентификатор удаляемого подключения.</param>
    /// <param name="groupName">Название группы подключений.</param>
    /// <returns></returns>
    private async Task RemoveFromGroup(string connection, string groupName) => await Groups.RemoveFromGroupAsync(connection, groupName);





    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
      var cid = Context.ConnectionId;
      var grp = "";

      /// Подключение к хабу только что установлено, создаем объект Connection
      /// и заполняем его всей доступной на этот момент информацией.
      var conn = new Connection
      {
        Cid = cid,
        Group = grp,
        IP = GetCallerRemoteIP(Context),
        StartedOn = DateTime.Now,
        IsActive = true,
      };

      var username = hctx.GetCurrentUser(Context);
      var user = dbctx.Users.FirstOrDefault(f => f.UserName == username);
      var userid = user.Id;

      conn.CallerId = userid;

      if(user.ActiveConns == 0)
      {
        conn.ConnectReason = Enums.ConnectReason.AFTER_START;
      }
      
      dbctx.Connections.Add(conn);
      dbctx.SaveChanges();

      await base.OnConnectedAsync();

      /// Отправляем код подключения к хабу SignalR в клиентский браузер,
      /// из которого было инициировано это подключение.
      await Clients.Caller.RcvConnectedEvent(cid);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="exception"><inheritdoc /></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
      var cid = Context.ConnectionId;
      var conn = dbctx.Connections.FirstOrDefault(f => f.Cid == cid);
      var grp = conn.Group;

      conn.Close();      
      dbctx.Connections.Update(conn);
      dbctx.SaveChanges();

      /// Если закрывамое подключение - последнее из активных подключений пользователя...
      if (conn.Caller.ActiveConns == 0)
      {
        /// Устанавливаем причину закрытия этого подключения = закрытие браузера.
        conn.DisconnectReason = Enums.DisconnectReason.ON_EXIT;
        dbctx.Connections.Update(conn);

        /// Если текущий пользователь не анонимный...
        if (!conn.Caller.IsAnonymous())
        {
          /// Записываем дату и время, когда этот пользователь последний раз был на сайте
          conn.Caller.LastOnlineOn = DateTime.Now;
          dbctx.Users.Update(conn.Caller);
          
          /// Удаляем объект подключения из всех браузеров всех посетителей сайта
          await Clients.All.RemConnectedUser(cid, hctx.GetCurrentGroup(Context), conn.Caller.UserName);
        }

        dbctx.SaveChanges();
      }
      
      await RemoveFromGroup(cid, grp);
      await base.OnDisconnectedAsync(exception);
    }






    /// <summary>
    /// <para>Возвращает текстовое представление IP-адреса пользователя, открывшего подключение.</para>
    /// </summary>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public string GetCallerRemoteIP(HubCallerContext ctx)
    {
      var ret = ctx?.GetHttpContext()?.Connection?.RemoteIpAddress?.ToString() ?? ctx?.GetHttpContext()?.Connection?.LocalIpAddress?.ToString();
      if (ret == "::1" || ret == "127.0.0.1")
      {
        ret = "localhost";
      };

      return ret;
    }

    /// <summary>
    /// <para>Возвращает имя группы подключений, в которую необходимо добавить текущее клиентское подключение.</para>
    /// </summary>
    /// <param name="pathname"></param>
    /// <param name="useragent"></param>
    /// <param name="prevcid"></param>
    /// <returns></returns>
    public string ComputeGroupName(string pathname, string useragent, string prevcid)
    {
      string result = "";

      result = pathname;
      // логика вычисления имени группы

      return result;
    }








    /// <summary>
    /// <para>Транслирует идентификатор текущего подключения в браузер вызвавшего клиента.</para>
    /// <para>Обеспечивает отслеживание цепочки подключений после закрытия предыдущего подключения и открытия нового.</para>
    /// </summary>
    /// <returns></returns>
    public async Task RewriteActualCid() => await Clients.Caller.EnsureActualCid(Context.ConnectionId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pathname"></param>
    /// <param name="useragent"></param>
    /// <param name="prevcid"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public async Task UpdateCallerInfo(string pathname, string useragent, string prevcid, short width, short height)
    {
      var cid = Context.ConnectionId;
      var conn = dbctx.Connections.FirstOrDefault(f => f.Cid == cid);
      var prevconn = dbctx.Connections.FirstOrDefault(f => f.Cid == prevcid);
      conn.UpdateInfo(useragent, width, height);
      conn.UpdatePrevConn(prevconn?.Id);
      conn.Group = ComputeGroupName(pathname, useragent, prevcid);

      if(prevconn != null)
      {
        /// Если название группы (в данном случае - адрес страницы приложения) для предыдущего
        /// и текущего подключения совпадают, устанавливаем причину закрытия предыдущего подключения = ON_REFRESH,
        /// а причину открытия текущего подключения = AFTER_REFRESH.
        if(prevconn.Group == conn.Group)
        {
          prevconn.DisconnectReason = Enums.DisconnectReason.ON_REFRESH;
          conn.ConnectReason = Enums.ConnectReason.AFTER_REFRESH;
        }
        else
        {
          prevconn.DisconnectReason = Enums.DisconnectReason.ON_BROWSE;
          conn.ConnectReason = Enums.ConnectReason.AFTER_BROWSE;
        }

        dbctx.Connections.Update(prevconn);
      }

      
      dbctx.Connections.Update(conn);
      dbctx.SaveChanges();

      await AddToGroup(cid, conn.Group);
      await Clients.Caller.RcvUpdatedPath(cid, pathname, useragent, prevcid);

      if (!conn.Caller.IsAnonymous())
      {
        conn.Caller.LastOnlineOn = null;
        dbctx.Users.Update(conn.Caller);
        dbctx.SaveChanges();

        await Clients.AllExcept(cid).AddConnectedUser(conn.Caller.UserName);
      };
    }

    /// <summary>
    /// <para>Запрос на отправку вызвавшему посетителю данных о текущем подключении.</para>
    /// </summary>
    /// <returns></returns>
    public async Task GetMyConnection() => await Clients.Caller.RcvMyConnection(hctx.GetCurrentConnection(Context));

    /// <summary>
    /// <para>Запрос на отправку вызвавшему посетителю данных о его подключении.</para>
    /// </summary>
    /// <returns></returns>
    public async Task UpdateMyConnection() => await Clients.Caller.UpdMyConnection(hctx.GetCurrentConnection(Context));

    /// <summary>
    /// <para>Запрос на выполнение принудительной переадресации вызвавшего посетителя на страницу с указанным URL-адресом.</para>
    /// </summary>
    /// <param name="newUrl">URL-адрес, на который производится переадресация.</param>
    /// <param name="openNew">Опционально: признак открытия URL-адреса в новом окне браузера.</param>
    /// <returns></returns>
    public async Task RedirectClientToUrl(string newUrl, bool openNew = false) => await Clients.Caller.BrowseToUrl(newUrl, openNew);






  }
}
