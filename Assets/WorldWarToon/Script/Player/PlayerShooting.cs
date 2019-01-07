using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    // PUBLIC VARIABLES \\
    //The weapon object that can be found attached to the player
    public GameObject weapon;
    //The left hand position of the player on the gun
    public Transform weaponLeftHandPos;
    public Transform weaponRightHandPos;
    //The spine located on the player's character
    public Transform spine;

    // PRIVATE COMPONENTS \\
    //The Animation controller
    private Animator anim;
    //The player movement script
    private PlayerMovement playerMovement;
    //The player camera script
    private PlayerCamera playerCamera;
    //The weapon behaviour script that is attached to the weapon game object
    private WeaponBehaviour weaponBehaviour;

    // PRIVATE VARIABLES \\
    //Whether or not the player is aiming
    private bool isAiming;
    //Whether or not the player is sprinting
    private bool isSprinting;
    //Whether or not player is reloading
    private bool isReloading;
    //Whether or not player is meleeing
    private bool isMeleeing;
    //Animation stuff you can ignore
    private float currentLayerWeight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        isSprinting = playerMovement.getIsSprinting();
        playerCamera = GetComponent<PlayerCamera>();

        weaponBehaviour = GetComponentInChildren<WeaponBehaviour>();
        //Debug.Log(weaponBehaviour.GetType());

        currentLayerWeight = 0;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (playerMovement.getDisableControls())
        {
            return;
        }
        else
        {
            //Checks if the player is currently sprinting which is determined in player movement
            isSprinting = playerMovement.getIsSprinting();
            isReloading = weaponBehaviour.getReloadState();


            if (Input.GetKeyDown(PlayerInputCustomiser.Melee))
            {
                Melee();
            }

            if (Input.GetKeyDown(PlayerInputCustomiser.Reload))
            {
                Reload();
            }

            //Checks if the player is holding the right mouse button and they are not currently sprinting
            if (Input.GetMouseButton(PlayerInputCustomiser.Aim) && !isSprinting)
            {

                isAiming = true;

            }
            else
            {
                isAiming = false;
            }

            //isAiming = true;
            Aiming();
            Animate();
        }
        
    }

    private void Aiming()
    {
        if (isAiming)
        {
            AlignWeapon(playerCamera.getMouseWorldPosition());

            //weaponBehaviour.fireWeapon();

            weaponBehaviour.drawAimLine(playerCamera.getMouseWorldPosition());
            // Rotates player to face mouse when aiming
            RotateTowards();

            if (Input.GetMouseButtonDown(PlayerInputCustomiser.Shoot))
            {
                //Debug.Log("Left mouse click registered");
                weaponBehaviour.fireWeapon();
            }


        }
        else
        {
            weaponBehaviour.setAimLine(false);
            return;
        }
    }

    private void Reload()
    {
        Debug.Log("Player has press reload");
        weaponBehaviour.Reload();
        //play reload animation
    }

    private void AlignWeapon(Vector3 mouseWorldPosition)
    {
        weaponBehaviour.alignGunEnd(mouseWorldPosition);

    }

    private void RotateTowards()
    {

        //Rotates player towards mouse cursor when aiming
        Vector3 modifiedMousePosition = playerCamera.getMouseWorldPosition();
        modifiedMousePosition.y = 1;
        Vector3 direction = (modifiedMousePosition - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);
    }

    private void Melee()
    {
        isMeleeing = true;
        anim.SetTrigger("isMeleeing");
    }

    private void EventFinishMelee()
    {
        isMeleeing = false;
    }
    

    private void Animate()
    {
        anim.SetBool("isAiming", isAiming);
        //Debug.Log(weaponBehaviour.getReloadState());
        anim.SetBool("isReloading", isReloading);
    }

    //This is all animation stuff, you can ignore it you're not directly working with it.
    private void OnAnimatorIK()
    {
        if (!isReloading && !isMeleeing)
        {
            anim.SetIKPosition(AvatarIKGoal.LeftHand, weaponLeftHandPos.transform.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, weaponLeftHandPos.transform.rotation);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        }

        //Sets the upperbody mask layer value to 1 to show animation
        if (isAiming || isReloading || isMeleeing)
        {
            currentLayerWeight = 1;
            anim.SetLayerWeight(1, currentLayerWeight);
        }
        else
        {
            anim.SetLookAtPosition(playerCamera.getMouseWorldPosition());
            currentLayerWeight = Mathf.Lerp(currentLayerWeight, 0, Time.deltaTime * 5f);
            anim.SetLayerWeight(1, currentLayerWeight);
        }
    }

    public bool getIsAiming()
    {
        return isAiming;
    }
}
