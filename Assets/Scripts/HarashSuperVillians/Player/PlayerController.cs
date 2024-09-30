using System;
using System.Numerics;
using Photon.Pun;
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

        [SerializeField]
        [Range(0f,1f)]
        private float airSpeed;
        
        [SerializeField]
        [Range(0f,1000f)]
        private float groundDrag;

        [Header("Ground Check")]
        [SerializeField] float playerHeight;
        [SerializeField] LayerMask whatIsGround;
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
        private float jumpCooldown = 1f;
        private bool readyToJump = true;
        private bool wantsToJump = false;


        public bool ragdolled = false;
        private Rigidbody rb;
        
        void Awake(){
            this.rb = GetComponent<Rigidbody>();
            if(rb == null) this.enabled = false;
            if(orientation == null){
                this.enabled = false;
                Debug.Log("orientation Missing on " + this.name);
            }
        }

        void OnDeath(){
            ragdolled = true;
            rb.freezeRotation = false;
        }

        void Update(){
            if(!ragdolled){
                // ground check
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight/2 + 0.2f, whatIsGround);    

                // drag to slow down while on ground
                rb.linearDamping = grounded ? groundDrag : 0;

                // try to move
                Vector3 moveDir = orientation.forward * currentDir.y + orientation.right * currentDir.x;
                rb.AddForce((grounded ? 1f : airSpeed) * 25f * maxSpeed * moveDir.normalized, ForceMode.Force);

                // timer for how long we've been grounded, negative being not grounded (for cayotte time)
                if(grounded){
                    if(groundedFor < 0) groundedFor = 0;
                    groundedFor += Time.deltaTime;
                    if(rb.linearVelocity.y < 0) hasCoyotteJumped = false;
                } else {
                    if(groundedFor > 0) groundedFor = 0;
                    groundedFor -= Time.deltaTime;
                }

                
                // Limit max speed only if grounded for some time.
                if(groundedFor > groundedDelay){
                    SpeedControl();
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

        void SpeedControl(){
            // z becomes y
            Vector2 flatVel = new(rb.linearVelocity.x, rb.linearVelocity.z);
            if(flatVel.magnitude > maxSpeed){
                Vector2 limitedVel = flatVel.normalized * maxSpeed;
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
            rb.linearVelocity.Set(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(100f * jumpHeight * Vector3.up);
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
        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new NotImplementedException();
            if(stream.IsWriting){
                stream.SendNext(currentDir);
                stream.SendNext(wantsToJump);
            } else {
                currentDir = (Vector2) stream.ReceiveNext();
                wantsToJump = (bool) stream.ReceiveNext();
            }
        }*/
    }
}