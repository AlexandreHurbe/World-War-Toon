using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1GarandBehaviour : WeaponBehaviour {


    //public M1GarandBehaviour(float fireRate, float weaponDamage, int magSize, int totalAmmo, float reloadTime, bool chamberedRound, int viewDist) : base(fireRate, weaponDamage, magSize, totalAmmo, reloadTime, chamberedRound, viewDist)
    //{
    //    Debug.Log("Constructor is called");
    //}

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
        
    }

    // Use this for initialization
    new void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        fireRate = WeaponStats.M1GarandStats.fireRate;
        weaponDamage = WeaponStats.M1GarandStats.weaponDamage;
        magSize = WeaponStats.M1GarandStats.magSize;
        totalAmmo = WeaponStats.M1GarandStats.totalAmmo;
        reloadTime = WeaponStats.M1GarandStats.reloadTime;
        chamberedRound = WeaponStats.M1GarandStats.chamberedRound;
        viewDist = WeaponStats.M1GarandStats.viewDist;
        setCurrentAmmoInMag();
        setCurrentTotalAmmo();
    }



    // Update is called once per frame
    void Update () {
		
	}

    
}
