using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911Behaviour : WeaponBehaviour
{
    

    private void Awake()
    {
        aimLine = GetComponentInChildren<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponentInChildren<Light>();
        
        weaponName = WeaponStats.M1911Stats.weaponName;
        fireRate = WeaponStats.M1911Stats.fireRate;
        weaponDamage = WeaponStats.M1911Stats.weaponDamage;
        magSize = WeaponStats.M1911Stats.magSize;
        totalAmmo = WeaponStats.M1911Stats.totalAmmo;
        reloadTime = WeaponStats.M1911Stats.reloadTime;
        chamberedRound = WeaponStats.M1911Stats.chamberedRound;
        viewDist = WeaponStats.M1911Stats.viewDist;
        isPistol = WeaponStats.M1911Stats.isPistol;
        setCurrentAmmoInMag();
        setCurrentTotalAmmo();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
