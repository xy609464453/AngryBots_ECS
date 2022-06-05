using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateAfter(typeof(MoveForwardSystem))]
public partial class TimedDestroySystem : SystemBase
{
    private EntityCommandBufferSystem _bufferSystem;

    protected override void OnCreate()
    {
        _bufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var buffer = _bufferSystem.CreateCommandBuffer().AsParallelWriter();
        var deltaTime = Time.DeltaTime;

        this.Entities
            .ForEach((int entityInQueryIndex, Entity entity, ref TimeToLive timeToLive) => {
                timeToLive.Value -= deltaTime;
                if (timeToLive.Value <= 0f)
                    buffer.DestroyEntity(entityInQueryIndex, entity);
            })
            .Schedule();
        _bufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}

