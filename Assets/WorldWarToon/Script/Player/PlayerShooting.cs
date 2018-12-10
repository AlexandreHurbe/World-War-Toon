using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject weapon;
    public Transform weaponLeftHandPos;
    public Transform spine;

    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private WeaponBehaviour weaponBehaviour;

    private bool isAiming;
    private bool isSprinting;

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
        isSprinting = playerMovement.getIsSprinting();

        if (Input.GetMouseButton(1) && !isSprinting)
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

            if (Input.GetMouseButtonDown(0))
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
        

        Vector3 direction = (playerCamera.getMouseWorldPosition() - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);

        
        //// Set our Look Weights
        //anim.SetLookAtWeight(1, 1, 1, 1, 1);
        //// Set the Look At Position which is a point along the camera direction     
        //anim.SetLookAtPosition(direction);
    }

    private void Animate()
    {
        anim.SetBool("isAiming", isAiming);
    }

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

            currentLayerWeight = Mathf.Lerp(currentLayerWeight, 0, Time.deltaTime * 5f);
            //anim.SetLayerWeight(1, currentLayerWeight);
        }
    }

    public bool getIsAiming()
    {
        return isAiming;
    }
}
