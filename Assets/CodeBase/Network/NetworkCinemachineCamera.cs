using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Network
{
    public class NetworkCinemachineCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        private void Start() =>
            NetworkManager.Singleton.OnClientConnectedCallback += _ => ProvidePlayerToCamera();

        private void ProvidePlayerToCamera()
        {
            var playerObjectTransform = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().transform;
            cinemachineVirtualCamera.Follow = playerObjectTransform;
            cinemachineVirtualCamera.LookAt = playerObjectTransform;
            
            playerObjectTransform.GetComponent<PlayerInput>().camera = Camera.main;
        }
    }
}