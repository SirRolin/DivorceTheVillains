using UnityEngine;

public class POICollections : MonoBehaviour
{
    private readonly object locker = new();
    public Transform GetRandomPOI(Transform previousPOI){
        lock (locker){
            int childCount = transform.childCount;
            int randomIndex = Random.Range(0,childCount);
            Transform newPOI = transform.GetChild(randomIndex);
            DisablePOI(newPOI);
            if(previousPOI != null){
                EnablePOI(previousPOI);
            }
            return newPOI;
        }
    }

    private void DisablePOI(Transform POI)
    {
        POI.SetParent(null);
    }
    private void EnablePOI(Transform POI)
    {
        POI.SetParent(transform);
    }
}
