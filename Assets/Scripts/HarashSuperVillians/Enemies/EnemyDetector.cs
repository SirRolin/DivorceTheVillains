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
    [Serialize]
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
            if(Physics.Raycast(ray, out RaycastHit hit, viewDistance, ignoreRaycasts)) {
                Debug.DrawRay(ray.origin, hit.point - ray.origin,  Color.green, 1);
                if(hit.collider.gameObject.Equals(player)){
                    detected = true;
                    break;
                }
            } else {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
            }
        }

        // Alert
        if(detected) OnDetect?.Invoke( player);
    }

    void Start()
    {
        ignoreRaycasts = LayerMask.NameToLayer("TransparentFX");
        Debug.Log(ignoreRaycasts); // 1
        OnDetect += (go) => {Debug.Log("Detected player: " + go.name);};
    }
}
