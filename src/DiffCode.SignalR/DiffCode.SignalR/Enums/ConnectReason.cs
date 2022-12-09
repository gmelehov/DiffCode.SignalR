namespace DiffCode.SignalR.Enums
{
  /// <summary>
  /// <para>Причины открытия подключений.</para>
  /// </summary>
  public enum ConnectReason
  {

    /// <summary>
    /// <para>Причина не указана/не установлена.</para>
    /// </summary>
    NONE = 0,
    /// <summary>
    /// <para>Подключение открыто после того, как пользователь обновил страницу в браузере.</para>
    /// </summary>
    AFTER_REFRESH = 1,
    /// <summary>
    /// <para>Подключение открыто после того, как пользователь перешел к новому адресу на странице браузера.</para>
    /// </summary>
    AFTER_BROWSE = 2,
    /// <summary>
    /// <para>Подключение открыто после успешного автоматического реконнекта по команде сервера.</para>
    /// </summary>
    AFTER_RECONNECT = 3,
    /// <summary>
    /// <para>Подключение было открыто после принудительного закрытия зависшего подключения по причине аварийного завершения работы приложения/браузера/и т.п.</para>
    /// </summary>
    AFTER_ERROR = 4,
    /// <summary>
    /// <para>Подключение было открыто после открытия страницы приложения в браузере.</para>
    /// </summary>
    AFTER_START = 5,
    /// <summary>
    /// <para>Другая причина.</para>
    /// </summary>
    AFTER_OTHER = 10,

  }
}
