using ModuleLauncher.NET.Models.Resources;
using ModuleLauncher.NET.Utilities;

namespace ModuleLauncher.NET.Manager;

public sealed class Minecraft : MinecraftEntry
{
    public string Id => Json.Id;

    public MinecraftType Type => this.GetMinecraftType();
    
    public MinecraftJsonType JsonType => Json.Type ?? MinecraftJsonType.Release;

    public DirectoryInfo Root => Tree.Root;

    public DirectoryInfo VersionRoot => Tree.VersionRoot;

    public List<MinecraftModEntry> Mods
    {
        get => Tree.Mods.GetFiles()
            .Where(x => string.Compare(x.Extension, ".jar", StringComparison.OrdinalIgnoreCase) == 0 ||
                        string.Compare(x.Extension, ".DISABLED", StringComparison.OrdinalIgnoreCase) == 0)
            .Select(item => MinecraftModEntry.Parse(item)).ToList();
    }
}