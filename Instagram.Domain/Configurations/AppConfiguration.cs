namespace Instagram.Domain.Configurations;

public class AppConfiguration
{
    public string AppName { get; set; } = null!;
    public ApplicationConfiguration Application { get; set; } = null!;
    public OidcConfiguration Oidc { get; set; } = null!;
    public ConnectionsConfiguration Connections { get; set; } = null!;
    public FileProvidersConfiguration FileProviders { get; set; } = null!;
}



