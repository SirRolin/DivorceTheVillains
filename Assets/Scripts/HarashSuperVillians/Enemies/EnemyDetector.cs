using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyDetector : MonoBehaviour, IEnemyDetector
{
    public event Action<GameObject> OnDetect;
    public Transform eyes;
    [SerializeField]
    private float viewDistance = 25f;

    [Header("Constants")]
    private int ignoreRaycasts;
    private Ray ray;

    public void TryDetect(GameObject player, List<Transform> detectspots)
    {
        // If parameters are not set, return
        if(eyes == null || detectspots == null || detectspots.Count == 0) return;

        // Check if any of the spots can be spotted
        bool detected = false;
        foreach(Transform spot in detectspots){
            ray.origin = eyes.position;
            ray.direction = spot.position - eyes.position;
            if (Physics.Raycast(ray, out RaycastHit hit, viewDistance, ignoreRaycasts) && hit.collider.gameObject.Equals(player))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, 1);
                detected = true;
                // Alert
                OnDetect?.Invoke(player);
                break;
            }
            else if (hit.collider != null)
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 1);
            }
            else
            {
                Debug.DrawRay(ray.origin, Vector3.Normalize(spot.position - ray.origin) * viewDistance, Color.red, 1);
            }
        }
    }

    void Start()
    {
        ignoreRaycasts = ~LayerMask.GetMask("TransparentFX", "ignoreRayCast", "UI", "Water");
        //OnDetect += (go) => {Debug.Log("Detected player: " + go.name);};
    }
}
