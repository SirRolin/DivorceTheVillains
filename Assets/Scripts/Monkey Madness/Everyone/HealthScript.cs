using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.MonkeyMadness.Everyone {
    public class HealthScript : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth = 100;
        [SerializeField]
        private float health = 100;

        public event Action<float> onTakeDamage;
        public event Action onDeath;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            health = maxHealth;
            onTakeDamage += OnTakeDamage;
        }

        // Update is called once per frame
        void Update()
        {
            //if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            if(Input.GetKeyDown(KeyCode.N))
            {
                if (onTakeDamage != null)
                {
                    onTakeDamage.Invoke(33f);
                    Debug.Log("dealing: " + 33 + " damage.");
                }
                if (health <= 0)
                {
                    if (onDeath != null)
                        onDeath.Invoke();
                }
                Debug.Log("health:" + health + "/" + maxHealth);
            }
        }

        public void OnTakeDamage(float health)
        {
            this.health -= health;
        }
    }
}