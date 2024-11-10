using GDWeave;

namespace TitlesPlus;

public class Mod : IMod {

    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new SteamNetwork());
        modInterface.RegisterScriptMod(new PlayerHud());
        modInterface.Logger.Information("hotdogs");
    }

    public void Dispose() {
        
    }
}
