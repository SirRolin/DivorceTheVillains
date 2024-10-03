using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.HarashSuperVillains.Objects{
  public class Interactable: MonoBehaviour
  {
    public List<String> interactables = new();
    public List<String> consumes = new();
    public Dictionary<String, Action> OnInteract = new();
    internal bool Interact(IPickupable objInHand)
    {
      if(interactables.Contains(objInHand.getID())){
        OnInteract[objInHand.getID()]?.Invoke();
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