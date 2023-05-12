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
        private float _rotationSpeed;
        private Quaternion _zeroRotation;

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
            _rotationSpeed = controller.rotationSpeed;
            _zeroRotation = lookPoint.localRotation;
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data)) return;
            
            var moveDirection = _transform.TransformDirection(data.direction.normalized);
            controller.Move(moveDirection);
            controller.RotateY(data.lookDelta.x);

            var from = lookPoint.localRotation;
            var to = from * Quaternion.AngleAxis(-data.lookDelta.y, Vector3.right);
            if (Quaternion.Angle(_zeroRotation, to) < 90f)
                lookPoint.localRotation = Quaternion.Slerp(from, to, _rotationSpeed * Runner.DeltaTime);

            if (data.jumped) controller.Jump();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playerCamera.gameObject.SetActive(false);
        }
    }
}