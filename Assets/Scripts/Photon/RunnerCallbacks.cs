using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Player;
using UnityEngine;

namespace Photon
{
    public class RunnerCallbacks : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private PlayerControls controls;
        
        public void SubscribeToInput()
        {
            // controls.MoveAction.started += OnMovementInput;
            // controls.MoveAction.performed += OnMovementInput;
            // controls.MoveAction.canceled += OnMovementInput;
            //
            // controls.RunAction.started += OnRunPressed;
            // controls.RunAction.canceled += OnRunPressed;
            //
            // controls.JumpAction.performed += Jump;
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();
            if (controls.MoveAction.inProgress)
            {
                // data.direction += controls.MoveAction.ReadValue<Vector2>();
            }
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