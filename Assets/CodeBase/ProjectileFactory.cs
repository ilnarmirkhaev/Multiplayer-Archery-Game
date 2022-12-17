using CodeBase.Network;
using CodeBase.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace CodeBase
{
    public class ProjectileFactory : NetworkSingleton<ProjectileFactory>
    {
        [SerializeField] private Arrow arrowPrefab;
        
        public void FireLocallyAndSendRpc(Vector3 position, Vector3 direction, float power)
        {
            // CreateArrow(ownerId, position, direction, power).Launch();
            FireServerRpc(position, direction, power);
        }

        [ServerRpc(RequireOwnership = false)]
        public void FireServerRpc(Vector3 position, Vector3 direction, float power, ServerRpcParams serverRpcParams = default)
        {
            // FireClientRpc(serverRpcParams.Receive.SenderClientId, position, direction, power);
            CreateArrow(serverRpcParams.Receive.SenderClientId, position, direction, power).Launch();
        }

        // [ClientRpc]
        // private void FireClientRpc(ulong senderId, Vector3 position, Vector3 direction, float power)
        // {
        //     // if (NetworkManager.Singleton.LocalClientId == senderId) return;
        //     Debug.Log($"Local: {NetworkManager.LocalClientId}, owner: {senderId}");
        //
        //     CreateArrow(senderId, position, direction, power).Launch();
        // }

        private Arrow CreateArrow(ulong senderId, Vector3 position, Vector3 direction, float force)
        {
            var arrow = Instantiate(arrowPrefab, position, Quaternion.LookRotation(direction));
            arrow.Initialize(senderId, force);
            arrow.networkObject.Spawn();
            return arrow;
        }
    }
}