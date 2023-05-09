using UnityEngine;

namespace Player
{
    public class CharacterAnimation : MonoBehaviour
    {
        public Animator animator;
        
        private static readonly int WalkingHash = Animator.StringToHash("isWalking");
        private static readonly int RunningHash = Animator.StringToHash("isRunning");

        public void StartWalking() => animator.SetBool(WalkingHash, true);
        public void StopWalking() => animator.SetBool(WalkingHash, false);
        
        public void StartRunning() => animator.SetBool(RunningHash, true);
        public void StopRunning() => animator.SetBool(RunningHash, false);

        public bool IsWalking() => animator.GetBool(WalkingHash);
        public bool IsRunning() => animator.GetBool(RunningHash);
    }
}