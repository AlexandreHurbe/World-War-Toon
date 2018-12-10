using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float crouchSprintSpeed;
    public float rotationSpeed = 30f;


    private PlayerCamera playerCamera;
    private PlayerShooting playerShooting;
    private Vector3 camForward;
    private Vector3 camRight;
    private Rigidbody playerRigidBody;
    private Animator anim;

    private Vector3 currentMoveSpeed;
    private bool isAiming;
    private bool isMoving;
    private bool isSprinting;
    private bool canCrouch;
    private bool isCrouching;

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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        camForward = playerCamera.getPlayerCamera().transform.forward;
        camRight = playerCamera.getPlayerCamera().transform.right;

        isAiming = playerShooting.getIsAiming();

        if (Input.GetKey(KeyCode.LeftShift) && (h != 0 || v != 0)) {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canCrouch)
        {
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
        Animate(h, v);
    }

    private void Animate(float h, float v) {

        anim.SetBool("isCrouching", isCrouching);
        

        if (isAiming)
        {
            Vector3 movementSelf = (this.transform.right.normalized * h) + (this.transform.forward.normalized * v);
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
        if (!isAiming)
        {
            if (v > 0 && h > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 45, Time.deltaTime *20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;
                

            }
            else if (v > 0 && h < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 315, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }
            else if (v < 0 && h > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 135, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;
                
            }
            else if (v < 0 && h < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 225, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }
            else if (v > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;


            }
            else if (v < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 180, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;


                
            }
            else if (h > 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 90, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }
            else if (h < 0)
            {
                float newY = Mathf.Lerp(transform.localEulerAngles.y, playerCameraTransform.localEulerAngles.y + 270, Time.deltaTime * 20f);
                Vector3 newAngle = new Vector3(transform.localEulerAngles.x, newY, transform.localEulerAngles.z);
                transform.localEulerAngles = newAngle;

                
            }
        }
  
        camForward.y = 0;
        camRight.y = 0;

        if (isCrouching)
        {
            if (isSprinting)
            {
                
                movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * crouchSprintSpeed * Time.deltaTime;
            }
            else
            {
                movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * crouchSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (isSprinting)
            {
                
                movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * sprintSpeed * Time.deltaTime;
            }
            else
            {
                movement = ((camRight.normalized * h) + (camForward.normalized * v)).normalized * speed * Time.deltaTime;
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
