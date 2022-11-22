using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Player
{
    public class Bow : IChargeableWeapon
    {
        private const float ReloadTime = 0.5f;
        private const float ChargeTime = 0.75f;
        private const float MinCharge = 0.25f;
        private const float MaxCharge = 1.1f;
        private const float ChargeMultiplier = (MaxCharge - MinCharge) * (1 / ChargeTime);

        private bool _isReloading;
        private bool _isCharging;
        private float _currentCharge = MinCharge;

        private readonly ProjectileFactory _factory;
        private readonly ulong _ownerClientId;
        private readonly float _firePower;

        public Bow(ProjectileFactory factory, ulong ownerClientId, float firePower)
        {
            _factory = factory;
            _ownerClientId = ownerClientId;
            _firePower = firePower;
        }

        public float Power =>
            _currentCharge * _firePower;

        public void StartCharging() =>
            _isCharging = true;

        public void Charge()
        {
            if (_isCharging && !_isReloading)
                _currentCharge = Mathf.Clamp(_currentCharge + ChargeMultiplier * Time.deltaTime, MinCharge, MaxCharge);
        }

        public void Fire(Vector3 from, Vector3 direction)
        {
            if (_isReloading) return;

            Debug.Log($"Fire power: {_currentCharge}");
            _factory.FireServerRpc(_ownerClientId, from, direction, Power);

            Discharge();
            Reload();
        }

        public void Discharge()
        {
            _currentCharge = MinCharge;
            _isCharging = false;
        }

        public async void Reload()
        {
            _isReloading = true;
            await Task.Delay((int)(ReloadTime * 1000));
            _isReloading = false;
        }

        private void FireLocalAndServer(ulong localClientId, Vector3 shootingPosition, Vector3 direction, float power)
        {
            Arrow arrow = _factory.CreateArrow(localClientId, shootingPosition, direction, power);
            arrow.Launch();

            _factory.FireServerRpc(localClientId, shootingPosition, direction, power);
        }
    }
}