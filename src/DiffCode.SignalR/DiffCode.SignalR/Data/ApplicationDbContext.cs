using DiffCode.SignalR.Enums;
using DiffCode.SignalR.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;



namespace DiffCode.SignalR.Data
{
  /// <summary>
  /// <para>Контекст базы данных.</para>
  /// </summary>
  public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
    {
      Database.EnsureCreated();
    }








    public DbSet<Connection> Connections { get; set; }







    protected override void OnModelCreating(ModelBuilder mb)
    {
      base.OnModelCreating(mb);


      mb.Entity<Connection>(ent =>
      {
        ent.HasKey(k => k.Id);
        ent.HasOne(h => h.Caller).WithMany(m => m.Connections).HasForeignKey(f => f.CallerId).OnDelete(DeleteBehavior.NoAction);
        ent.Property(p => p.ConnectReason).HasConversion(new EnumToStringConverter<ConnectReason>());
        ent.Property(p => p.DisconnectReason).HasConversion(new EnumToStringConverter<DisconnectReason>());
        ent.HasOne(h => h.Previous).WithOne().HasForeignKey<Connection>(f => f.PrevId);
      });


      mb.Entity<User>(ent =>
      {
        ent.ToTable("Users");
      });

    }









    /// <summary>
    /// Закрывает "зависшие" подключения, оставшиеся после аварийного завершения работы веб-приложения.
    /// </summary>
    public void CloseHangedConnections()
    {
      foreach (var conn in Connections.AsEnumerable().Where(w => w.ClosedOn == null && w.IsActive == true && w.IsForcedClosed == false))
      {
        conn.CloseIfHanged();
      };
      SaveChanges();
    }



  }
}