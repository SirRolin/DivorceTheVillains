using UnityEngine;

public class POICollections : MonoBehaviour
{
    public Transform disabledGroup;
    private readonly object locker = new();
    public Transform GetRandomPOI(){
        lock (locker){
            int childCount = transform.childCount;
            int randomIndex = Random.Range(0,childCount);
            return transform.GetChild(randomIndex);
        }
    }

    public void DisablePOI(Transform POI)
    {
        lock (locker){
            POI.SetParent(null);
        }
    }
    public void EnablePOI(Transform POI)
    {
        lock (locker){
            POI.SetParent(transform);
        }
    }
}
