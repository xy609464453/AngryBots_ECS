using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(MoveForwardSystem))]
public partial class TurnTowardsPlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Settings.IsPlayerDead())
            return;

        var playerPosition = (float3)Settings.PlayerPosition;
        this.Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Rotation rot, in Translation pos) => {
                var heading = playerPosition - pos.Value;
                heading.y = 0f;
                rot.Value = quaternion.LookRotation(heading, math.up());
            })
            .Schedule();
    }
}

