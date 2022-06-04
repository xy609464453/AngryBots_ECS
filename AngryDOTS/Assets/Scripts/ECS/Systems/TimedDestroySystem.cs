using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateAfter(typeof(MoveForwardSystem))]
public partial class TimedDestroySystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem buffer;

    protected override void OnCreate()
    {
        buffer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    partial struct CullingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter commands;
        public float dt;

        public void Execute([EntityInQueryIndex] int jobIndex, Entity entity, ref TimeToLive timeToLive)
        {
            timeToLive.Value -= dt;
            if (timeToLive.Value <= 0f)
                commands.DestroyEntity(jobIndex, entity);
        }
    }

    protected override void OnUpdate()
    {
        var job = new CullingJob
        {
            commands = buffer.CreateCommandBuffer().AsParallelWriter(),
            dt = Time.DeltaTime
        };

        this.Dependency = job.Schedule(this.Dependency);
        buffer.AddJobHandleForProducer(this.Dependency);
    }
}

