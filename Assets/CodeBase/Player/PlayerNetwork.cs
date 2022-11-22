using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Player
{
    public class PlayerNetwork : NetworkBehaviour
    {
	    [SerializeField] private Transform followTarget;

	    private readonly NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);

        private Transform _transform;

        private void Start()
		{
			_transform = transform;
			
			if (!IsOwner) return;
			CinemachineVirtualCamera cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
			cinemachineCamera.Follow = followTarget;
			cinemachineCamera.LookAt = followTarget;
			GetComponent<PlayerInput>().camera = Camera.main;
		}

        private void Update()
        {
            if (IsOwner)
            {
	            _netState.Value = new PlayerNetworkData()
	            {
		            Position = _transform.position,
		            Rotation = _transform.rotation.eulerAngles
	            };
            }
            else
            {
                _transform.position = _netState.Value.Position;
                _transform.rotation = Quaternion.Euler(_netState.Value.Rotation);
            }
        }
    }
}