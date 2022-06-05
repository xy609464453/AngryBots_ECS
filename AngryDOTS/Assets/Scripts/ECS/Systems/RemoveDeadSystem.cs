using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class RemoveDeadSystem : SystemBase
{
    private EntityCommandBufferSystem _bufferSystem;

    protected override void OnCreate()
    {
        this._bufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var manager = this.EntityManager;
        this.Entities
            .WithoutBurst()
            .WithAll<PlayerTag>()
            .ForEach((Entity entity, ref Health health, ref Translation pos) => {
                if (health.Value <= 0)
                {
                    Settings.PlayerDied();
                }
            }).Run();

        var buffer = this._bufferSystem.CreateCommandBuffer();
        this.Entities
            .WithoutBurst()
            .WithAll<EnemyTag>()
            .ForEach((Entity entity, ref Health health, ref Translation pos) => {
                if (health.Value <= 0)
                {
                    buffer.DestroyEntity(entity);
                    BulletImpactPool.PlayBulletImpact(pos.Value);
                }
            }).Run();
    }
}