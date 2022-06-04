using Unity.Burst;
using Unity.Collections;
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

        var job = new TurnJob
        {
            playerPosition = Settings.PlayerPosition
        };

        this.Dependency = job.Schedule(this.Dependency);
    }
}

[BurstCompile]
[WithAll(typeof(EnemyTag))]
public partial struct TurnJob : IJobEntity
{
    public float3 playerPosition;

    public void Execute([ReadOnly] ref Translation pos, ref Rotation rot)
    {
        float3 heading = playerPosition - pos.Value;
        heading.y = 0f;
        rot.Value = quaternion.LookRotation(heading, math.up());
    }
}

