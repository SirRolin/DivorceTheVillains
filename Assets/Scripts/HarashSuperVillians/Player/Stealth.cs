using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.HarashSuperVillains.Player {
    public class Stealth : MonoBehaviour
    {
        [SerializeField]
        public List<Transform> detectSpots = new();
        private readonly List<IEnemyDetector> enemiesInRange = new();

        void OnTriggerEnter(Collider other){
            if(!other.gameObject.TryGetComponent<IEnemyDetector>(out var ed)) return;
            enemiesInRange.Add(ed);
        }

        void OnTriggerExit(Collider other){
            if(!other.gameObject.TryGetComponent<IEnemyDetector>(out var ed)) return;
            enemiesInRange.Remove(ed);
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if(detectSpots.Count == 0)
                detectSpots.Add(transform);
            InvokeRepeating(nameof(CheckForEnemies), 1.0f, 1.0f);
        }

        void CheckForEnemies(){
            enemiesInRange.ForEach(x => {
                x.TryDetect(gameObject, detectSpots);
                });
        }
    }
}