using System;
using UnityEngine;

namespace Assets.HarashSuperVillains.Objects {
    public class Pickupable : MonoBehaviour, IPickupable
    {
        public String ID;
        public Vector3 pickupSize = new(0.1f, 0.1f, 0.1f);
        private Vector3 originalSize;

        private Transform originalTransform;
        public string getID()
        {
            return ID;
        }

        public void Interact(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        public void Pickup(Transform hand)
        {
            transform.SetParent(hand);
            transform.localPosition = new(0,0,0);
            transform.localScale = pickupSize;
        }

        public void Putdown(Vector3 pos, Vector3 norm)
        {
            transform.SetParent(originalTransform);
            transform.position = pos + Vector3.Scale(norm, new Vector3(originalSize.x / 2, originalSize.y / 2, originalSize.z / 2));
            transform.localScale = originalSize;
            if(TryGetComponent<Rigidbody>(out Rigidbody body)){
                body.WakeUp();
            } else {
                body = gameObject.AddComponent<Rigidbody>();
                body.mass = 1000000;
                Invoke(nameof(RemoveRigidBody), 5f);    
            }
        }

        void Start(){
            originalTransform = transform.parent;
            originalSize = transform.localScale;
        }

        void RemoveRigidBody(){
            if(TryGetComponent<Rigidbody>(out Rigidbody body)){
                Destroy(body);
            }
        }
    }
}
