using Microsoft.Xna.Framework;
using Monocle;
using Celeste.Mod.SpeedrunTool.RoomTimer;

namespace Celeste.Mod.AxiomeToolbox.Midpoint;

[Tracked]
public class MidpointTrigger : Entity {

    private readonly Color beamColor;
    private bool triggered = false;
    private float flash = 0f;

    private const float Width = 11f;
    private const float Height = 17f;

    public MidpointTrigger(Vector2 position, Color color) : base(position) {
        beamColor = color;

        Collider = new Hitbox(Width, Height, -Width / 2f, -Height / 2f);
        Depth = -10000;
    }

    public override void Added(Scene scene) {
        base.Added(scene);
        triggered = false;
    }

    public override void Update() {
        base.Update();

        Player player = CollideFirst<Player>();
        if (player != null && !triggered) {
            triggered = true;
            flash = 1f;

            RoomTimerManager.UpdateTimerState();
            Audio.Play("event:/game/general/assist_screenbottom");
        }

        if (flash > 0f) flash -= Engine.DeltaTime * 3f;
    }

    public override void Render() {
        base.Render();

        float alpha = triggered ? 0.3f : 0.5f;
        alpha += flash * 0.6f;

        float x = Position.X - Width / 2f;
        float y = Position.Y - Height;

        Draw.Rect(x, y, Width, Height, beamColor * alpha);
        Draw.HollowRect(x, y, Width, Height, beamColor * (triggered ? 0.6f : 1f));
    }
}