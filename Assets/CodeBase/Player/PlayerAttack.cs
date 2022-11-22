using Unity.Netcode;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace CodeBase.Player
{
    public class PlayerAttack : NetworkBehaviour, IInputHandler
    {
        public PlayerControls controls;

        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private Transform shootingPoint;

        [SerializeField] private float firePower = 100f;
        [SerializeField] private float angle = 3.5f;

        private readonly Vector3 _screenCenter = new(0.5f, 0.5f, 0);
        private Camera _camera;

        private IChargeableWeapon _bow;
        private ProjectileFactory _factory;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) enabled = false;

            _camera = Camera.main;

            _factory = new ProjectileFactory(arrowPrefab);

            _bow = new Bow(_factory, OwnerClientId, firePower);
            _bow.Discharge();


            SubscribeToInput();
        }

        public void SubscribeToInput()
        {
            controls.fireAction.started += OnFireActionStarted;
            controls.fireAction.canceled += OnFireActionCanceled;
        }

        private void OnDisable() =>
            UnsubscribeFromInput();

        public void UnsubscribeFromInput()
        {
            controls.fireAction.started -= OnFireActionStarted;
            controls.fireAction.canceled -= OnFireActionCanceled;
        }

        private void OnFireActionStarted(CallbackContext _) =>
            _bow.StartCharging();

        private void Update() =>
            _bow.Charge();

        private void OnFireActionCanceled(CallbackContext _) =>
            _bow.Fire(shootingPoint.position, ShootingDirection());

        private Vector3 ShootingDirection() =>
            Quaternion.AngleAxis(-angle, shootingPoint.right) * AimRay().direction;

        private Ray AimRay() =>
            _camera.ViewportPointToRay(_screenCenter);
    }
}