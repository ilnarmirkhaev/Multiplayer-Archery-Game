using Cinemachine;
using Photon;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Contexts
{
    public class AppRoot : LifetimeScope
    {
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(playerCamera);
            builder.RegisterComponentInHierarchy<HealthDisplay>();

            builder.RegisterBuildCallback(resolver =>
            {
                PlayerControllerN.PlayerSpawned += o =>
                {
                    foreach (var behaviour in o.NetworkedBehaviours)
                    {
                        resolver.Inject(behaviour);
                    }
                };
            });
            
            DontDestroyOnLoad(gameObject);
        }
    }
}