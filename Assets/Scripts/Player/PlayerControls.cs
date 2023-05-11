using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerControls : MonoBehaviour
    {
        public PlayerInput playerInput;

        public InputAction MoveAction { get; private set; }
        public InputAction LookAction { get; private set; }
        public InputAction RunAction { get; private set; }
        public InputAction JumpAction { get; private set; }
        public InputAction FireAction { get; private set; }
        public InputAction AimAction { get; private set; }

        private const string Move = "Move";
        private const string Look = "Look";
        private const string Run = "Run";
        private const string Jump = "Jump";
        private const string Fire = "Fire";

        private void Awake()
        {
            MoveAction = playerInput.actions[Move];
            LookAction = playerInput.actions[Look];
            RunAction = playerInput.actions[Run];
            JumpAction = playerInput.actions[Jump];
            FireAction = playerInput.actions[Fire];
        }

        public Vector2 GetPlayerMovement() => MoveAction.ReadValue<Vector2>();

        public Vector2 GetRotationDelta() => LookAction.ReadValue<Vector2>();

        public bool JumpedThisFrame() => JumpAction.triggered;

        public bool IsHoldingRun() => RunAction.inProgress;

        public bool IsHoldingFire() => FireAction.inProgress;
    }
}