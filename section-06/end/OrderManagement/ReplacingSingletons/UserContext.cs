namespace OrderManagement.ReplacingSingletons;

public interface IUserContext
{
    string GetUsername();
    string GetRole();
}

public class UserContext : IUserContext
{
    private string _username;
    private string _role;
    public void SetUser(string username, string role)
    {
        _username = username;
        _role = role;
    }

    public string GetUsername() => _username;
    public string GetRole() => _role;

}