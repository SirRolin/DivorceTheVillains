using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.HarashSuperVillains.Objects{
  public class Interactable: MonoBehaviour
  {
    public List<String> interactables = new();
    public List<String> consumes = new();
    public bool wantTransparency = false;
    public float opacity = 0.3f;
    private bool isConsumed = true;
    public Dictionary<String, Action> OnInteract = new();

    void Start (){
      CheckAppearance();
      /*
      if(wantOpacity){
        foreach (Transform child in gameObject.transform)
        {
          child.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, opacity);
        } 
      }*/
    }

    /// <summary>
    /// Changed from Update due to that being a misuse of resources.
    /// It shouldn't do these checks and change the colour every frame.
    ///  - Rolin
    /// </summary>
    internal void CheckAppearance(){
      if(wantTransparency){
        foreach (Transform child in gameObject.transform)
        {
          child.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, opacity);
        }
      } else if (wantTransparency == false && isConsumed ){
        foreach (Transform child in gameObject.transform)
        {
          if(child.gameObject.TryGetComponent<MeshRenderer>(out var MR)){
            Color randomColor = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            MR.material.color = randomColor;
          }
        }
        isConsumed = false;
      }
    }
  

    internal bool Interact(IPickupable objInHand)
    {
      if((objInHand == null && interactables.Contains("Empty"))){
        OnInteract["Empty"]?.Invoke();
        CheckAppearance();
        return true;
      } else if(objInHand != null && interactables.Contains(objInHand.getID())){
        OnInteract[objInHand.getID()]?.Invoke();
        wantTransparency = false;
        CheckAppearance();
        return true;
      }
      return false;
    }
    internal bool DoesConsume(IPickupable objInHand)
    {
      if(consumes.Contains(objInHand.getID())){
        return true;
      }
      return false;
    }
  }
}