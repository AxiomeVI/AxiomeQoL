using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.AxiomeToolbox.Midpoint {

    public static class MidpointPlacementManager {

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
            
            if (settings?.PlaceMidpoint.Pressed ?? false) {
                PlaceMidpointAtPlayer(level);
            }

            if (settings?.ClearMidpoints.Pressed ?? false) {
                ClearRoomMidpoints(level);
            }
        }

        private static void PlaceMidpointAtPlayer(Level level) {
            Player player = level.Tracker.GetEntity<Player>();
            if (player == null) return;

            string roomID = GetRoomID(level);
            Vector2 pos = player.Position;

            if (!placements.ContainsKey(roomID))
                placements[roomID] = [];
            placements[roomID].Add(pos);

            var settings = AxiomeToolboxModule.Instance._Settings as AxiomeToolboxModuleSettings;
            Color color = Calc.HexToColor(settings?.MidpointColor ?? "00FFFF");
            level.Add(new MidpointTrigger(pos, color));

            Audio.Play("event:/game/general/strawberry_blue_touch");
        }

        private static void ClearRoomMidpoints(Level level) {
            string roomID = GetRoomID(level);

            placements.Remove(roomID);

            foreach (var midpoint in level.Tracker.GetEntities<MidpointTrigger>()) {
                midpoint.RemoveSelf();
            }

            Audio.Play("event:/ui/main/button_back");
        }

        private static void OnLoadLevel(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader) {
            orig(self, playerIntro, isFromLoader);

            string roomID = GetRoomID(self);
            if (placements.TryGetValue(roomID, out var positions)) {
                var settings = AxiomeToolboxModule.Instance._Settings as AxiomeToolboxModuleSettings;
                Color color = Calc.HexToColor(settings?.MidpointColor ?? "00FFFF");

                foreach (var pos in positions) {
                    self.Add(new MidpointTrigger(pos, color));
                }
            }
        }

        private static string GetRoomID(Level level) {
            AreaKey area = level.Session.Area;
            return $"{area.SID ?? area.ID.ToString()}:{level.Session.Level}";
        }
    }
}