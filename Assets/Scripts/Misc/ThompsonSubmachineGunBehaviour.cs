using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThompsonSubmachineGunBehaviour : WeaponBehaviour
{

    private void Awake()
    {
        aimLine = GetComponentInChildren<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponentInChildren<Light>();
        weaponName = WeaponStats.ThompsonSubmachineGunStats.weaponName;
        fireRate = WeaponStats.ThompsonSubmachineGunStats.fireRate;
        weaponDamage = WeaponStats.ThompsonSubmachineGunStats.weaponDamage;
        magSize = WeaponStats.ThompsonSubmachineGunStats.magSize;
        totalAmmo = WeaponStats.ThompsonSubmachineGunStats.totalAmmo;
        reloadTime = WeaponStats.ThompsonSubmachineGunStats.reloadTime;
        chamberedRound = WeaponStats.ThompsonSubmachineGunStats.chamberedRound;
        viewDist = WeaponStats.ThompsonSubmachineGunStats.viewDist;
        Debug.Log("@Thompson view dist: " + WeaponStats.ThompsonSubmachineGunStats.viewDist);
        isPistol = WeaponStats.ThompsonSubmachineGunStats.isPistol;
        setCurrentAmmoInMag();
        setCurrentTotalAmmo();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
