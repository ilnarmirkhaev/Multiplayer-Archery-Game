using System.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Photon
{
    public class PlayerAttackN : NetworkBehaviour
    {
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private ArrowNew arrowPrefab;
        [SerializeField] private float firePower = 100f;

        private const float ReloadTime = 0.5f;
        private const float ChargeTime = 0.75f;
        private const float MinCharge = 0.25f;
        private const float MaxCharge = 1.1f;
        private const float ChargeMultiplier = (MaxCharge - MinCharge) * (1 / ChargeTime);

        private float _currentCharge;
        private bool _isCharging;
        private bool _isReloading;

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data)) return;

            var fire = data.holdingFire;
            if (!_isCharging && fire)
                StartCharging();
            else if (_isCharging)
            {
                if (fire)
                    Charge();
                else
                    Fire();
            }
        }

        private void StartCharging() => _isCharging = true;

        private void Charge()
        {
            if (!_isCharging || _isReloading) return;
            _currentCharge = Mathf.Clamp(_currentCharge + ChargeMultiplier * Runner.DeltaTime, MinCharge, MaxCharge);
        }

        private void Fire()
        {
            if (_isReloading)
            {
                if (_isCharging) Discharge();
                return;
            }

            // Debug.Log($"Fire power: {_currentCharge}");
            Runner.Spawn(
                arrowPrefab,
                shootingPoint.position,
                Quaternion.LookRotation(shootingPoint.forward),
                Object.InputAuthority,
                (runner, o) => { o.GetComponent<ArrowNew>().Initialize(_currentCharge * firePower); }
            );

            Discharge();
            Reload();
        }

        private async void Reload()
        {
            _isReloading = true;
            await Task.Delay((int)(ReloadTime * 1000));
            _isReloading = false;
        }

        private void Discharge()
        {
            _currentCharge = MinCharge;
            _isCharging = false;
        }
    }
}