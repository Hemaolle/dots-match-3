using Unity.Entities;

namespace Unity.TinyGems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SetupGameSystem : ComponentSystem
    {
        protected override void OnStartRunning()
        {
            EntityManager.CreateEntity(typeof(ColorComponent));
        }

        protected override void OnUpdate()
        {
        }
    }
}