using UnityEngine;
using Assets.Scripts.HarashSuperVillains.Objects;

public class InteractableDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable inter;
        if(!TryGetComponent<Interactable>(out inter))
        {
            inter = gameObject.AddComponent<Interactable>();
            inter.inactiveOpacity = 1;
            inter.activeOpacity = 1;
            inter.randomiseColors = false;
        }

        inter.AddInteraction(
            "",
            () => {
                ToggleDoor();
                return false;
            });
    }

    // Update is called once per frame
    void ToggleDoor()
    {
        
    }
}
