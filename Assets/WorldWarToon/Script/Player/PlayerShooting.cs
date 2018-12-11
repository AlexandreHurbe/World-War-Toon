using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    // PUBLIC VARIABLES \\
    //The weapon object that can be found attached to the player
    public GameObject weapon;
    //The left hand position of the player on the gun
    public Transform weaponLeftHandPos;
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
    //Animation stuff you can ignore
    private float currentLayerWeight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        isSprinting = playerMovement.getIsSprinting();
        playerCamera = GetComponent<PlayerCamera>();

        weaponBehaviour = GetComponentInChildren<WeaponBehaviour>();
        
        currentLayerWeight = 0;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Checks if the player is currently sprinting which is determined in player movement
        isSprinting = playerMovement.getIsSprinting();

        if (Input.GetKeyDown(PlayerInputCustomiser.Reload))
        {
            weaponBehaviour.Reload();
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

        
        Aiming();
        Animate();
	}

    private void Aiming()
    {
        if (isAiming)
        {
            
            weaponBehaviour.drawAimLine(playerCamera.getMouseWorldPosition());
            //Want to rotate gun end to face mouse
            //RotateTowards();

            if (Input.GetMouseButtonDown(PlayerInputCustomiser.Shoot))
            {
                weaponBehaviour.fireWeapon();
            }
            

        }
        else
        {
            weaponBehaviour.setAimLine(false);
            return;
        }
    }

    private void RotateTowards()
    {
        
        //Rotates player towards mouse cursor when aiming
        Vector3 direction = (playerCamera.getMouseWorldPosition() - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);

        //This is still being tested for more accurate aiming 
        //// Set our Look Weights
        //anim.SetLookAtWeight(1, 1, 1, 1, 1);
        //// Set the Look At Position which is a point along the camera direction     
        //anim.SetLookAtPosition(direction);
    }

    private void Animate()
    {
        anim.SetBool("isAiming", isAiming);
    }

    //This is all animation stuff, you can ignore it you're not directly working with it.
    private void OnAnimatorIK()
    {


        anim.SetIKPosition(AvatarIKGoal.LeftHand, weaponLeftHandPos.transform.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, weaponLeftHandPos.transform.rotation);
        
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);


        if (isAiming)
        {
            RotateTowards();
            //anim.SetLayerWeight(1, 1);
            currentLayerWeight = 1;
            
        }
        else
        {
            anim.SetLookAtPosition(playerCamera.getMouseWorldPosition());
            currentLayerWeight = Mathf.Lerp(currentLayerWeight, 0, Time.deltaTime * 5f);
            //anim.SetLayerWeight(1, currentLayerWeight);
        }
    }

    public bool getIsAiming()
    {
        return isAiming;
    }
}
