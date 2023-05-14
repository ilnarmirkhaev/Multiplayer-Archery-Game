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

        public Vector3 MovementDirection { get; set; }

        [Inject]
        private void Inject(CinemachineVirtualCamera playerCamera)
        {
            _playerCamera = playerCamera;
            _playerCamera.Follow = lookPoint;
            _playerCamera.gameObject.SetActive(true);
        }

        public override void Spawned()
        {
            if (HasInputAuthority) PlayerSpawned?.Invoke(this);
            
            _transform = transform;
            _rotationSpeed = controller.rotationSpeed;
            _zeroRotation = lookPoint.localRotation;
        }

        public override void FixedUpdateNetwork()
        {
            Vector3 direction;
            Quaternion lookRotation;
            float angleX;
            var from = lookPoint.localRotation;
            
            if (GetInput(out NetworkInputData data))
            {
                direction = _transform.TransformDirection(data.direction).normalized;
                MovementDirection = direction;
                
                angleX = data.lookDelta.x;
                lookRotation = from * Quaternion.AngleAxis(-data.lookDelta.y, Vector3.right);
                
                if (data.jumped) controller.Jump();
            }
            else
            {
                direction = MovementDirection;
                angleX = 0;
                lookRotation = lookPoint.localRotation;
            }
            
            controller.Move(direction);
            controller.RotateY(angleX);
            if (Quaternion.Angle(_zeroRotation, lookRotation) < 90f)
                lookPoint.localRotation = Quaternion.Slerp(from, lookRotation, _rotationSpeed * Runner.DeltaTime);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playerCamera.gameObject.SetActive(false);
        }
    }
}