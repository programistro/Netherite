namespace Netherite.Domain;

public class Task
{
    private Task(Guid id, string title, string description, string icon, int reward)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.Icon = icon;
        this.Reward = reward;
    }

    public Guid Id { get; set; }

    public string Title { get; set; } = (string) null;

    public string Description { get; set; } = (string) null;

    public string Icon { get; set; } = (string) null;

    public int Reward { get; set; } = 0;

    public static (Task Task, string Error) Create(
        Guid id,
        string title,
        string description,
        string icon,
        int reward)
    {
        string str = string.Empty;
        if (string.IsNullOrEmpty(title))
            str = "Title can't be empty";
        if (string.IsNullOrEmpty(description))
            str = "Description can't be empty";
        return (new Task(id, title, description, icon, reward), str);
    }
}