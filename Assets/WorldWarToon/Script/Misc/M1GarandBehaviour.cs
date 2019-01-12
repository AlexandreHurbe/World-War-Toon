using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1GarandBehaviour : WeaponBehaviour {



    private void Awake()
    {
        aimLine = GetComponentInChildren<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponentInChildren<Light>();
        weaponName = WeaponStats.M1GarandStats.weaponName;
        fireRate = WeaponStats.M1GarandStats.fireRate;
        weaponDamage = WeaponStats.M1GarandStats.weaponDamage;
        magSize = WeaponStats.M1GarandStats.magSize;
        totalAmmo = WeaponStats.M1GarandStats.totalAmmo;
        reloadTime = WeaponStats.M1GarandStats.reloadTime;
        chamberedRound = WeaponStats.M1GarandStats.chamberedRound;
        viewDist = WeaponStats.M1GarandStats.viewDist;
        isPistol = WeaponStats.M1GarandStats.isPistol;
        setCurrentAmmoInMag();
        setCurrentTotalAmmo();
    }

    // Use this for initialization
    new void Start()
    {
        
    }



    // Update is called once per frame
    void Update () {
		
	}

    
}
