using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Player;
using UnityEngine;

namespace Photon
{
    [RequireComponent(typeof(PlayerControls))]
    public class InputProvider : SimulationBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private PlayerControls controls;

        private void OnEnable()
        {	
            Debug.Log("DADADADA");
            if (Runner == null) return;
			
            Debug.Log("DADADADA");
			Runner.AddCallbacks(this);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();
            if (controls.MoveAction.inProgress)
            {
                var value = controls.MoveAction.ReadValue<Vector2>();
                data.direction += new Vector3(value.x, 0, value.y);
            }

            if (controls.FireAction.triggered) data.fired = true;
            if (controls.JumpAction.triggered) data.jumped = true;

            input.Set(data);
        }

        private void OnDisable()
        {
            if (Runner != null) Runner.RemoveCallbacks(this);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress,
            NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}