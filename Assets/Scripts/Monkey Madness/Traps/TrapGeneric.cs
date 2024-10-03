using UnityEngine;
using System.Collections;
using Assets.Scripts.HarashSuperVillains.Objects;

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
        private AnimationClip trappedAnimation;
        [SerializeField]
        private string isActivatedBy = "";
        

        private Animator trapAnimator;
        [SerializeField]
        private bool hasBeenActivated = false;
        private bool stillColliding = false;
        private GameObject triggerer;
    
        void Awake()
        {
            // Get the Animator component from the trap
            trapAnimator = GetComponent<Animator>();
            if(trapAnimator == null){
                Debug.Log(this + " is missing an animator!");
            }
            GetComponent<Interactable>().OnInteract.Add(isActivatedBy, () => SetActivated(true));
        }
    
        void OnTriggerEnter(Collider other)
        {
            if (hasBeenActivated && shouldTrigger(other))
            {
                stillColliding = true;
                triggerer = other.gameObject;
                StartCoroutine(OpenCloseTrap());
            }
        }
    
        void OnTriggerExit(Collider other)
        {
            if (hasBeenActivated && shouldTrigger(other))
            {
                stillColliding = false;
                StopCoroutine(OpenCloseTrap());
                if(closeOnExit)
                    trapAnimator.SetTrigger("close");
            }
        }

        private bool shouldTrigger(Collider other){
            return (other.CompareTag("Player") && isPlayerTrap) || (other.CompareTag("Enemy") && !isPlayerTrap);
        }

        public bool GetActivated(){
            return hasBeenActivated;
        }

        public bool SetActivated(bool activated){
            bool output = hasBeenActivated == activated;
            hasBeenActivated = activated;
            return output;
        }
    
        IEnumerator OpenCloseTrap(){
            // Play open animation
            trapAnimator?.SetTrigger("open");

            // Wait for the close animation to finish
            yield return new WaitForSeconds(triggerTimeBeforeHit);

            // Check if the player is still on the trap
            if (stillColliding)
            {
                if(isPlayerTrap){
                    // Trigger the player's death using DeathManager
                    triggerer.GetComponent<DeathManager>()?.Die();
                } else {
                    
                }
            }

            // should it close after it has been triggered?
            if(closeOnTime){
                // Wait waitTime seconds
                yield return new WaitForSeconds(waitTime - triggerTimeBeforeHit);
                // Play close animation
                trapAnimator?.SetTrigger("close");
            }
        }
    }