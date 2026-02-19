using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.AxiomeToolbox;
[SettingName(DialogIds.AxiomeToolboxId)]
public class AxiomeToolboxModuleSettings : EverestModuleSettings {

    public bool Enabled { get; set; } = true;
    public bool StopTimerWhenPaused { get; set; } = false;

    [SettingName(DialogIds.PlaceMidPointId)]
    [DefaultButtonBinding(0, Keys.None)]
    public ButtonBinding PlaceCheckpoint { get; set; }

    [SettingName(DialogIds.ClearCheckpointId)]
    [DefaultButtonBinding(0, Keys.None)]
    public ButtonBinding ClearCheckpoints { get; set; }

    [SettingName("Checkpoint Color")]
    [SettingSubText("Color of the in-game checkpoint beam")]
    public string CheckpointColor { get; set; } = "00FFFF";  // Cyan
}