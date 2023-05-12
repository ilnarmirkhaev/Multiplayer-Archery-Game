using Fusion;
using UnityEngine;

namespace Photon
{
    public class PlayerHealth : NetworkBehaviour
    {
        [Networked(OnChanged = "HitPointsChanged")] public int HitPoints { get; set; }

        public override void Spawned()
        {
            HitPoints = 100;
        }

        public static void HitPointsChanged(Changed<PlayerHealth> changed)
        {
            Debug.Log(changed.Behaviour.HitPoints);
        }
    }
}