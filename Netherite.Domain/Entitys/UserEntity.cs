namespace Netherite.Domain.Entitys;

public class UserEntity
{
    public Guid Id { get; set; }

    public Decimal Balance { get; set; }

    public string Location { get; set; } = (string) null;

    public Guid? InvitedId { get; set; } = new Guid?(Guid.Empty);

    public bool IsPremium { get; set; }

    public string TelegramId { get; set; } = (string) null;

    public string TelegramName { get; set; } = (string) null;

    public string Wallet { get; set; } = (string) null;

    public int Profit { get; set; }

    public ICollection<UserTaskEntity> UserTasks { get; set; } = (ICollection<UserTaskEntity>) new List<UserTaskEntity>();

    public ICollection<FavoritesEntity> Favorites { get; set; } = (ICollection<FavoritesEntity>) new List<FavoritesEntity>();
}