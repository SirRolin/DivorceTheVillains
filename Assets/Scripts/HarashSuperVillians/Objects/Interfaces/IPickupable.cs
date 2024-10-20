using System;
using UnityEngine;

public interface IPickupable
{
  GameObject getGameObject();
  void Pickup(Transform hand);
  void Putdown(Vector3 pos, Vector3 norm);
  void Interact(GameObject obj);
  String getID();
}
