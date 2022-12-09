using DiffCode.SignalR.Models;

using Microsoft.AspNetCore.SignalR;



namespace DiffCode.SignalR.Services
{
  /// <summary>
  /// <para>Интерфейс сервиса для работы с контекстом текущего клиентского соединения.</para>
  /// </summary>
  public interface IHubContextService
  {





    /// <summary>
    /// <para>Возвращает объект текущего подключения.</para>
    /// <para>Если текущее подключение не найдено в репозитории, возвращает <see langword="null"/>.</para>
    /// </summary>
    /// <returns></returns>
    Connection GetCurrentConnection(HubCallerContext ctx);

    /// <summary>
    /// <para>Возвращает пользователя, открывшего текущее подключение.</para>
    /// </summary>
    /// <returns></returns>
    User GetCurrentCaller(HubCallerContext ctx);

    /// <summary>
    /// <para>Возвращает группу подключений для текущего подключения.</para>
    /// </summary>
    /// <returns></returns>
    string GetCurrentGroup(HubCallerContext ctx);

    /// <summary>
    /// <para>Возвращает имя текущего пользователя, либо "anonymous" для анонимных посетителей.</para>
    /// </summary>
    /// <returns></returns>
    string GetCurrentUser(HubCallerContext ctx);



  }
}
