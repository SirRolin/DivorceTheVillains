using System;
using UnityEngine;
using UnityEngine.UI;

public class Anger : MonoBehaviour
{
    [SerializeField]
    private float currentAnger = 0f;
    [SerializeField]
    private float maxAnger = 0f;
    [SerializeField]
    private bool Temporary = true;

    public event Action<Image, float, float> OnUpdate;
    public event Action OnCheckForWin;
    public Image barFill;

    public void ApplyAnger(float amount){
        currentAnger += amount;
        if(Temporary && (currentAnger > maxAnger)){
            maxAnger = currentAnger;
        }
        OnUpdate?.Invoke(barFill, currentAnger, maxAnger);
        OnCheckForWin?.Invoke();
    }
    public float GetAnger() 
    {
        return currentAnger;
    }

    public float GetMaxAnger() 
    { 
        return maxAnger;
    }

    

    public void Awake(){
        Temporary = maxAnger == 0;
        /*// Test
        if(gameObject.TryGetComponent<EnemyDetector>(out EnemyDetector ed)){
            ed.OnDetect += ((gameObject) => ApplyAnger(2));
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // if angry lower anger by time since last frame
        if(Temporary && currentAnger > 0){
            // if less angry than last frame, set to 0
            // my optimised way without losing readability
            currentAnger -= Math.Min(currentAnger, Time.deltaTime);
            OnUpdate?.Invoke(barFill, currentAnger, maxAnger);
        } else if(Temporary){
            maxAnger = 0;
        }
    }
}
