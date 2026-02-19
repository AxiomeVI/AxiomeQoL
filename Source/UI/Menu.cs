using Celeste.Mod.AxiomeToolbox.Midpoint;
using Monocle;

namespace Celeste.Mod.AxiomeToolbox.Menu;

public static class ModMenuOptions {
    private static AxiomeToolboxModuleSettings _settings = AxiomeToolboxModule.Settings;

    public static void CreateMenu(TextMenu menu)
    {   
        TextMenu.OnOff _stopTimerWhenPaused = (TextMenu.OnOff)new TextMenu.OnOff(
            Dialog.Clean(DialogIds.StopTimerWhenPaused), 
            _settings.StopTimerWhenPaused).Change(
                value =>
                {
                    _settings.StopTimerWhenPaused = value;
                    if (!value && Engine.Scene is Level level) {
                        level.TimerStopped = false;
                    }
                }
        );

        menu.Add(new TextMenu.OnOff(Dialog.Clean(DialogIds.EnabledId), _settings.Enabled).Change(
            value =>
            {
                _settings.Enabled = value;
                _stopTimerWhenPaused.Visible = value;
                if (!value) {
                    MidpointPlacementManager.ClearAll();
                    if (_settings.StopTimerWhenPaused && Engine.Scene is Level level) {
                        level.TimerStopped = false;
                    }
                }
            }
        ));

        menu.Add(_stopTimerWhenPaused);
    }
}