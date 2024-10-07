using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.HarashSuperVillains.Objects{
  public class Interactable: MonoBehaviour
  {
    public List<String> interactables = new();
    public List<String> consumes = new();
    public bool wantOpacity = true;
    public float opacity = 0.3f;
    private bool isConsumed = true;
    public Dictionary<String, Action> OnInteract = new();

    void Start (){
      if(wantOpacity){
        foreach (Transform child in gameObject.transform)
        {
          child.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, opacity);
      } 
    }
    }

    void Update(){
      if(wantOpacity){
        foreach (Transform child in gameObject.transform)
        {
          child.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, opacity);
        }
      } else if (wantOpacity == false && isConsumed ){
        foreach (Transform child in gameObject.transform)
        {
          Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
          child.gameObject.GetComponent<MeshRenderer>().material.color = randomColor;
        }
        isConsumed = false;
      }
    }
  

    internal bool Interact(IPickupable objInHand)
    {
      if(interactables.Contains(objInHand.getID())){
        OnInteract[objInHand.getID()]?.Invoke();
        wantOpacity = false;
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