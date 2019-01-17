using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSoldierMovement : MonoBehaviour {

    //Public Variables
    public Transform[] spawnPoints;
    public float speed = 2f;
    public float sprintSpeed = 4f;
    public float rotationSpeed;
    public float maxStamina;
    public float staminaRegen;
    public Slider staminaSlider;
    
    //Private variables
    private Vector3 movement;
    private bool isSprinting = false;
    private float sprintCost = 10f;
    private bool isRolling = false;
    private float rollCost = 20f;
    private bool isCrouching = false;
    private bool rollRecovered = true;
    private float stamina;

    //PlayerSoldierShoot may be used later to implement controls
    private PlayerSoldierShoot playerShootBehaviour;
    private Animator anim;
    Rigidbody playerRigidBody;
    CapsuleCollider capsuleCollider;
    
    int floorMask;
    float camRayLength = 100f;
    private Vector3 moveVelocity;
    private float directionAngle;
    private Vector3 playerToMouse;
    private float timer;
    private float rollDuration = 0.84f;






    private void Awake() {
        int spawnIndex = Random.Range(0, spawnPoints.Length - 1);
        this.transform.position = spawnPoints[spawnIndex].position;


        Cursor.visible = true;

        floorMask = LayerMask.GetMask("Floor");

        anim = GetComponent<Animator>();
        playerShootBehaviour = GetComponent<PlayerSoldierShoot>();
        playerRigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        

        maxStamina = SoldierUpgrades.maxStamina;
        stamina = maxStamina;
        
        staminaSlider.maxValue = stamina;
        staminaSlider.value = maxStamina;

        staminaRegen = SoldierUpgrades.staminaRegen;
        sprintCost = SoldierUpgrades.sprintCost;
        rollCost = SoldierUpgrades.rollCost;


    }


    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        timer += Time.deltaTime;

        if (isRolling) {
            
            Roll();
        }


        if (h != 0 || v != 0) {
            isCrouching = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && stamina >= rollCost && isRolling == false)
        {
            stamina -= rollCost;
            
            StartCoroutine(canRoll());
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && (isCrouching == false)) {
            Debug.Log("Crouched");
            isCrouching = true;
            //capsuleCollider.height = 1;
            //capsuleCollider.center = new Vector3(0, 0.5f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && (isCrouching == true))
        {
            //capsuleCollider.height = 2;
            //capsuleCollider.center = new Vector3(0, 1f, 0);
            Debug.Log("Uncrouched");
            isCrouching = false;
        }

        
        

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0.1){
            stamina -= sprintCost * Time.deltaTime;
            isSprinting = true;
        }
        else {
            isSprinting = false;
        }

        
        Turning();
        Move(h, v);
        Animating(h, v);

        staminaSlider.value = stamina;
        if (isRolling == false && isSprinting == false) {
            if (stamina <= maxStamina) {
                stamina += Time.deltaTime * staminaRegen;
            }
        }

    }


    private void Move(float h, float v) {
        
        if (isRolling) {
            movement.Set(0, 0, 0);
            return;
        }
        if (isCrouching) { 
            movement.Set(0, 0, 0);
            return;
        }
        else if (isSprinting) {
            //Curent movement system self space
            movement = ((this.transform.right.normalized * h) + (this.transform.forward.normalized * v)) * sprintSpeed * Time.deltaTime;
        }
        else {
            //Current movement system self space
            movement = ((this.transform.right.normalized * h) + (this.transform.forward.normalized * v)) * speed * Time.deltaTime;
        }
        movement.Set(0, this.transform.position.y, 0);
        playerRigidBody.MovePosition(transform.position + movement);


    }

  
    


    IEnumerator canRoll() {
        isRolling = true;
        rollRecovered = false;
        anim.SetBool("isRolling", isRolling);
        yield return new WaitForSeconds(rollDuration);
        isRolling = false;
        anim.SetBool("isRolling", isRolling);
    }

    private void Roll() {
        movement = transform.forward * sprintSpeed;
        movement = movement.normalized * (sprintSpeed*2f) * Time.deltaTime;
        playerRigidBody.MovePosition(transform.position + movement);
    }

    private void Turning() {
        if (isRolling)
        {
            return;
        }
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
            playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }
    }

    private void Animating(float h, float v) {

        //Vector3 vertical = ((this.transform.right.normalized * h) * speed * Time.deltaTime;

        anim.SetBool("isCrouched", isCrouching);
        anim.SetBool("isSprinting", false);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("Horizontal", h);
        

        if (isSprinting) {
            anim.SetBool("isSprinting", true);
            anim.SetFloat("Vertical", v * sprintSpeed);
            anim.SetFloat("Horizontal", h * sprintSpeed);
        }
        
        
    }

    public void SetCrouching(bool crouch) {
        this.isCrouching = crouch;
    }

    

}
