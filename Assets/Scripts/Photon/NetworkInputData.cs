using Fusion;
using UnityEngine;

namespace Photon
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 direction;
        public bool jumped;
    }
}