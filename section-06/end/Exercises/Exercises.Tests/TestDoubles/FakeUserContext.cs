namespace Exercises.Tests.TestDoubles;

public class FakeUserContext : IUserContext
{
    private string _username;
    private string _role;
        
    public FakeUserContext(string username, string role)
    {
        _username = username;
        _role = role;
    }
        
    public string GetUsername() => _username;
    public string GetRole() => _role;
}