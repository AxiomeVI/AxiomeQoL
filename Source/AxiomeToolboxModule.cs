using System;
using Celeste.Mod.AxiomeToolbox.Menu;
using FMOD.Studio;
using Celeste.Mod.AxiomeToolbox.Checkpoint;

namespace Celeste.Mod.AxiomeToolbox;

public class AxiomeToolboxModule : EverestModule {
    public static AxiomeToolboxModule Instance { get; private set; }

    public override Type SettingsType => typeof(AxiomeToolboxModuleSettings);
    public static AxiomeToolboxModuleSettings Settings => (AxiomeToolboxModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(AxiomeToolboxModuleSession);
    public static AxiomeToolboxModuleSession Session => (AxiomeToolboxModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(AxiomeToolboxModuleSaveData);
    public static AxiomeToolboxModuleSaveData SaveData => (AxiomeToolboxModuleSaveData) Instance._SaveData;

    public AxiomeToolboxModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(AxiomeToolboxModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(AxiomeToolboxModule), LogLevel.Info);
#endif
    }

    public override void Load() {
        On.Celeste.Level.Update += Level_OnUpdate;
        On.Celeste.Level.End    += OnLevelEnd;
        CheckpointPlacementManager.Load();
    }

    public override void Unload() {
        On.Celeste.Level.Update -= Level_OnUpdate;
        On.Celeste.Level.End    -= OnLevelEnd;
        CheckpointPlacementManager.Unload();
    }

    public override void CreateModMenuSection(TextMenu menu, bool inGame, EventInstance pauseSnapshot)
    {
        CreateModMenuSectionHeader(menu, inGame, pauseSnapshot);
        ModMenuOptions.CreateMenu(menu);
        CreateModMenuSectionKeyBindings(menu, inGame, pauseSnapshot);
    }

    private static void Level_OnUpdate(On.Celeste.Level.orig_Update orig, Level self) {
        orig(self);
        if (!Settings.Enabled) return;
        
        CheckpointPlacementManager.Update(self);

        if (!Settings.StopTimerWhenPaused) return;

        if (self.Paused || self.wasPaused) 
            self.TimerStopped = true;
        else 
            self.TimerStopped = false;
    }

    private void OnLevelEnd(On.Celeste.Level.orig_End orig, Level self) {
        orig(self);
        if (Settings.Enabled) CheckpointPlacementManager.ClearAll();
    }
}