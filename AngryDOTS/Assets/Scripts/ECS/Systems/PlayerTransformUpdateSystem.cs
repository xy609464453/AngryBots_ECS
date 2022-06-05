using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(CollisionSystem))]
public partial class PlayerTransformUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Settings.IsPlayerDead())
            return;

        var playerPosition = Settings.PlayerPosition;
        this.Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation pos) => {
                pos = new Translation { Value =  playerPosition};
            })
            .Run();
    }
}