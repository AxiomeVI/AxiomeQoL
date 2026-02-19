using Monocle;

namespace Celeste.Mod.AxiomeToolbox.Menu;

public static class ModMenuOptions {
    private static AxiomeToolboxModuleSettings _settings = AxiomeToolboxModule.Settings;

    public static void CreateMenu(TextMenu menu)
    {     
        menu.Add(new TextMenu.OnOff(Dialog.Clean(DialogIds.StopTimerWhenPaused), _settings.StopTimerWhenPaused).Change(
            value =>
            {
                _settings.StopTimerWhenPaused = value;
                if (!value && Engine.Scene is Level level) {
                    level.TimerStopped = false;
                }
            }
        ));
    }
}