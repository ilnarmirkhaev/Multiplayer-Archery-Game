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
	
	public static class AnimatorExtensions
	{
        private static readonly int WalkingHash = Animator.StringToHash("isWalking");
        private static readonly int RunningHash = Animator.StringToHash("isRunning");
        private static readonly int OnDiedHash = Animator.StringToHash("OnDied");
		
		public static void SetWalking(this Animator animator, bool value) => animator.SetBool(WalkingHash, value);
		public static void SetRunning(this Animator animator, bool value) => animator.SetBool(RunningHash, value);
		public static bool IsWalking(this Animator animator) => animator.GetBool(WalkingHash);
		public static bool IsRunning(this Animator animator) => animator.GetBool(RunningHash);
		public static void SetDeath(this Animator animator) => animator.SetTrigger(OnDiedHash);
		public static void ResetDeath(this Animator animator) => animator.ResetTrigger(OnDiedHash);
	}
}