using UnityEngine;

public class POICollections : MonoBehaviour
{

    public Transform GetRandomPOI(){
        int childCount = transform.childCount;
        int randomIndex = Random.Range(0,childCount);
        return transform.GetChild(randomIndex);
    }
}
