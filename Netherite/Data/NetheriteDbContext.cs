using Microsoft.EntityFrameworkCore;
using Netherite.Domain.Entitys;

namespace Netherite.Data;

public class NetheriteDbContext : DbContext
{
    public NetheriteDbContext(DbContextOptions<NetheriteDbContext> options)
        : base((DbContextOptions) options)
    {
    }

    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<TaskEntity> Tasks { get; set; }

    public DbSet<MinerEntity> Miners { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<UserTaskEntity> UserTasks { get; set; }

    public DbSet<IntervalEntity> Intervals { get; set; }

    public DbSet<CurrencyPairsEntity> CurrencyPairs { get; set; }

    public DbSet<CurrencyPairsIntervalEntity> CurrencyPairsIntervals { get; set; }

    public DbSet<FavoritesEntity> Favorites { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=data.db");
    }
}