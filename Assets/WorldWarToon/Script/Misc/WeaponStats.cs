using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour {

    /*
     * Fire Rate: The time in between shots the player must wait, higher value means longer wait
     * Weapon Damage: The amount of damage per shot
     * magSize: How much ammo each magazine stores
     * totalAmmo: How much ammo the player can carry for that gun
     * reloadTime: The amount of time it takes for player to reload
     * chamberedRound: If the player can have a chambered round
     * viewDist: The smaller the value the further the player can see around them
     */


    public static class M1GarandStats {
        public static string weaponName = "M1Garand";
        public static int weaponID = 0;
        public static float fireRate = 0.3f;
        public static float weaponDamage = 60;
        public static int magSize = 8;
        public static int totalAmmo = 80;
        public static float reloadTime = 3.2f;
        public static bool chamberedRound = false;
        public static float viewDist = 5f;
        public static bool isPistol = false;
    }

    public static class M1911Stats
    {
        public static string weaponName = "M1911";
        public static int weaponID = 1;
        public static float fireRate = 0.15f;
        public static float weaponDamage = 30;
        public static int magSize = 7;
        public static int totalAmmo = 80;
        public static float reloadTime = 1.5f;
        public static bool chamberedRound = true;
        public static float viewDist = 5f;
        public static bool isPistol = true;
    }







}
