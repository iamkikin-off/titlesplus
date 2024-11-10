using GDWeave;
using TitlesPatching;

namespace TitleAPI;

public class Mod : IMod {
    public Config Config;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new PlayerLabel());
        modInterface.RegisterScriptMod(new SteamNetwork());
        modInterface.RegisterScriptMod(new PlayerHud());
        modInterface.Logger.Information("hotdogs");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
