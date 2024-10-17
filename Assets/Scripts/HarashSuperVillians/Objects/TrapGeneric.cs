using UnityEngine;
using System.Collections;
using Assets.Scripts.HarashSuperVillains.Objects;
using Assets.Scripts.HarashSuperVillains.Player;
using Unity.VisualScripting;
using UnityEditor.Animations;

public class PlayerTrapGeneric : MonoBehaviour
    {
        [Header("Triggering")]
        [SerializeField]
        private bool closeOnTime = true;
        [SerializeField]
        private bool closeOnExit = true;
        [SerializeField]
        private float waitTime = 2f;
        [SerializeField]
        private float triggerTimeBeforeHit = 1f;

        [SerializeField]
        private bool isPlayerTrap = false;
        
        [SerializeField]
        private float angerOnTrapped = 10f;

        [Header("Activation")]
        [SerializeField]
        private string isActivatedBy = "";
        [SerializeField]
        private bool consumesItem = true;
        
        [Header("Triggering")]
        [SerializeField]
        private Collider triggerCollider;
        private Animator trapAnimator;
        [SerializeField]
        private bool hasBeenActivated = false;
        private bool stillColliding = false;
        private GameObject triggerer;

        [Header("Animations")]
        [SerializeField]
        private AnimationClip armedAnimation;
        [SerializeField]
        private AnimationClip unarmedAnimation;
        [SerializeField]
        private AnimationClip triggeredAnimation;
        [SerializeField]
        private bool triggeredAnimationReversed = false;
        [SerializeField]
        private AnimationClip personOnTriggeredAnimation;

    
        void Awake()
        {
            // Get the Animator component from the trap
            trapAnimator = GetComponent<Animator>();
            if(trapAnimator == null){
                trapAnimator = gameObject.AddComponent<Animator>();
                trapAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Base Trap");
                //Debug.LogError(this + " is missing an animator!");
            }
            AnimatorOverrideController aoc = new(trapAnimator.runtimeAnimatorController);
            trapAnimator.runtimeAnimatorController = aoc;
            if(armedAnimation != null) aoc["armed"] = armedAnimation;
            if(unarmedAnimation != null) aoc["unarmed"] = unarmedAnimation;
            if(triggeredAnimation != null) aoc["triggered"] = triggeredAnimation;
            
            Interactable inter;
            if(!TryGetComponent<Interactable>(out inter))
                inter = gameObject.AddComponent<Interactable>();

            inter.AddInteraction(
                isActivatedBy,
                () => {
                    SetActivated(true);
                    return isActivatedBy != "" ? consumesItem : true;
                });
        }
    
        void OnTriggerEnter(Collider other)
        {
            if (hasBeenActivated && ShouldTrigger(other))
            {
                stillColliding = true;
                triggerer = other.gameObject;
                StartCoroutine(OpenCloseTrap());
            }
        }
    
        void OnTriggerExit(Collider other)
        {
            if (hasBeenActivated && ShouldTrigger(other))
            {
                stillColliding = false;
                StopCoroutine(OpenCloseTrap());
                if(closeOnExit)
                    trapAnimator.SetTrigger("close");
            }
        }

        private bool ShouldTrigger(Collider other){
            return (triggerCollider == null || other.Equals(triggerCollider)) && ((other.CompareTag("Player") && isPlayerTrap) || (other.CompareTag("Enemy") && !isPlayerTrap));
        }

        public bool GetActivated(){
            return hasBeenActivated;
        }

        public bool SetActivated(bool activated){
            bool output = hasBeenActivated == activated;
            hasBeenActivated = activated;
            if(trapAnimator != null){
                if(activated){
                    trapAnimator.Play("armed");
                } else {
                    trapAnimator.Play("unarmed");
                }
            }
            return output;
        }
    
        IEnumerator OpenCloseTrap(){
            // Play open animation
            //trapAnimator?.SetTrigger("open");
            trapAnimator.Play("triggered");
            if(triggeredAnimationReversed) trapAnimator.speed = -1;

            // Wait for the close animation to finish
            yield return new WaitForSeconds(triggerTimeBeforeHit);

            // Check if the player is still on the trap
            if (stillColliding)
            {
                if(isPlayerTrap){
                    // Trigger the player's death using DeathManager
                    if(triggerer.TryGetComponent<DeathManager>(out DeathManager death)) death.Die();
                } else {
                    if(triggerer.TryGetComponent<Anger>(out Anger angst)){
                        angst.ApplyAnger(angerOnTrapped);
                    }
                }
                if(triggerer.TryGetComponent<Animator>(out Animator ani)) {
                    AnimatorOverrideController aoc = new(ani.runtimeAnimatorController);
                    ani.runtimeAnimatorController = aoc;
                    aoc["triggeredTrap"] = personOnTriggeredAnimation;
                    ani.Play("triggeredTrap");
                }
            }

            // should it close after it has been triggered?
            if(closeOnTime){
                // Wait waitTime seconds
                yield return new WaitForSeconds(waitTime - triggerTimeBeforeHit);
                // Play close animation
                trapAnimator?.Play("close");
            }
        }
    }