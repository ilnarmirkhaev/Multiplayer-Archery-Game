using Fusion;
using UnityEngine;

namespace Photon
{
    public class PlayerControllerN : NetworkBehaviour
    {
        [SerializeField] private NetworkCharacterControllerPrototype controller;
        
        
        public override void FixedUpdateNetwork()
        {
            // if (GetInput(out NetworkInputData))
        }
    }
}