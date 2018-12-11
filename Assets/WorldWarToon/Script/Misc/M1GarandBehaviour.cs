using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1GarandBehaviour : WeaponBehaviour {

    //// PROTECTED VARIABLES \\
    ////The rate in which the gun can fire, the higher the value the slower it is;
    //protected float fireRate;
    ////The amount of damage the weapon does
    //protected float weaponDamage;
    ////The amount of ammo the magazine carries
    //protected int magSize;
    ////The amount of total ammo the player can carry
    //protected int totalAmmo;
    ////The time it takes for user to reload
    //protected float reloadTime;
    ////Some guns leave a bullet in the chamber when it is not an empty reload
    //protected bool chamberedRound;
    ////Guns with scopes will allow players to view further and shoot further.
    //protected int viewDist;

    

    private void Awake()
    {
        aimLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        fireRate = 0.3f;
        weaponDamage = 60;
        magSize = 5;
        totalAmmo = 80;
        reloadTime = 3.2f;
        chamberedRound = false;
        viewDist = 5f;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
