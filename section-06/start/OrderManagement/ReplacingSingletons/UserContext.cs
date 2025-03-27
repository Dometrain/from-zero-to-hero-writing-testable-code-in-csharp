namespace OrderManagement.ReplacingSingletons;

public class UserContext
{
    private static UserContext _current = new UserContext();
    
    public static UserContext Current => _current;
    
    private UserContext()
    {
        // In a real application, this would be set from HttpContext
        Username = "system";
        Role = "User";
    }
    
    public string Username { get; set; }
    public string Role { get; set; }
}