using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCustomisation : MonoBehaviour {

    [SerializeField]
    public CustomiseWeapons[] customiseWeapons = new CustomiseWeapons[6];


    [System.Serializable]
    public class CustomiseWeapons {
        public string weaponName;
        //public GameObject weaponObject;
        public bool isUnlocked;
        public bool isEquipped;
    }

    // Use this for initialization
    void Start () {
        customiseWeapons = new CustomiseWeapons[6];
        addUnlockedWeapons();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addUnlockedWeapons() {
        Debug.Log("addUnlockedWeapons called");
        //customiseWeapons[0].weaponName = "MP40";
        customiseWeapons[0].isUnlocked = SoldierUpgrades.unlockedMP40;
        customiseWeapons[0].isEquipped = false;

        customiseWeapons[1].weaponName = "Brengun";
        customiseWeapons[1].isUnlocked = SoldierUpgrades.unlockedBrengun;
        customiseWeapons[1].isEquipped = false;

        customiseWeapons[2].weaponName = "MG42";
        customiseWeapons[2].isUnlocked = SoldierUpgrades.unlockedMG42;
        customiseWeapons[2].isEquipped = false;

        customiseWeapons[3].weaponName = "Thompson";
        customiseWeapons[3].isUnlocked = SoldierUpgrades.unlockedThompson;
        customiseWeapons[3].isEquipped = false;

        customiseWeapons[4].weaponName = "Stengun";
        customiseWeapons[4].isUnlocked = SoldierUpgrades.unlockedStengun;
        customiseWeapons[4].isEquipped = false;

        customiseWeapons[5].weaponName = "Bazooka";
        customiseWeapons[5].isUnlocked = SoldierUpgrades.unlockedBazooka;
        customiseWeapons[5].isEquipped = false;


    }
}
