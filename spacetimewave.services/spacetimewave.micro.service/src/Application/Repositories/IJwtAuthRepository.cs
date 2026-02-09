namespace Application.Repositories;

public interface IJwtAuthRepository
{
    string GenerateJwtToken(string audience, Dictionary<string, string> claims, int expirationSeconds);
    Dictionary<string, string> GetJwtClaims(string token);
    IEnumerable<object> GetIssuerJwks();
    IEnumerable<string> GetIssuerPublicKeys();

    string Base64UrlEncode(byte[] input);
    byte[] Base64UrlDecode(string input);
}