using Celeste.Mod.AxiomeQoLModule;

namespace Celeste.Mod.AxiomeQoL;
[SettingName(DialogIds.AxiomeQoLId)]
public class AxiomeQoLModuleSettings : EverestModuleSettings {

    [SettingName(DialogIds.StopTimerWhenPaused)]
    public bool StopTimerWhenPaused { get; set; } = false;
}