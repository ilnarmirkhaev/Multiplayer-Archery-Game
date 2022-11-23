using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.CameraUtils
{
    public class CameraProvider : NetworkBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform followTarget;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            CinemachineVirtualCamera cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
            cinemachineCamera.Follow = followTarget;
            cinemachineCamera.LookAt = followTarget;

            playerInput.camera = Camera.main;
        }
    }
}