namespace Million.Application.Ports.Interfaces
{
    /// <summary>Puerto para generación de JWT (infra lo implementa)</summary>
    public interface IJwtTokenService
    {
        string GenerateToken(string username, IEnumerable<string> roles, TimeSpan? lifetime = null);
    }
}

