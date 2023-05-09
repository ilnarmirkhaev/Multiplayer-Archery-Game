using Core;
using Unity.Netcode;
using UnityEngine;
using Weapons;

public class ProjectileFactory : NetworkSingleton<ProjectileFactory>
{
    [SerializeField] private Arrow arrowPrefab;
        
    //TODO: попробуй отдельный метод для обращения, а рпц вызвать в этом методе
        
    [ServerRpc(RequireOwnership = false)]
    public void FireServerRpc(Vector3 position, Vector3 direction, float force, ServerRpcParams serverRpcParams = default)
    {
        SpawnArrow(serverRpcParams.Receive.SenderClientId, position, direction, force);
    }

    private void SpawnArrow(ulong senderId, Vector3 position, Vector3 direction, float force)
    {
        if (!IsServer) return;
            
        var arrow = Instantiate(arrowPrefab, position, Quaternion.LookRotation(direction));
        arrow.Initialize(senderId, force);
        arrow.networkObject.Spawn();
        arrow.Launch();
    }
}