using DiffCode.SignalR.Enums;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;




namespace DiffCode.SignalR.Models
{
  /// <summary>
  /// <para>Клиентское подключение к серверу.</para>
  /// </summary>
  public class Connection
  {






    /// <summary>
    /// <para>Обновляет ряд вспомогательных свойств клиентского подключения.</para>
    /// </summary>
    /// <param name="useragent">Строка UserAgent.</param>
    /// <param name="width">Ширина окна клиентского браузера, в котором открыто подключение.</param>
    /// <param name="height">Высота окна клиентского браузера, в котором открыто подключение.</param>
    /// <returns></returns>
    public Connection UpdateInfo(string useragent = null, short? width = null, short? height = null)
    {
      if (IsActive && ClosedOn == null)
      {
        UserAgent = useragent;
        ClientWidth = width;
        ClientHeight = height;
      };
      return this;
    }

    /// <summary>
    /// <para>Обновляет данные о предыдущем клиентском подключении.</para>
    /// </summary>
    /// <param name="prevconnId">Идентификатор предыдущего клиентского подключения.</param>
    /// <returns></returns>
    public Connection UpdatePrevConn(int? prevconnId)
    {
      if (PrevId == null && prevconnId != null)
      {
        PrevId = prevconnId;
      };
      return this;
    }

    /// <summary>
    /// <para>Помечает клиентское подключение как закрытое (неактивное).</para>
    /// </summary>
    /// <returns></returns>
    public Connection Close()
    {
      if(ClosedOn == null && IsActive)
      {
        ClosedOn = DateTime.Now;
        IsActive = false;
      }

      return this;
    }

    /// <summary>
    /// <para>
    /// Помечает "зависшее" клиентское подключение как принудительно закрытое
    /// (вызывается после аварийного перезапуска сервера).
    /// </para>
    /// </summary>
    public void CloseIfHanged()
    {
      if (ClosedOn == null && IsActive == true && IsForcedClosed == false)
      {
        ClosedOn = DateTime.Now;
        IsActive = false;
        IsForcedClosed = true;
        DisconnectReason = DisconnectReason.ON_ERROR;
      };
    }










    /// <summary>
    /// <para>Identity.</para>
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// <para>Код подключения, сгенерированный библиотекой SignalR.</para>
    /// </summary>
    public string Cid { get; set; }

    /// <summary>
    /// <para>Название группы, в которую входит это подключение.</para>
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// <para>Строка UserAgent, определяющая браузер клиента.</para>
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// <para>Ширина экрана пользователя.</para>
    /// </summary>
    public short? ClientWidth { get; set; }

    /// <summary>
    /// <para>Высота экрана пользователя.</para>
    /// </summary>
    public short? ClientHeight { get; set; }

    /// <summary>
    /// <para>IP-адрес клиентского подключения.</para>
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// <para>Момент открытия подключения.</para>
    /// </summary>
    public DateTime StartedOn { get; set; }

    /// <summary>
    /// <para>Момент закрытия подключения.</para>
    /// </summary>
    public DateTime? ClosedOn { get; set; }

    /// <summary>
    /// <para>Признак активного (открытого в данный момент времени) подключения.</para>
    /// <para>После закрытия подключения устанавливается значение <see langword="false"/>.</para>
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// <para>
    /// Признак принудительного закрытия "зависшего" подключения, ошибочно оставшегося
    /// открытым после аварийного завершения работы приложения/клиентского браузера/и т.п.
    /// </para>
    /// </summary>
    public bool IsForcedClosed { get; set; } = false;

    /// <summary>
    /// <para>Причина открытия этого подключения.</para>
    /// </summary>
    public ConnectReason ConnectReason { get; set; }

    /// <summary>
    /// <para>Причина закрытия этого подключения.</para>
    /// </summary>
    public DisconnectReason DisconnectReason { get; set; }

    /// <summary>
    /// Идентификатор предыдущего подключения.
    /// </summary>
    [NotMapped]
    public string PrevCid => Previous?.Cid;













    /// <summary>
    /// <para>Внешний ключ.</para>
    /// </summary>
    public int CallerId { get; set; }

    /// <summary>
    /// <para>Ссылка пользователя, открывшего это подключение.</para>
    /// </summary>
    public virtual User Caller { get; set; }




    /// <summary>
    /// <para>Внешний ключ.</para>
    /// </summary>
    public int? PrevId { get; set; }

    /// <summary>
    /// <para>Ссылка на предыдущее подключение.</para>
    /// </summary>
    [JsonIgnore]
    public virtual Connection Previous { get; set; }

  }
}
