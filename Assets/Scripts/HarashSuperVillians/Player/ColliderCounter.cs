using Unity.VisualScripting;
using UnityEngine;

public class ColliderCounter : MonoBehaviour
{
    [SerializeField]
    private uint count = 0;
    private void OnTriggerEnter(Collider c){
        count++;
    }

    private void OnTriggerExit(Collider c){
        count--;
    }
    public bool HasContact(){
        return count > 0;
    }
}
