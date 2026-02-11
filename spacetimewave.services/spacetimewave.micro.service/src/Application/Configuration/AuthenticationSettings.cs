namespace Application.Configuration;

public class AuthenticationSettings
{
    public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.None;
    public string Realm { get; set; } = string.Empty; // Tenant or realm
    public string MetadataUrl { get; set; } = string.Empty;
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
}


public enum AuthenticationType
{
    None,
    JwtBearer
}