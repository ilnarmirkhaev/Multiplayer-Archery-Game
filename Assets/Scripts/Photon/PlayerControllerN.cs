using System;
using Cinemachine;
using Fusion;
using Player;
using UnityEngine;
using VContainer;

namespace Photon
{
    public class PlayerControllerN : NetworkBehaviour
    {
        [SerializeField] private NetworkCharacterControllerPrototype controller;
        [SerializeField] private Transform lookPoint;
		[SerializeField] private Animator animator;

        private CinemachineVirtualCamera _playerCamera;
        private Transform _transform;
        private Quaternion _zeroRotation;

        public static event Action<NetworkObject> PlayerSpawned;

        public Vector3 MovementDirection { get; set; }
        public Quaternion LookRotation { get; set; }

        [Inject]
        private void Inject(CinemachineVirtualCamera playerCamera)
        {
            _playerCamera = playerCamera;
            _playerCamera.Follow = lookPoint;
            _playerCamera.gameObject.SetActive(true);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override void Spawned()
        {
            if (HasInputAuthority) PlayerSpawned?.Invoke(Object);
            
            _transform = transform;
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
                LookRotation = lookRotation;

                if (data.jumped) controller.Jump();
            }
            else
            {
                direction = MovementDirection;
                angleX = 0;
                lookRotation = LookRotation;
            }
            
            controller.Move(direction);
			HandleAnimation(direction.magnitude >= 0.01f);
            controller.RotateY(angleX);
            if (Quaternion.Angle(_zeroRotation, lookRotation) < 90f && Quaternion.Angle(from, lookRotation) > .1f)
                lookPoint.localRotation = Quaternion.Slerp(from, lookRotation, controller.rotationSpeed * Runner.DeltaTime);
        }
		
		private void HandleAnimation(bool isMoving)
        {
            var isWalking = animator.IsWalking();
            var isRunning = animator.IsRunning();

            if (isMoving && !isWalking)
                animator.SetWalking(true);
            else if (!isMoving && isWalking)
                animator.SetWalking(false);

            // if (isMoving && _isRunPressed && !isRunning)
            //     animator.SetRunning(true);
            // else if ((!isMoving || !_isRunPressed) && isRunning)
            //     animator.SetRunning(false);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playerCamera.gameObject.SetActive(false);
        }
    }
}