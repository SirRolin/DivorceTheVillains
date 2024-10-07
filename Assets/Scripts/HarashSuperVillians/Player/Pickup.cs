using Assets.HarashSuperVillains.Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using Interactable = Assets.Scripts.HarashSuperVillains.Objects.Interactable;

namespace Assets.Scripts.HarashSuperVillains.Player{
    public class Pickup : MonoBehaviour
    {
        public Camera cam;
        [Range(0f, 100f)]
        public float reach = 5f;
        public Transform hand;
        IPickupable objInHand = null;
        void OnInteract(InputValue input){
            Ray ray = new(cam.transform.position, cam.transform.forward);
            if(objInHand == null){
                if(Physics.Raycast(ray, out RaycastHit hit, reach)){
                    if(hit.collider.gameObject.TryGetComponent<IPickupable>(out IPickupable pickupObj)){
                        pickupObj.Pickup(hand);
                        objInHand = pickupObj;
                    } else if(hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable)){
                        interactable.Interact(null);
                    }
                }
            } else {
                if(Physics.Raycast(ray, out RaycastHit hit, reach)){
                    if(hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable)){
                        if(interactable.Interact(objInHand)) {
                            if(interactable.DoesConsume(objInHand)){
                                Destroy(((Pickupable) objInHand).gameObject);
                                objInHand = null;
                            }
                        }
                    } else {
                        objInHand.Putdown(hit.point, hit.normal);
                        objInHand = null;
                    }
                }
            }
        }

        void OnEnable(){
            if(hand == null){
                enabled = false;
            }
        }
    }
}