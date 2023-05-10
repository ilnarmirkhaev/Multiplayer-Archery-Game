using Fusion;
using UnityEngine;

namespace Photon
{
    public class PlayerControllerN : NetworkBehaviour
    {
        [SerializeField] private NetworkCharacterControllerPrototype controller;
        
        
        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data)) return;
            
            data.direction.Normalize();
            controller.Move(5 * data.direction * Runner.DeltaTime);
            
            if (data.fired) Debug.Log("Fire!");
        }
    }
}