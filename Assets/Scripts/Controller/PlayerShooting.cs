using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    //THIS WILL NEED TO BE MOVED TO ANOTHER PLAYER CONFIGURATION SCRIPT EVENTUALLY
    private int primaryWeaponID = 0;
    private int secondaryWeaponID = 2;

    //These are the weapon objects found in the hand of player
    [SerializeField]
    private GameObject[] weapons;
    //These are the weapon objects found in the back of player
    [SerializeField]
    private GameObject[] backWeapons;
    //These are the weapons found to the side of the player
    [SerializeField]
    private GameObject[] sideWeapons;

    [SerializeField]
    private GameObject RightHand;


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
    //The weapon object that can be found attached to the player
    private GameObject weapon;
    //The left hand position of the player on the gun
    private Transform weaponLeftHandPos;
    //This is the ID of the weapon player currently has equipped weapon id can be found weapon stats script
    private int currentWeaponID;
    //This is the ID of the weapon the player is going to equip
    private int nextWeaponID;
    //Checks if the current weapon is a pistol
    private bool currentWeaponIsPistol;
    //Whenever weight needs to be applied to the upper layer of the animation controller 
    private bool swappingWeapons;
    //
    private bool disableUpperControls; 

    public void Init()
    {
        
        

        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        isSprinting = playerMovement.getIsSprinting();
        playerCamera = GetComponent<PlayerCamera>();

        //weaponBehaviour = GetComponentInChildren<WeaponBehaviour>();
        //Debug.Log(weaponBehaviour.GetType());
        
        ReadyWeapons();
        currentLayerWeight = 0;
        
    }

    private void ReadyWeapons()
    {
        Debug.Log(primaryWeaponID);
        weapon = weapons[primaryWeaponID];
        currentWeaponID = primaryWeaponID;
        weapon.SetActive(true);
        Debug.Log(weapon);
        weaponBehaviour = weapon.GetComponent<WeaponBehaviour>();
        playerCamera.setViewDist(weaponBehaviour.getViewDist());
        Debug.Log(weaponBehaviour.getViewDist());
        weaponLeftHandPos = weaponBehaviour.getLeftHandPos();
        currentWeaponIsPistol = weaponBehaviour.returnIsPistol();


        GameObject secondaryWeapon = weapons[secondaryWeaponID];
        secondaryWeapon.SetActive(true);
        //Handle the gun visuals so they show on the back or side of player
        if (secondaryWeapon.GetComponent<WeaponBehaviour>().returnIsPistol())
        {
            sideWeapons[secondaryWeaponID].SetActive(true);
        }
        else
        {
            backWeapons[secondaryWeaponID].SetActive(true);
        }
        secondaryWeapon.SetActive(false);

    }

    private void SwapWeapon(int weaponID)
    {
        //play stowaway animation
        anim.SetTrigger("PutAway");
        nextWeaponID = weaponID;
        swappingWeapons = true;
        Debug.Log("Upper body animation @ SwapWeapon: " + swappingWeapons);
    }


    private void EventAddGunToBack()
    {
        weapon.SetActive(false);
        //set current weapon active on back or side
        if (currentWeaponIsPistol)
        {
            sideWeapons[currentWeaponID].SetActive(true);
        }
        else
        {
            backWeapons[currentWeaponID].SetActive(true);
        }

        //Initialises next weapon in player's hand
        weapon = weapons[nextWeaponID];
        weapon.SetActive(true);
        Debug.Log(weapon);
        currentWeaponID = nextWeaponID;
        weaponBehaviour = weapon.GetComponent<WeaponBehaviour>();
        currentWeaponIsPistol = weaponBehaviour.returnIsPistol();
        Debug.Log(currentWeaponIsPistol);
        anim.SetTrigger("Grab");
        weapon.SetActive(false);
        

        Debug.Log("Upper body animation @ EventGunToBack: " + swappingWeapons);
    }


    private void EventAddGunToHand()
    {
        if (currentWeaponIsPistol)
        {
            sideWeapons[currentWeaponID].SetActive(false);
        }
        else
        {
            backWeapons[currentWeaponID].SetActive(false);
        }


        weapon.SetActive(true);
        weaponBehaviour = weapon.GetComponent<WeaponBehaviour>();
        playerCamera.setViewDist(weaponBehaviour.getViewDist());
        currentWeaponIsPistol = weaponBehaviour.returnIsPistol();
        weaponLeftHandPos = weaponBehaviour.getLeftHandPos();


        Debug.Log("Upper body animation @ EventAddGunToHand: " + swappingWeapons);
        
    }

    

    private void EventDisableUpperControls()
    {
        disableUpperControls = true;
        Debug.Log("disableUpperControls @ EventDisableUpperControls: " + disableUpperControls);
    }
 
    private void EventEnableUpperControls()
    {
        disableUpperControls = false;
        Debug.Log("disableUpperControls @ EventEnableUpperControls: " + disableUpperControls);
       
    }

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (playerMovement.getDisableControls() || disableUpperControls)
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(PlayerInputCustomiser.PrimaryWeapon) && currentWeaponID != primaryWeaponID)
            {
                
                SwapWeapon(primaryWeaponID);
                Debug.Log("Swapping to primary the current weapon ID is: " + currentWeaponID);
                return;
            }
            if (Input.GetKeyDown(PlayerInputCustomiser.SecondaryWeapon) && currentWeaponID != secondaryWeaponID)
            {
                SwapWeapon(secondaryWeaponID);
                Debug.Log("Swapping to secondary the current weapon ID is: " + currentWeaponID);
                return;
            }

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

            if (Input.GetMouseButton(PlayerInputCustomiser.Shoot))
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
        anim.SetBool("isPistol", currentWeaponIsPistol);
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
        if (isAiming || isReloading || isMeleeing || swappingWeapons || currentWeaponIsPistol)
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
