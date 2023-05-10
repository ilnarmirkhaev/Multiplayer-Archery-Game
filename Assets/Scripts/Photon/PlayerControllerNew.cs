﻿using Fusion;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Photon
{
    public class PlayerControllerNew : NetworkBehaviour
    {
        [SerializeField] private NetworkCharacterControllerPrototype controller;
        [SerializeField] private PlayerControls controls;
        [SerializeField] private CharacterAnimation characterAnimation;

        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float runningSpeed = 10.0f;
        [SerializeField] private float jumpHeight = 1.0f;

        private Transform _cameraTransform;
        private const float GravityValue = -9.81f;

        private Vector3 _playerVelocity;
        private bool _isPlayerGrounded;

        private Vector2 _movementInput;
        private bool _isMoving;
        private bool _isRunPressed;

        private void Awake()
        {
            if (Camera.main != null) _cameraTransform = Camera.main.transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override void Spawned()
        {
            if (!HasStateAuthority)
            {
                this.enabled = false;
                controls.enabled = false;
                controls.playerInput.enabled = false;
            }
        }

        private void Start() =>
            SubscribeToInput();

        private void OnDisable() =>
            UnsubscribeFromInput();

        private void Update()
        {
            _isPlayerGrounded = controller.IsGrounded;
            if (_isPlayerGrounded && _playerVelocity.y < 0)
                _playerVelocity.y = -0.05f;

            HandleGravity();
            Move();

            HandleRotation();
            HandleAnimation();
        }

        private void Move() =>
            controller.Move(ConvertToCameraSpace(_playerVelocity) * Time.deltaTime);

        private void HandleRotation() =>
            transform.rotation = Quaternion.AngleAxis(_cameraTransform.eulerAngles.y, Vector3.up);

        private void Jump(InputAction.CallbackContext _)
        {
            if (_isPlayerGrounded)
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * GravityValue);
        }

        private void HandleGravity() =>
            _playerVelocity.y += GravityValue * Time.deltaTime;

        private void HandleAnimation()
        {
            var isWalking = characterAnimation.IsWalking();
            var isRunning = characterAnimation.IsRunning();

            if (_isMoving && !isWalking)
                characterAnimation.StartWalking();
            else if (!_isMoving && isWalking)
                characterAnimation.StopWalking();

            if (_isMoving && _isRunPressed && !isRunning)
                characterAnimation.StartRunning();
            else if ((!_isMoving || !_isRunPressed) && isRunning)
                characterAnimation.StopRunning();
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
            _playerVelocity.x = _movementInput.x;
            _playerVelocity.z = _movementInput.y;
            _isMoving = _movementInput.x != 0 || _movementInput.y != 0;
        }

        private void OnRunPressed(InputAction.CallbackContext context) =>
            _isRunPressed = context.ReadValueAsButton();

        private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
        {
            var currentY = vectorToRotate.y;

            Vector3 cameraForward = _cameraTransform.forward;
            Vector3 cameraRight = _cameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            var speed = _isRunPressed ? runningSpeed : playerSpeed;

            Vector3 rotatedToCameraSpace =
                (vectorToRotate.z * cameraForward + vectorToRotate.x * cameraRight) * speed;
            rotatedToCameraSpace.y = currentY;
            return rotatedToCameraSpace;
        }

        public void SubscribeToInput()
        {
            controls.MoveAction.started += OnMovementInput;
            controls.MoveAction.performed += OnMovementInput;
            controls.MoveAction.canceled += OnMovementInput;

            controls.RunAction.started += OnRunPressed;
            controls.RunAction.canceled += OnRunPressed;

            controls.JumpAction.performed += Jump;
        }

        public void UnsubscribeFromInput()
        {
            controls.MoveAction.started += OnMovementInput;
            controls.MoveAction.performed += OnMovementInput;
            controls.MoveAction.canceled += OnMovementInput;

            controls.RunAction.started += OnRunPressed;
            controls.RunAction.canceled += OnRunPressed;

            controls.JumpAction.performed += Jump;
        }

    }
}