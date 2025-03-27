namespace Exercises;

public class UserContext : IUserContext
{
    private readonly string _username;
    private readonly string _role;

    public UserContext(string username, string role)
    {
        _username = username;
        _role = role;
    }

    public string GetUsername()
    {
        return _username;
    }

    public string GetRole()
    {
        return _role;
    }
}