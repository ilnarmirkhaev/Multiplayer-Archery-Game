using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerControls : MonoBehaviour
    {
        public PlayerInput playerInput;

        public InputAction MoveAction { get; private set; }
        public InputAction RunAction { get; private set; }
        public InputAction JumpAction { get; private set; }
        public InputAction FireAction { get; private set; }
        public InputAction AimAction { get; private set; }

        private const string Move = "Move";
        private const string Run = "Run";
        private const string Jump = "Jump";
        private const string Fire = "Fire";

        private void Awake()
        {
            MoveAction = playerInput.actions[Move];
            RunAction = playerInput.actions[Run];
            JumpAction = playerInput.actions[Jump];
            FireAction = playerInput.actions[Fire];
        }
    }
}