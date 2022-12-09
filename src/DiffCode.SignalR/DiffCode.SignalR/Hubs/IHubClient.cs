using DiffCode.SignalR.Models;



namespace DiffCode.SignalR.Hubs
{
  /// <summary>
  /// <para>Интерфейс вызова методов, выполняющихся на стороне клиента (в браузере).</para>
  /// </summary>
  public interface IHubClient
  {



    /// <summary>
    /// <para>Извещает клиентский браузер о создании нового объекта клиентского подключения (<see cref="Connection"/>).</para>
    /// </summary>
    /// <param name="cid">Код текущего клиентского подключения.</param>
    /// <returns></returns>
    Task RcvConnectedEvent(string cid);

    /// <summary>
    /// <para>
    /// Передает в клиентский браузер сведения о предыдущем клиентском подключении,
    /// инициирует запрос к серверу на передачу обновленного объекта клиентского подключения.
    /// </para>
    /// </summary>
    /// <param name="cid">Код текущего клиентского подключения.</param>
    /// <param name="pathname"></param>
    /// <param name="useragent"></param>
    /// <param name="prevcid"></param>
    /// <returns></returns>
    Task RcvUpdatedPath(string cid, string pathname, string useragent, string prevcid);

    /// <summary>
    /// <para>
    /// Передает в клиентский браузер код текущего клиентского подключения,
    /// инициирует его запись в браузерный Session Storage.
    /// </para>
    /// </summary>
    /// <param name="cid">Код текущего клиентского подключения.</param>
    /// <returns></returns>
    Task EnsureActualCid(string cid);




    /// <summary>
    /// <para>Передает в клиентский браузер объект текущего клиентского подключения.</para>
    /// </summary>
    /// <param name="conn">Объект текущего клиентского подключения.</param>
    /// <returns></returns>
    Task RcvMyConnection(Connection conn);

    /// <summary>
    /// <para>Передает в клиентский браузер объект текущего клиентского подключения.</para>
    /// </summary>
    /// <param name="conn">Объект текущего клиентского подключения.</param>
    /// <returns></returns>
    Task UpdMyConnection(Connection conn);





    /// <summary>
    /// <para>Удаляет сведения об отключившемся от сервера пользователе из общего чата во всех клиентских браузерах.</para>
    /// </summary>
    /// <param name="cid"></param>
    /// <param name="grp"></param>
    /// <param name="userid"></param>
    /// <returns></returns>
    Task RemConnectedUser(string cid, string grp, string userid);

    /// <summary>
    /// <para>Передает сведения о подключившемся к серверу пользователе в общие чаты во все клиентские браузеры.</para>
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task AddConnectedUser(string username);





    /// <summary>
    /// <para>
    /// Вызывает клиентское событие BrowseToUrl, выполняющее принудительную переадресацию на указанный URL-адрес.
    /// </para>
    /// </summary>
    /// <param name="newUrl">URL-адрес, на который производится переадресация.</param>
    /// <param name="openNew">Опционально: признак открытия URL-адреса в новом окне браузера.</param>
    /// <returns></returns>
    Task BrowseToUrl(string newUrl, bool openNew = false);


  }
}
