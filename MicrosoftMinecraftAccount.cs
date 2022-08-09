using ModuleLauncher.NET.Manager;
using ModuleLauncher.NET.Models.Authentication;
using ModuleLauncher.NET.Models.Utils;
using ModuleLauncher.NET.Utilities;

namespace ModuleLauncher.NET.Authentications;

public sealed class MicrosoftMinecraftAccount : MinecraftAccount
{
    public new string Name => AuthenticationCredential.Name!;


    public bool IsAvailable => !string.IsNullOrEmpty(AuthenticationCredential.AccessToken) &&
                               !string.IsNullOrEmpty(AuthenticationCredential.UUID);

    public override void Authenticate(string code,
        AuthenticationCredential.AuthenticationRefreshRequired? authenticationRefreshRequired = null)
    {
        AuthenticationCredential = new AuthenticationCredential(new MicrosoftAuthenticator(){Code = code});
        if (authenticationRefreshRequired is not null)
            AuthenticationCredential.OnRefreshRequired += authenticationRefreshRequired;
        else
            AuthenticationCredential.OnRefreshRequired += async (authentication) =>
                await authentication.Authenticator.RefreshAuthenticateAsync(authentication.RefreshToken!);;
    }

    public MicrosoftMinecraftAccount(string code,
        AuthenticationCredential.AuthenticationRefreshRequired? authenticationRefreshRequired = null)
    {
        Authenticate(code, authenticationRefreshRequired);
    }

    public async Task<string?> GetUuidByUsernameAsync()
    {
        return IsAvailable ? await MojangApiUtils.GetUuidByUsernameAsync(Name) : null;
    }
    
    public async Task<MinecraftProfile?> ChangeUsernameAsync(string newName)
    {
        return IsAvailable
            ? await MojangApiUtils.ChangeUsernameAsync(AuthenticationCredential.AccessToken!, newName)
            : null;
    }
    
    public async Task<bool?> CheckNameAvailabilityAsync(string name)
    {
        return IsAvailable
            ? await MojangApiUtils.CheckNameAvailabilityAsync(AuthenticationCredential.AccessToken!, name)
            : null;
    }
    
    public async Task<List<(string name, DateTime? time)>?> GetNameHistoryByUuidAsync(
        )
    {
        return IsAvailable ? await MojangApiUtils.GetNameHistoryByUuidAsync(AuthenticationCredential.UUID!) : null;
    }

    public async Task<MinecraftProfile?> GetProfileByUuidAsync()
    {
        return IsAvailable ? await MojangApiUtils.GetProfileByUuidAsync(AuthenticationCredential.UUID!) : null;
    }
    
    public async Task<(DateTime ChangedAt, DateTime CreatedAt, bool NameChangeAllowed)?>
        GetProfileNameChangeInfoAsync()
    {
        return IsAvailable
            ? await MojangApiUtils.GetProfileNameChangeInfoAsync(AuthenticationCredential.AccessToken!)
            : null;
    }
    
    public async Task<MinecraftProfile?> ChangeSkinAsync(FileInfo skinFile, SkinVariant skinVariant = SkinVariant.Classic)
    {
        return IsAvailable
            ? await SkinUtils.ChangeSkinAsync(AuthenticationCredential.AccessToken!, skinFile, skinVariant)
            : null;
    }
    
    public async Task<MinecraftProfile?> ChangeSkinAsync(string skinUrl, SkinVariant variant = SkinVariant.Classic)
    {
        return IsAvailable
            ? await SkinUtils.ChangeSkinAsync(AuthenticationCredential.AccessToken!, skinUrl, variant)
            : null;
    }
    
    public async Task<MinecraftProfile?> ShowCapeAsync(string capeId)
    {
        return IsAvailable ? await SkinUtils.ShowCapeAsync(AuthenticationCredential.AccessToken!, capeId) : null;
    }
    
    public async Task<MinecraftProfile?> HideCapeAsync()
    {
        return IsAvailable ? await SkinUtils.HideCapeAsync(AuthenticationCredential.AccessToken!) : null;
    }
    
    public async Task<MinecraftProfile?> ResetSkinAsync()
    {
        return IsAvailable ? await SkinUtils.ResetSkinAsync(AuthenticationCredential.AccessToken!) : null;
    }
}