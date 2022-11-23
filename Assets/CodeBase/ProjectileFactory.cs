using Unity.Netcode;
using UnityEngine;

namespace CodeBase
{
    public class ProjectileFactory : NetworkBehaviour
    {
        private static ProjectileFactory _instance;

        public static ProjectileFactory Singleton => _instance;

        [SerializeField] private Arrow arrowPrefab;

        private bool _isInitialized;

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public override void OnNetworkSpawn()
        {
            InitializePool();
        }

        public void FireLocallyAndSendRpc(Vector3 position, Vector3 direction, float power)
        {
            // CreateArrow(ownerId, position, direction, power).Launch();
            FireServerRpc(position, direction, power);
        }

        [ServerRpc(RequireOwnership = false)]
        private void FireServerRpc(Vector3 position, Vector3 direction, float power, ServerRpcParams serverRpcParams = default)
        {
            FireClientRpc(serverRpcParams.Receive.SenderClientId, position, direction, power);
        }

        [ClientRpc]
        private void FireClientRpc(ulong senderId, Vector3 position, Vector3 direction, float power)
        {
            // if (NetworkManager.Singleton.LocalClientId == senderId) return;
            Debug.Log($"Local: {NetworkManager.LocalClientId}, owner: {senderId}");

            CreateArrow(senderId, position, direction, power).Launch();
        }

        private Arrow CreateArrow(ulong senderId, Vector3 position, Vector3 direction, float force)
        {
            var arrow = Instantiate(arrowPrefab, position, Quaternion.LookRotation(direction));
            arrow.Initialize(senderId, force);
            return arrow;
        }

        private void InitializePool()
        {
            if (_isInitialized) return;
            _isInitialized = true;
        }
    }
}