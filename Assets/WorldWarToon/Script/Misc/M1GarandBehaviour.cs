using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1GarandBehaviour : WeaponBehaviour {

    public GameObject M1GarandBullet;

    [SerializeField]
    private Transform GunEnd;

    private void Awake()
    {
        
    }

    // Use this for initialization
    new void Start()
    {
        aimLine = GetComponentInChildren<LineRenderer>();
        gunAudio = GetComponentInChildren<AudioSource>();
        gunLight = GetComponentInChildren<Light>();
        gunEnd = GunEnd;
        bullet = M1GarandBullet;
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
