using Fusion;
using UnityEngine;
using Cinemachine;
using Player;
using System.Threading.Tasks;
using VContainer;

namespace Photon
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField] private int maxHealth = 300;
		[SerializeField] private Animator animator;
		
		private Vector3 _spawnPoint;
        private CinemachineVirtualCamera _playerCamera;

        [Networked(OnChanged = nameof(HpChanged))] public int HitPoints { get; set; }

        [Inject]
        private void Inject(HealthDisplay display, CinemachineVirtualCamera playerCamera)
        {
            Display = display;
            Display.Init(maxHealth);
            _playerCamera = playerCamera;
        }

        private HealthDisplay Display { get; set; }

        public override void Spawned()
        {
            HitPoints = maxHealth;
			_spawnPoint = transform.position;
        }

        public static void HpChanged(Changed<PlayerHealth> changed)
        {
            var behaviour = changed.Behaviour;
            if (behaviour.HasInputAuthority)
                changed.Behaviour.Display.UpdateHealth(behaviour.HitPoints);
			if (changed.Behaviour.HitPoints <= 0)
			{
				changed.Behaviour.animator.SetDeath();
				changed.Behaviour.OnDied();
			}
        }
		
		private async void OnDied()
		{
			gameObject.SetActive(false);
			_playerCamera.gameObject.SetActive(false);
			await Task.Delay(5000);
			gameObject.SetActive(true);
			transform.position = _spawnPoint;
			HitPoints = maxHealth;
			animator.ResetDeath();
			_playerCamera.gameObject.SetActive(true);
		}
    }
}