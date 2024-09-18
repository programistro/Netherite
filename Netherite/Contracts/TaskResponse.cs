namespace Netherite.Contracts;

public record TasksResponse(Guid Id, string Title, string Description, string Icon, int Reward);