using System;
using Cinemachine;
using Fusion;
using UnityEngine;
using VContainer;

namespace Photon
{
    public class PlayerControllerN : NetworkBehaviour
    {
        [SerializeField] private NetworkCharacterControllerPrototype controller;
        [SerializeField] private Transform lookPoint;

        private CinemachineVirtualCamera _playerCamera;
        private Transform _transform;

        public static event Action<SimulationBehaviour> PlayerSpawned;

        [Inject]
        private void Inject(CinemachineVirtualCamera playerCamera)
        {
            _playerCamera = playerCamera;
            _playerCamera.Follow = lookPoint;
            _playerCamera.gameObject.SetActive(true);
        }

        public override void Spawned()
        {
            if (!HasInputAuthority) return;
            
            PlayerSpawned?.Invoke(this);
            _transform = transform;
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data)) return;

            var wasd = data.direction.normalized;
            var direction = _transform.TransformDirection(wasd);

            controller.Move(direction * (5 * Runner.DeltaTime));

            if (data.holdingFire) Debug.Log("Fire!");
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playerCamera.gameObject.SetActive(false);
        }
    }
}