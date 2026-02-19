using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AxiomeToolbox.Checkpoint {

    public static class CheckpointPlacementManager {

        private static readonly Dictionary<string, List<Vector2>> placements = [];

        public static void Load() {
            On.Celeste.Level.LoadLevel += OnLoadLevel;
        }

        public static void Unload() {
            On.Celeste.Level.LoadLevel -= OnLoadLevel;
        }

        public static void ClearAll() {
            placements.Clear();
        }

        public static void Update(Level level) {
            var settings = AxiomeToolboxModule.Instance._Settings as AxiomeToolboxModuleSettings;
            
            if (settings?.PlaceCheckpoint.Pressed ?? false) {
                PlaceCheckpointAtPlayer(level);
            }

            if (settings?.ClearCheckpoints.Pressed ?? false) {
                ClearRoomCheckpoints(level);
            }
        }

        private static void PlaceCheckpointAtPlayer(Level level) {
            Player player = level.Tracker.GetEntity<Player>();
            if (player == null) return;

            string roomID = GetRoomID(level);
            Vector2 pos = player.Position;

            if (!placements.ContainsKey(roomID))
                placements[roomID] = [];
            placements[roomID].Add(pos);

            var settings = AxiomeToolboxModule.Instance._Settings as AxiomeToolboxModuleSettings;
            Color color = Calc.HexToColor(settings?.CheckpointColor ?? "00FFFF");
            level.Add(new CheckpointTrigger(pos, color));

            Audio.Play("event:/game/general/strawberry_blue_touch");
        }

        private static void ClearRoomCheckpoints(Level level) {
            string roomID = GetRoomID(level);

            placements.Remove(roomID);

            foreach (var checkpoint in level.Tracker.GetEntities<CheckpointTrigger>()) {
                checkpoint.RemoveSelf();
            }

            Audio.Play("event:/ui/main/button_back");
        }

        private static void OnLoadLevel(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader) {
            orig(self, playerIntro, isFromLoader);

            string roomID = GetRoomID(self);
            if (placements.TryGetValue(roomID, out var positions)) {
                var settings = AxiomeToolboxModule.Instance._Settings as AxiomeToolboxModuleSettings;
                Color color = Calc.HexToColor(settings?.CheckpointColor ?? "00FFFF");

                foreach (var pos in positions) {
                    self.Add(new CheckpointTrigger(pos, color));
                }
            }
        }

        private static string GetRoomID(Level level) {
            AreaKey area = level.Session.Area;
            return $"{area.SID ?? area.ID.ToString()}:{level.Session.Level}";
        }
    }
}