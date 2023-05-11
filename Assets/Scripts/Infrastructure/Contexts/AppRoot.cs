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
            DontDestroyOnLoad(gameObject);

            builder.RegisterComponent(playerCamera);
            
            builder.RegisterBuildCallback(resolver =>
            {
                PlayerControllerN.PlayerSpawned += resolver.Inject;
            });
        }
    }
}