using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject weapon;
    public Transform weaponLeftHandPos;

    private Animator anim;

    private bool isAiming;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1))
        {
            Debug.Log("Soldier aiming");
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
            //Want to rotate gun end to face mouse
        }
        else
        {
            return;
        }
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
    }
}
