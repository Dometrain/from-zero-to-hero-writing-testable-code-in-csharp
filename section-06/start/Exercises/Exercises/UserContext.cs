namespace Exercises;

public class UserContext
{
    private static UserContext _instance = new UserContext();

    public static UserContext Current => _instance;

    private UserContext()
    {
        Username = "system";
        Role = "User";
    }

    public string Username { get; set; }
    public string Role { get; set; }
}