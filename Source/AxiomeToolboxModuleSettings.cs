using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.AxiomeToolbox;
[SettingName(DialogIds.AxiomeToolboxId)]
public class AxiomeToolboxModuleSettings : EverestModuleSettings {

    public bool Enabled { get; set; } = true;
    public bool StopTimerWhenPaused { get; set; } = false;

    [SettingName(DialogIds.PlaceMidPointId)]
    [DefaultButtonBinding(0, Keys.None)]
    public ButtonBinding PlaceMidpoint { get; set; }

    [SettingName(DialogIds.ClearMidpointId)]
    [DefaultButtonBinding(0, Keys.None)]
    public ButtonBinding ClearMidpoints { get; set; }

    [SettingName("Midpoint Color")]
    [SettingSubText("Color of the in-game midpoint beam")]
    public string MidpointColor { get; set; } = "00FFFF";  // Cyan
}