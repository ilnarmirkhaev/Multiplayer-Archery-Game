using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace CodeBase
{
    public class PlayerAttack : MonoBehaviour
    {
        public PlayerControls controls;
        public CharacterController controller;

        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private Transform shootingPoint;

        [SerializeField] private float firePower = 100f;
        [SerializeField] private float reloadTime = 0.5f;
        [SerializeField] private bool isPrecise;

        private const float MinCharge = 0.25f;
        private const float MaxCharge = 1.1f;
        private const float ChargeTime = 0.75f;
        private const float ChargeMultiplier = (MaxCharge - MinCharge) * (1 / ChargeTime);

        private Arrow _currentArrow;
        private Camera _camera;

        private bool _isReloading;
        private float _currentCharge;
        private bool _isCharging;

        private Ray _aimRay;
        private readonly Vector3 _screenCenter = new Vector3(0.5f,0.5f, 0);

        private void Awake() =>
            _camera = Camera.main;

        private Ray AimRay() =>
            _camera.ViewportPointToRay(_screenCenter);

        private void Start()
        {
            SubscribeToInput();

            CreateArrow();
            _currentCharge = MinCharge;
        }

        private void OnDisable() =>
            UnsubscribeFromInput();

        private void Update()
        {
            ChargeArrow();
            RotateArrowWithCamera();
            Debug.DrawRay(shootingPoint.position, _aimRay.direction, Color.red);
        }

        private void ChargeArrow()
        {
            if (_isCharging && !_isReloading)
                _currentCharge = Mathf.Clamp(_currentCharge + ChargeMultiplier * Time.deltaTime, MinCharge, MaxCharge);
        }

        private void StartChargingArrow(CallbackContext _) =>
            _isCharging = true;

        private void ReleaseArrow(CallbackContext _)
        {
            if (_isReloading) return;

            Debug.Log($"Fire: {_currentCharge}");
            Debug.DrawRay(_aimRay.origin, _aimRay.direction, Color.red, 1f);

            _currentArrow.transform.SetParent(null);

            _currentArrow.Fire(_currentCharge * firePower);
            _currentArrow = null;

            _currentCharge = MinCharge;
            _isCharging = false;

            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            _isReloading = true;
            yield return new WaitForSeconds(reloadTime);
            _isReloading = false;
            CreateArrow();
        }

        private void RotateArrowWithCamera()
        {
            if (_currentArrow is null) return;

            _aimRay = AimRay();
            _currentArrow.transform.rotation = Quaternion.LookRotation(_aimRay.direction);
        }

        private void CreateArrow()
        {
            _currentArrow = Instantiate(arrowPrefab, shootingPoint);
            _currentArrow.Initialize(controller, isPrecise);
        }

        private void SubscribeToInput()
        {
            controls.fireAction.started += StartChargingArrow;
            controls.fireAction.canceled += ReleaseArrow;
        }

        private void UnsubscribeFromInput()
        {
            controls.fireAction.started -= StartChargingArrow;
            controls.fireAction.canceled -= ReleaseArrow;
        }
    }
}