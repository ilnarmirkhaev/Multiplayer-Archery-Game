using UnityEngine;

namespace Weapons
{
    public interface IWeapon
    {
        void Fire(Vector3 from, Vector3 direction);
        void Reload();
    }

    public interface IChargeableWeapon : IWeapon
    {
        float Power { get; }
        void Charge();
        void Discharge();
        void StartCharging();
    }
}