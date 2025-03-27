using System.Security.Claims;

namespace OrderManagement.Api;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    bool IsInRole(string role);
}

public class HttpUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public bool IsAuthenticated => 
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    
    public string UserId => 
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    public bool IsInRole(string role) => 
        _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
}