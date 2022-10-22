using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase
{
    public class PlayerControls : MonoBehaviour
    {
        public PlayerInput playerInput;

        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction runAction;
        [HideInInspector] public InputAction jumpAction;
        [HideInInspector] public InputAction fireAction;
        [HideInInspector] public InputAction aimAction;

        private void Awake()
        {
            moveAction = playerInput.actions["Move"];
            runAction = playerInput.actions["Run"];
            jumpAction = playerInput.actions["Jump"];
            fireAction = playerInput.actions["Fire"];
        }
    }
}