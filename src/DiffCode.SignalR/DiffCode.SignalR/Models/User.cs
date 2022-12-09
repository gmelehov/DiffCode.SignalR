using Microsoft.AspNetCore.Identity;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;




namespace DiffCode.SignalR.Models
{
  /// <summary>
  /// <para>Пользователь.</para>
  /// </summary>
  public class User : IdentityUser<int>
  {
    public User()
    {
      Connections = new List<Connection>();
    }






    /// <summary>
    /// <para>Возвращает результат проверки, является ли этот пользователь анонимным.</para>
    /// </summary>
    /// <returns></returns>
    public bool IsAnonymous() => UserName?.Trim().ToLower() == "anonymous";







    /// <summary>
    /// <para>Дата и время последнего посещения страницы сайта этим пользователем.</para>
    /// <para>Равно <see langword="null"/>, если пользователь сейчас онлайн.</para>
    /// </summary>
    public DateTime? LastOnlineOn { get; set; }

    /// <summary>
    /// <para>Текущий статус сетевого подключения пользователя.</para>
    /// </summary>
    [NotMapped]
    public bool IsOnline => Connections?.Any(a => a.IsActive) ?? false;

    /// <summary>
    /// <para>
    /// Количество текущих подключений, открытых пользователем 
    /// (количество вкладок браузера с любой страницей сайта, открытых под именем этого пользователя).
    /// </para>
		/// </summary>
    [NotMapped]
    public int ActiveConns => Connections?.Count(w => w.IsActive) ?? 0;






    /// <summary>
    /// <para>Список всех подключений (в том числе неактивных), открытых этим пользователем.</para>
    /// </summary>
    [JsonIgnore]
    public virtual List<Connection> Connections { get; set; }

  }
}
