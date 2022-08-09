using ModuleLauncher.NET.Authentications;

namespace ModuleLauncher.NET.Manager;

public sealed class OfflineMinecraftAccount : MinecraftAccount
{
    public new string Name { get; }

    public override void Authenticate(string? code,
        AuthenticationCredential.AuthenticationRefreshRequired? authenticationRefreshRequired = null)
    {
        var offlineAuthenticator = new OfflineAuthenticator(Name);
        AuthenticationCredential = new AuthenticationCredential(offlineAuthenticator);
    }

    public OfflineMinecraftAccount(string name)
    {
        Name = name;
        Authenticate(null);
    }
}