using UnityEngine;
using System.Collections;

public class SpikeTrapDemo : MonoBehaviour
    {
        private Animator spikeTrapAnim;
        private bool playerOnTrap = false;
        private GameObject Player;
    
        void Awake()
        {
            // Get the Animator component from the trap
            spikeTrapAnim = GetComponent<Animator>();
        }
    
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerOnTrap = true;
                Player = other.gameObject;
                StartCoroutine(OpenCloseTrap());
            }
        }
    
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerOnTrap = false;
                StopCoroutine(OpenCloseTrap());
                spikeTrapAnim.SetTrigger("close");
            }
        }
    
       IEnumerator OpenCloseTrap()
    {
        // Play open animation
        spikeTrapAnim.SetTrigger("open");
        // Wait 2 seconds
        yield return new WaitForSeconds(2);
        // Play close animation
        spikeTrapAnim.SetTrigger("close");

        // Wait for the close animation to finish
        yield return new WaitForSeconds(1);

        // Check if the player is still on the trap
        if (playerOnTrap)
        {
            // Trigger the player's death using DeathManager
            DeathManager deathManager = Player.GetComponent<DeathManager>();
            if (deathManager != null && !deathManager.isDead)
            {
               deathManager.isDead = true;
            }
        }
    }
    }