using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.HarashSuperVillains.Player {
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        [Range(-1f,1f)]
        private float camSpeedX = 0.1f;
        [SerializeField]
        [Range(-1f,1f)]
        private float camSpeedY = 0.1f;
        [SerializeField]
        private Transform camAnchor;
        [SerializeField]
        private Transform orientation;


        [Header("Speed")]
        [SerializeField]
        [Range(0f,100f)]
        private float maxSpeed = 1f;
        private Vector2 currentDir = new(0,0);

        [Header("Ground Check")]
        [SerializeField] float playerHeight;
        [SerializeField] LayerMask whatIsGround;
        [SerializeField] ColliderCounter GroundCollider;
        [SerializeField] bool grounded;
        [SerializeField]
        private float groundedFor = 0f;

        [SerializeField]
        [Range(0f,2f)]
        private float groundedDelay = 0.25f;
        
        [SerializeField]
        [Range(0f,100f)]
        private float jumpHeight = 10f;
        [SerializeField]
        [Range(0f,2f)]
        private float jumpCayotte = 0.25f;
        private bool hasCoyotteJumped = false;
        [SerializeField]
        [Range(0f,10f)]
        private float jumpCooldown = 0.1f;
        private bool readyToJump = true;
        [SerializeField]
        private bool wantsToJump = false;


        public bool ragdolled = false;
        private Rigidbody rb;
        
        void Awake(){
            rb = GetComponent<Rigidbody>();
            if(rb == null) enabled = false;
            if(orientation == null){
                enabled = false;
                Debug.Log("orientation Missing on " + name);
            }
            if(GroundCollider!=null){
                GroundCollider.gameObject.GetComponent<Collider>().includeLayers = whatIsGround;
            }
        }

        void OnDeath(){
            ragdolled = true;
            rb.freezeRotation = false;
        }

        void Update(){
            if(!ragdolled){
                // ground check
                if(GroundCollider == null){
                    grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight/2 + 0.1f, whatIsGround);
                } else {
                    grounded = GroundCollider.HasContact();
                }

                //Movement Direction
                Vector3 moveDir = orientation.forward * currentDir.y + orientation.right * currentDir.x;

                // timer for how long we've been grounded, negative being not grounded (for cayotte time)
                if(grounded){
                    if(groundedFor < 0) groundedFor = 0; else groundedFor += Time.deltaTime;
                    
                    //// If we were falling, it means we can jump again. - check to counteract a doublejump bug.
                    if(rb.linearVelocity.y <= 0) hasCoyotteJumped = false;
                } else {
                    if(groundedFor > 0) groundedFor = 0; else groundedFor -= Time.deltaTime;
                }

                
                if(groundedFor > groundedDelay){
                    // try to move
                    rb.AddForce(1000f * maxSpeed * Time.deltaTime * moveDir.normalized, ForceMode.Force);

                    // Limit max speed only if grounded for some time.
                    SpeedControl(moveDir.magnitude);
                }

                // Jumping
                if(wantsToJump && readyToJump && CanCayotte()){
                    readyToJump = false;
                    hasCoyotteJumped = true;
                    Jump();
                    Invoke(nameof(ResetJump), jumpCooldown);
                }
            }
        }

        void SpeedControl(float dial){
            // z becomes y
            Vector2 flatVel = new(rb.linearVelocity.x, rb.linearVelocity.z);
            if(flatVel.magnitude > maxSpeed * dial){
                Vector2 limitedVel = flatVel.normalized * maxSpeed * dial;
                // y becomes z again.
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.y);
            }
        }

        void OnJump(InputValue input){
            //if (!photonView.IsMine) return;
            wantsToJump = input.isPressed;
        }

        private void ResetJump(){
            readyToJump = true;
        }

        private void Jump(){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpHeight * 250f, ForceMode.Force);
        }

        bool CanCayotte(){
            return !hasCoyotteJumped && groundedFor > -jumpCayotte;
        }

        void OnLook(InputValue rot){
            //if (!photonView.IsMine) return;
            // camAnchor check due to unity bug telling us it's not assigned, while it's assigned
            if(camAnchor != null){
                Vector2 inputRot = rot.Get<Vector2>();
                Vector3 camRot = camAnchor.rotation.eulerAngles;
                Vector3 playerRot = orientation.rotation.eulerAngles;
                camRot.x += inputRot.y * -camSpeedY;

                // Fixing Camera Flipping on it's head
                camRot.x = (camRot.x > 90 && camRot.x < 270) ? (camRot.x < 180 ? 90 : 270) : camRot.x; 
                
                playerRot.y += inputRot.x * camSpeedX;
                camAnchor.rotation = Quaternion.Euler(camRot);
                orientation.rotation = Quaternion.Euler(playerRot);
            }
        }

        void OnMove(InputValue dir){
            //if (!photonView.IsMine) return;
            if(!ragdolled){
                currentDir = dir.Get<Vector2>();
            } else {
                currentDir.x = 0;
                currentDir.y = 0;
            }
        }
    }
}