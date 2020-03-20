using Tiny2D;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Tiny;
using Unity.Tiny.Rendering;
using Unity.U2D.Entities.Physics;

namespace Unity.TinyGems
{
    public class InputToActionSystem : JobComponentSystem
    {
        private EntityQuery m_CellQuery;
        private EntityQuery m_CameraQuery;
        
        private float2 m_StartPosition;
        
        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            m_CameraQuery = GetEntityQuery(typeof(CameraMatrices));
            
            //RequireSingletonForUpdate<ActiveInput>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var input = World.GetExistingSystem<Tiny.Input.InputSystem>();
            var physicsWorld = World.GetExistingSystem<PhysicsWorldSystem>().PhysicsWorld;
            
            if (InputUtil.GetInputUp(input))
            {
                var inputPos = CameraUtil.ScreenPointToViewportPoint(EntityManager, InputUtil.GetInputPosition(input));
                
                Debug.Log("inputPos" + inputPos);
                
                var cameraMatrices = m_CameraQuery.ToComponentDataArray<CameraMatrices>(Allocator.TempJob);
                var resultPos = CameraUtil.ViewPortPointToNearClipPoint(cameraMatrices[0], inputPos);
                
                var pointInput = new OverlapPointInput()
                {
                    Position = resultPos,
                    Filter = CollisionFilter.Default
                };
                
                if (physicsWorld.OverlapPoint(pointInput, out var overlapPointHit))
                {
                    var body = physicsWorld.AllBodies[overlapPointHit.PhysicsBodyIndex];
                    Debug.Log("input hit physicsbody " + body.Entity.Index);
                    var hexagonState = GetSingleton<ColorComponent>();
                    hexagonState.Flag = !hexagonState.Flag;
                    SetSingleton<ColorComponent>(hexagonState);
                }

                cameraMatrices.Dispose();
            }

            return inputDeps;
        }

    }
}
