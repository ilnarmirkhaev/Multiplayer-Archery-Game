using Fusion;
using UnityEngine;

namespace Photon
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 direction;
        public NetworkBool isRunning;
        public NetworkBool jumped;
        public NetworkBool holdingFire;
    }
}