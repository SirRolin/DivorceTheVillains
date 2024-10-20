using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using rng = UnityEngine.Random;

namespace Assets.Scripts.HarashSuperVillains.Objects {
  public class Interactable: MonoBehaviour
  {
    public bool randomiseColors = false;
    public float inactiveOpacity = 0.3f;
    public float activeOpacity = 1f;
    public Dictionary<String, Func<bool>> registeredInteractions = new();

    void Start (){
      CheckAppearance(false);
      /*
      if(wantOpacity){
        foreach (Transform child in gameObject.transform)
        {
          child.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, opacity);
        } 
      }*/
    }

    /// <summary>
    ///  Changed from Update due to that being a misuse of resources.
    ///  It shouldn't do these checks and change the colour every frame.
    ///  recoded as this applied to ALL traps making or water sink random colored...
    ///  Also note, wrong place for this... if a interactable object has 2 types of traps depending on the items given, this can't differenciate between them.
    /// </summary>
    /// TODO: Move this to TrapGeneric.cs in a manner that makes sense.
    internal void CheckAppearance(bool _isActive){
      AssignColor(transform, _isActive);
      foreach (Transform child in gameObject.transform)
      {
        AssignColor(child, _isActive);
      }
    }
    /// <summary>
    /// If the game object of t has a Renderer, reevaluate it's color and opacity.
    /// </summary>
    /// <param name="t">Transform of game object</param>
    /// <param name="_isActive">Is the trap being activated or not.</param>
    internal void AssignColor(Transform t, bool _isActive){
      if(t.gameObject.TryGetComponent<MeshRenderer>(out var MR)){
        Color theColor = getColor(t, _isActive);
        MR.material.color = theColor;
      }
    }
    /// <param name="t">Transform of game object to inherit from</param>
    /// <param name="_isActive">Is the trap being activated or not.</param>
    /// <returns>Color depending on properties, Can be random or inherited from t. if neither it returns Magenta as an error Color.</returns>
    internal Color getColor(Transform t, bool _isActive){
        // we want a random colour when it's active and randomize is true
        if(randomiseColors && _isActive){
          return new Color(rng.value, rng.value, rng.value, _isActive ? activeOpacity : inactiveOpacity);
        }
        // otherwise if we have a color give that back
        if(t.gameObject.TryGetComponent<MeshRenderer>(out var MR)){
          Color c = MR.material.color;
          c.a = _isActive ? activeOpacity : inactiveOpacity;
          return c;
        }
        // if not give a fallback color as it can't be null.
        return Color.magenta;
    }

    internal bool Interact(IPickupable objInHand)
    {
      bool ShouldDelete = false;
      if(objInHand == null && registeredInteractions.ContainsKey("")){
        ShouldDelete = registeredInteractions[""]();
        CheckAppearance(true);
        registeredInteractions.Remove("");
      } else if(objInHand != null && registeredInteractions.ContainsKey(objInHand.getID())){
        ShouldDelete = registeredInteractions[objInHand.getID()]();
        registeredInteractions.Remove(objInHand.getID());
        CheckAppearance(true);
      }
      return ShouldDelete;
    }

    public void AddInteraction(string v, Func<bool> value)
    {
      if(registeredInteractions.ContainsKey(v)){
        Debug.LogError("2 Traps on the same object uses the same item");
        return;
      }
      registeredInteractions.Add(v, value);
    }
  }
}