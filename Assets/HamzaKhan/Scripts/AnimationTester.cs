using System.Collections;
using UnityEngine;

namespace CricketBowlingAnimations
{
    public class AnimationTester : MonoBehaviour
    {
        // Settings
        [Header("Settings")]
        [SerializeField] private Animator animator;
        [SerializeField] private string animatorBowlingTriggerName;
        [SerializeField] private string animatorBowlingTypeName;
        [SerializeField] public BowlingAnimation bowlingAnimation;
        [SerializeField] private bool useRootMotion = true;

        public GameObject Ball;

        /// <summary>
        /// Bowling Animation Enum has all the possible bowling types.
        /// </summary>
        public enum BowlingAnimation
        {
            LeftArmFastBowler,
            LeftArmMediumFastBowler,
            LeftArmOrthodoxSpinner,
            LeftArmWristSpinner,
            RightArmFastBowler,
            RightArmMediumFastBowler,
            RightArmLegSpinner,
            RightArmOffSpinner,
        }

        // Reference to the original position.
        [Header("Original Position")]
        [SerializeField] public Transform originalPositionTransform; // Drag your Transform here in the Inspector

        private Vector3 originalPosition;

        // Notes
        [Header("NOTE")]
        [TextArea]
        public string Note;

        // Awake
        private void Awake()
        {
            if (originalPositionTransform != null)
            {
                originalPosition = originalPositionTransform.position;
            }
            else
            {
                Debug.LogError("Original Position Transform is not assigned.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Change root motion mode.
            animator.applyRootMotion = useRootMotion;

            // Set the float in the animator correctly.
            animator.SetFloat(animatorBowlingTypeName, (int)bowlingAnimation + 1);

            // Only play the animation if the Space key is pressed
            if (Input.GetKeyDown(KeyCode.Space) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Bowling"))
            {
                PlayBowlingAnimation(); // Call the new method when the spacebar is pressed
            }
        }

        /// <summary>
        /// This method can be called by a UI button or spacebar to play the bowling animation.
        /// </summary>
        public void PlayBowlingAnimation()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Bowling"))
            {
                StartCoroutine(PlayAnimationAndReturnToPosition());
                StartCoroutine(removeBallAfterTime(1.85f));
            }
        }

        public void SetBowlerPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            originalPosition = newPosition;
        }

        private IEnumerator PlayAnimationAndReturnToPosition()
        {
            animator.SetTrigger(animatorBowlingTriggerName);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;

            yield return new WaitForSeconds(animationLength);

            transform.position = originalPosition;
        }

        private IEnumerator removeBallAfterTime(float delay)
        {
            yield return new WaitForSeconds(delay);
            Ball.SetActive(false);
        }
    }
}
