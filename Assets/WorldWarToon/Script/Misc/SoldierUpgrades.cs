using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierUpgrades : MonoBehaviour {

    //Money
    public static int money = 1000;

    //Health
    public static int maxHealth = 200;
    public static int levelHealth = 1;
    public static int healthRegen = 5;
    public static int upgradeCostHealth = 50;


    //Stamina
    public static float maxStamina = 300;
    public static int levelStamina = 1;
    public static int upgradeCostStamina = 50;
    public static float staminaRegen = 50;
    public static float sprintCost = 10f;
    public static float rollCost = 20f;
    

    //MP40
    public static int costMP40 = 0;
    public static bool unlockedMP40 = true;
    public static bool equipMP40 = true;
    public static int weaponDamageMP40 = 40;
    public static int magazineSizeMP40 = 30;
    public static int totalAmmoMP40 = 300;
    public static int upgradeCostMP40 = 30;
    public static int levelMP40 = 1;
    //public static KeyCode slotMP40;
    public static KeyCode slotMP40 = KeyCode.Alpha1;


    //Brengun
    public static int costBrengun = 20;
    public static bool unlockedBrengun = true;
    public static bool equipBrengun = true;
    public static int weaponDamageBrengun = 40;
    public static int magazineSizeBrengun = 80;
    public static int totalAmmoBrengun = 320;
    public static int upgradeCostBrengun = 50;
    public static int levelBrengun = 1;
    //public static KeyCode slotBrengun;
    public static KeyCode slotBrengun = KeyCode.Alpha2;

    //MG42
    public static int costMG42 = 35;
    public static bool unlockedMG42 = true;
    public static bool equipMG42 = true;
    public static int weaponDamageMG42 = 35;
    public static int magazineSizeMG42 = 100;
    public static int totalAmmoMG42 = 300;
    public static int upgradeCostMG42 = 80;
    public static int levelMG42 = 1;
    //public static KeyCode slotMG42;
    public static KeyCode slotMG42 = KeyCode.Alpha3;

    //Thomspon
    public static int costThompson = 15;
    public static bool unlockedThompson = false;
    public static bool equipThompson = false;
    public static int weaponDamageThompson = 20;
    public static int magazineSizeThompson = 45;
    public static int totalAmmoThompson = 315;
    public static int upgradeCostThompson = 30;
    public static int levelThompson = 1;
    public static KeyCode slotThompson;
    //public static KeyCode slotThompson = KeyCode.Alpha4;

    //Stengun
    public static int costStengun = 20;
    public static bool unlockedStengun = false;
    public static bool equipStengun = false;
    public static int weaponDamageStengun = 50;
    public static int magazineSizeStengun = 25;
    public static int totalAmmoStengun = 150;
    public static int upgradeCostStengun = 30;
    public static int levelStengun = 1;
    public static KeyCode slotStengun;
    //public static KeyCode slotStengun = KeyCode.Alpha5;

    //Bazooka
    public static int costBazooka = 40;
    public static bool unlockedBazooka = false;
    public static bool equipBazooka = false;
    public static int weaponDamageBazooka = 150;
    public static int magazineSizeBazooka = 1;
    public static int totalAmmoBazooka = 7;
    public static int upgradeCostBazooka = 100;
    public static int levelBazooka = 1;
    public static KeyCode slotBazooka;
    //public static KeyCode slotBazooka = KeyCode.Alpha6;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(money);
	}
}
