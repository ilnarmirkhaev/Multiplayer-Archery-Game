using CodeBase.Player;
using Unity.Netcode;
using UnityEngine;

namespace CodeBase
{
    public class ProjectileFactory
    {
        private readonly Arrow _arrowPrefab;

        public ProjectileFactory(Arrow arrowPrefab)
        {
            _arrowPrefab = arrowPrefab;
        }

        public Arrow CreateArrow(ulong ownerClientId, Vector3 position, Vector3 direction, float force)
        {
            var arrow = Object.Instantiate(_arrowPrefab, position, Quaternion.LookRotation(direction));
            arrow.Initialize(ownerClientId, force);
            return arrow;
        }

        [ServerRpc]
        public void FireServerRpc(ulong ownerId, Vector3 position, Vector3 direction, float power)
        {
            FireClientRpc(ownerId, position, direction, power);
        }

        [ClientRpc]
        private void FireClientRpc(ulong ownerId, Vector3 position, Vector3 direction, float power)
        {
            Arrow arrow = CreateArrow(ownerId, position, direction, power);
            arrow.Launch();
        }
    }
}