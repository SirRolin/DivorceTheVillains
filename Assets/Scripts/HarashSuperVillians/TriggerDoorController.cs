using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    [SerializeField] private string doorOpen = "DoorOpen";
    [SerializeField] private string doorClose = "DoorClose";

    [SerializeField] private Collider door;

    private int inCollider = 0; 

    

    private void OnTriggerEnter(Collider other)
    {
        inCollider++;
        if (inCollider == 1) 
        {
            myDoor.Play(doorOpen, 0, 0.0f);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        inCollider--;
        if (inCollider == 0)
        {
            myDoor.Play(doorClose, 0, 0.0f);
        }
    }
}
