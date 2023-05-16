using Fusion;
using UnityEngine;
using VContainer;

namespace Photon
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField] private int maxHealth = 300;

        [Networked(OnChanged = nameof(HpChanged))] public int HitPoints { get; set; }

        [Inject]
        private void Inject(HealthDisplay display)
        {
            Display = display;
            Display.Init(maxHealth);
        }

        private HealthDisplay Display { get; set; }

        public override void Spawned()
        {
            HitPoints = maxHealth;
        }

        public static void HpChanged(Changed<PlayerHealth> changed)
        {
            var behaviour = changed.Behaviour;
            if (behaviour.HasInputAuthority)
                changed.Behaviour.Display.UpdateHealth(behaviour.HitPoints);
        }
    }
}