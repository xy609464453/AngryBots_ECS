using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Transforms
{
    public partial class MoveForwardSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = Time.DeltaTime;
            this.Entities
                .WithAll<MoveForward>()
                .ForEach((ref Translation pos, in Rotation rot, in MoveSpeed speed) => {
                    pos.Value += (time * speed.Value * math.forward(rot.Value));
                })
                .Schedule();
        }
    }
}