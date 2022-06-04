using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(CollisionSystem))]
public partial class PlayerTransformUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Settings.IsPlayerDead())
            return;

        this.Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation pos) => {
                pos = new Translation { Value = Settings.PlayerPosition };
            })
            .Run();
    }
}