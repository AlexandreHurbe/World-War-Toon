﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // PUBLIC VARIABLES \\
    public float speed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float crouchSprintSpeed;
    public float rotationSpeed = 30f;

    //PRIVATE COMPONENTS \\
    private PlayerCamera playerCamera;
    private PlayerShooting playerShooting;
    private Vector3 camForward;
    private Vector3 camRight;
    private Rigidbody playerRigidBody;
    private Animator anim;

    // PRIVATE VARIABLES \\
    private Vector3 currentMoveSpeed;
    private bool isAiming;
    private bool isMoving;
    private bool isSprinting;
    private bool canCrouch;
    private bool isCrouching;

    private float h;
    private float v;

    private void Awake() {
        playerRigidBody = GetComponent<Rigidbody>();
        playerCamera = GetComponent<PlayerCamera>();
        playerShooting = GetComponent<PlayerShooting>();
        anim = GetComponent<Animator>();
        
        isSprinting = false;
        canCrouch = true;
    }

    // Use this for initialization
    void Start () {
        camForward = playerCamera.getPlayerCamera().transform.forward;
        camRight = playerCamera.getPlayerCamera().transform.right;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        camForward = playerCamera.getPlayerCamera().transform.forward;
        camRight = playerCamera.getPlayerCamera().transform.right;

        isAiming = playerShooting.getIsAiming();

        if (Input.GetKey(PlayerInputCustomiser.Sprint) && (h != 0 || v != 0)) {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (Input.GetKeyDown(PlayerInputCustomiser.Crouch) && canCrouch)
        {
            //This is one because the input can be detected several times in one frame thus cancelling the crouch.
            StartCoroutine(canCrouchRoutine());
            
            if (isCrouching)
            {
                isCrouching = false;
            }
            else
            {
                isCrouching = true;
            }
        }


        Move(h, v);
        
    }

    private void LateUpdate()
    {
        Animate(h, v);
    }

    private void Animate(float h, float v) {

        anim.SetBool("isCrouching", isCrouching);
        

        if (isAiming)
        {
            //Animation stuff you can ignore

            Vector3 movementSelf = (this.transform.right.normalized * h) + (this.transform.forward.normalized * v);
            //Debug.Log(new Vector3(h, 0, v));
            anim.SetFloat("Horizontal", movementSelf.x);
            anim.SetFloat("Vertical", movementSelf.z);
        }
        else
        {
            anim.SetFloat("movementSpeed", currentMoveSpeed.magnitude);
        }
        

    }

    private void Move(float h, float v) {
        Vector3 movement = new Vector3 (0, 0, 0);


        Transform playerCameraTransform = playerCamera.getPlayerCamera().transform;

        //When not aiming, player should be always facing the direction they are moving
        if (!isAiming)
        {
            //Forward Right
            if (v > 0 && h > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 45, Time.deltaTime *20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;
                

            }
            //Forward Left
            else if (v > 0 && h < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y - 45, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }
            
            //Backward right
            else if (v < 0 && h > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 135, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;
                
            }

            //Backward left
            else if (v < 0 && h < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y - 135, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }

            //Forward
            else if (v > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;


            }

            //Backward
            else if (v < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 180, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;


                
            }

            //Right
            else if (h > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 90, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }

            //Left
            else if (h < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y -90, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }
        }
  
        camForward.y = 0;
        camRight.y = 0;

        //Checks if the user is sprinting while crouched
        if (isCrouching)
        {
            if (isAiming)
            {
                movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * (crouchSpeed * 0.6f) * Time.deltaTime;
            }
            else
            {
                //if sprinting while crouched
                if (isSprinting)
                {

                    movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * crouchSprintSpeed * Time.deltaTime;
                }
                else
                {
                    movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * crouchSpeed * Time.deltaTime;
                }
            }

        }
        else
        {
            if (isAiming)
            {
                movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * (speed*0.6f) * Time.deltaTime;
            }
            else
            {
                //if sprinting while standing   
                if (isSprinting)
                {

                    movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * sprintSpeed * Time.deltaTime;
                }
                else
                {
                    movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * speed * Time.deltaTime;
                }
            }
            
        }
        

        //Debug.Log(movement.magnitude);
        currentMoveSpeed = Vector3.Lerp(currentMoveSpeed, movement, Time.deltaTime * (20f));

        playerRigidBody.MovePosition(transform.position + currentMoveSpeed);
        
    }

    IEnumerator canCrouchRoutine()
    {
        canCrouch = false;
        yield return new WaitForSeconds(0.1f);
        canCrouch = true;
    }

    public bool getIsSprinting()
    {
        return isSprinting;
    }
}
