using System;
using Unity.Netcode;
using UnityEngine;

namespace CodeBase.Player
{
    public class NetworkHealth : NetworkBehaviour
    {
        [HideInInspector] public NetworkVariable<int> HitPoints = new NetworkVariable<int>();

        public event Action HitPointsDepleted;
        public event Action HitPointsRestored;

        private void OnEnable()
        {
            HitPoints.OnValueChanged += HitPointsChanged;
        }

        private void OnDisable()
        {
            HitPoints.OnValueChanged -= HitPointsChanged;
        }

        private void HitPointsChanged(int previousValue, int newValue)
        {
            Debug.Log("Health changed");
            if (previousValue > 0 && newValue <= 0)
            {
                HitPointsDepleted?.Invoke();
            }
            else if (previousValue <= 0 && newValue > 0)
            {
                HitPointsRestored?.Invoke();
            }
        }
    }
}