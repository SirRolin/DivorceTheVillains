using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.HarashSuperVillains.Player {
    public class DeathManager : MonoBehaviour
    {
        [SerializeField]
        [Range(-100f,10f)]
        float deathAltitude = 0f;
        public event Action OnDeath;
        public event Action<DeathManager> OnPreDeath;
        public bool isDead = false;

        // Update is called once per frame
        void Update()
        {
            // if position is lower than altitude check, and you're not already dead, you die.
            if (!isDead && transform.position.y < deathAltitude){
                isDead = true;
                OnPreDeath?.Invoke(this);
                if(isDead) OnDeath?.Invoke();
            }
        }
    }
}
