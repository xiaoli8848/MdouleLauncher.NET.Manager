using Manganese.Data;
using ModuleLauncher.NET.Authentications;
using ModuleLauncher.NET.Models.Authentication;
using Timer = System.Timers.Timer;

namespace ModuleLauncher.NET.Manager;

public class AuthenticationCredential : AuthenticateResult
{
    public delegate Task<AuthenticateResult>? AuthenticationRefreshRequired(AuthenticationCredential sender);

    public event AuthenticationRefreshRequired OnRefreshRequired = async (credential) => await credential.Authenticator.AuthenticateAsync();

    private System.Timers.Timer? RefreshTimer;

    public IAuthenticator Authenticator { get; }

    public AuthenticationCredential(IAuthenticator authenticator)
    {
        Authenticator = authenticator;
        var task = Authenticator.AuthenticateAsync();
        task.Wait();
        CopyFromAuthenticateResult(task.Result);
        if (ExpireIn == TimeSpan.Zero) return;
        RefreshTimer = new Timer(ExpireIn.TotalMilliseconds);
        RefreshTimer.Elapsed += async (sender, args) =>
        {
#pragma warning disable CS8602
            CopyFromAuthenticateResult(await OnRefreshRequired(this));
#pragma warning restore CS8602
        };
        RefreshTimer.AutoReset = true;
        RefreshTimer.Enabled = true;
    }

    public AuthenticationCredential(IAuthenticator authenticator, AuthenticateResult authenticateResult)
    {
        Authenticator = authenticator;
        CopyFromAuthenticateResult(authenticateResult);
        if (ExpireIn == TimeSpan.Zero) return;
        RefreshTimer = new Timer(ExpireIn.TotalMilliseconds);
        RefreshTimer.Elapsed += async (sender, args) =>
        {
#pragma warning disable CS8602
            CopyFromAuthenticateResult(await OnRefreshRequired(this));
#pragma warning restore CS8602
        };
        RefreshTimer.AutoReset = true;
        RefreshTimer.Enabled = true;
    }

    private void CopyFromAuthenticateResult(AuthenticateResult result)
    {
        Name = result.Name;
        UUID = result.UUID;
        AccessToken = result.AccessToken;
        RefreshToken = result.RefreshToken;
        ExpireIn = result.ExpireIn;
        ClientToken = result.ClientToken;
    }
}