using ModuleLauncher.NET.Models.Authentication;

namespace ModuleLauncher.NET.Manager;

public abstract class MinecraftAccount
{
    public string Name { get; protected set; }

    public AuthenticationCredential AuthenticationCredential;

    public abstract void Authenticate(string code,
        AuthenticationCredential.AuthenticationRefreshRequired? authenticationRefreshRequired = null);

    public static implicit operator AuthenticateResult(MinecraftAccount account)
    {
        return account.AuthenticationCredential;
    }
}