namespace DiffCode.SignalR.Enums
{
  /// <summary>
  /// <para>Причины закрытия подключений.</para>
  /// </summary>
  public enum DisconnectReason
  {

    /// <summary>
    /// <para>Причина не указана/не установлена.</para>
    /// </summary>
    NONE = 0,
    /// <summary>
    /// <para>Подключение закрыто после того, как пользователь обновил страницу в браузере.</para>
    /// </summary>
    ON_REFRESH = 1,
    /// <summary>
    /// <para>Подключение закрыто после того, как пользователь перешел по другому адресу.</para>
    /// </summary>
    ON_BROWSE = 2,
    /// <summary>
    /// <para>Подключение закрыто по причине автоматического реконнекта по команде сервера.</para>
    /// </summary>
    ON_RECONNECT = 3,
    /// <summary>
    /// <para>Подключение было закрыто принудительно после аварийного завершения работы приложения/браузера/и т.п.</para>
    /// </summary>
    ON_ERROR = 4,
    /// <summary>
    /// <para>Подключение было закрыто после закрытия окна/вкладки браузера.</para>
    /// </summary>
    ON_EXIT = 5,
    /// <summary>
    /// <para>Другая причина.</para>
    /// </summary>
    ON_OTHER = 10,

  }
}
