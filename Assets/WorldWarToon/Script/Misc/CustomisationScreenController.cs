using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomisationScreenController : MonoBehaviour {
    public Text money;

    public Text healthText;
    public Text staminaText;

    public Text currentPrimaryWeaponName;
    public Text currentSecondaryWeaponName;
    public Text currentTertiaryWeaponName;

    public Text mp40Text;
    public Text brengunText;
    public Text mg42Text;
    public Text thompsonText;
    public Text stengunText;
    public Text bazookaText;

    private int totalGunsInGame = 6;
    private int currentPrimaryWeapon = -1;
    private int currentSecondaryWeapon = -1;
    private int currentTertiaryWeapon = -1;


    // private CustomiseWeapons[] displayPrimaryWeapons;
    // private CustomiseWeapons[] displaySecondaryWeapons;
    // private CustomiseWeapons[] displayTertiaryWeapons;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha0,
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    [SerializeField]
    public CustomiseWeapons[] customiseWeapons;
    

    [System.Serializable]
    public class CustomiseWeapons {
        public string weaponName;
        public GameObject weaponObject;
        public bool isUnlocked;
        public bool isEquipped;
        public int inventorySlot;

        public string ToString(){
            return weaponName + " " + isUnlocked + " " + isEquipped;
        }
    }

    

    


    // Use this for initialization
    public void Awake () {

        addUnlockedWeapons();
        

        DisplayPrimaryWeapons();
        DisplaySecondaryWeapons();
        DisplayTertiaryWeapons();

    }


    // Update is called once per frame
    public void Update() {
        money.text = "Money: " + SoldierUpgrades.money;
        updateWeaponsUI();
        updateSoldierUpgradesUI();

    }

    public void addUnlockedWeapons() {
        customiseWeapons[0].weaponName = "MP40";
        customiseWeapons[0].isUnlocked = SoldierUpgrades.unlockedMP40;
        customiseWeapons[0].inventorySlot = 0;
        customiseWeapons[0].isEquipped = false;
        

        customiseWeapons[1].weaponName = "Brengun";
        customiseWeapons[1].isUnlocked = SoldierUpgrades.unlockedBrengun;
        customiseWeapons[1].inventorySlot = 0;
        customiseWeapons[1].isEquipped = false;

        customiseWeapons[2].weaponName = "MG42";
        customiseWeapons[2].isUnlocked = SoldierUpgrades.unlockedMG42;
        customiseWeapons[2].inventorySlot = 0;
        customiseWeapons[2].isEquipped = false;

        customiseWeapons[3].weaponName = "Thompson";
        customiseWeapons[3].isUnlocked = SoldierUpgrades.unlockedThompson;
        customiseWeapons[3].inventorySlot = 0;
        customiseWeapons[3].isEquipped = false;

        
        customiseWeapons[4].weaponName = "Stengun";
        customiseWeapons[4].isUnlocked = SoldierUpgrades.unlockedStengun;
        customiseWeapons[4].inventorySlot = 0;
        customiseWeapons[4].isEquipped = false;

        customiseWeapons[5].weaponName = "Bazooka";
        customiseWeapons[5].isUnlocked = SoldierUpgrades.unlockedBazooka;
        customiseWeapons[5].inventorySlot = 0;
        customiseWeapons[5].isEquipped = false;


    }

    public void UpgradeHealth() {
        if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostHealth * SoldierUpgrades.levelHealth)) {
            SoldierUpgrades.maxHealth += 50;
            SoldierUpgrades.healthRegen += 3;
            SoldierUpgrades.levelHealth++;
            SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostHealth * SoldierUpgrades.levelHealth);
        }
    }

    public void UpgradeStamina() {
        if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostStamina * SoldierUpgrades.levelStamina)) {
            SoldierUpgrades.maxStamina += 20;
            SoldierUpgrades.staminaRegen += 5;
            SoldierUpgrades.levelStamina++;
            SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostStamina * SoldierUpgrades.levelStamina);
        }
    }

    public void updateSoldierUpgradesUI() {
        healthText.text = "Upgrade cost: " + (SoldierUpgrades.upgradeCostHealth * SoldierUpgrades.levelHealth);
        staminaText.text = "Upgrade cost: " + (SoldierUpgrades.upgradeCostStamina * SoldierUpgrades.levelStamina);
    }

    public void MP40CustomisationClick() {
        if (!SoldierUpgrades.unlockedMP40) {
            if (SoldierUpgrades.money > SoldierUpgrades.costMP40) {
                SoldierUpgrades.unlockedMP40 = true;
                // index for MP40 is 1
                customiseWeapons[0].isUnlocked = true;
                SoldierUpgrades.money -= SoldierUpgrades.costMP40;
            }
        }
        else {
            if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostMP40 * SoldierUpgrades.levelMP40)) {
                mp40Text.text = "Upgrade Cost: " + (SoldierUpgrades.levelMP40 * SoldierUpgrades.upgradeCostMP40);
                SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostMP40 * SoldierUpgrades.levelMP40);
                SoldierUpgrades.weaponDamageMP40 += SoldierUpgrades.weaponDamageMP40 / 10;
                SoldierUpgrades.magazineSizeMP40 += SoldierUpgrades.magazineSizeMP40 / 5;
                SoldierUpgrades.totalAmmoMP40 += SoldierUpgrades.totalAmmoMP40/ 10;
                SoldierUpgrades.levelMP40++;
            }
        }
    }

    public void BrengunCustomisationClick() {
        if (!SoldierUpgrades.unlockedBrengun) {
            //Display the cost to unlock the gun
            if (SoldierUpgrades.money > SoldierUpgrades.costBrengun) {
                SoldierUpgrades.unlockedBrengun = true;
                Debug.Log("SoldierUpgrades.Brengun unlocked: " + SoldierUpgrades.unlockedBrengun);
                customiseWeapons[1].isUnlocked = true;
                Debug.Log("CustomiseWeapons.Brengun unlocked: " + customiseWeapons[1].isUnlocked);
                SoldierUpgrades.money -= SoldierUpgrades.costBrengun;
            }
        }
        else {
            if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostBrengun * SoldierUpgrades.levelBrengun)) {
                brengunText.text = "Upgrade Cost: " + (SoldierUpgrades.levelBrengun * SoldierUpgrades.upgradeCostBrengun);
                SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostBrengun * SoldierUpgrades.levelBrengun);
                SoldierUpgrades.weaponDamageBrengun += SoldierUpgrades.weaponDamageBrengun / 10;
                SoldierUpgrades.magazineSizeBrengun += SoldierUpgrades.magazineSizeBrengun / 5;
                SoldierUpgrades.totalAmmoBrengun += SoldierUpgrades.totalAmmoBrengun / 10;
                SoldierUpgrades.levelBrengun++;
            }
            
        }
    }

    public void MG42CustomisationClick() {
        if (!SoldierUpgrades.unlockedMG42) {
            //Display the cost to unlock the gun
            if (SoldierUpgrades.money > SoldierUpgrades.costMG42) {
                SoldierUpgrades.unlockedMG42 = true;
                customiseWeapons[2].isUnlocked = true;
                SoldierUpgrades.money -= SoldierUpgrades.costMG42;
            }
        }
        else {
            if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostMG42 * SoldierUpgrades.levelMG42)) {
                mg42Text.text = "Upgrade Cost: " + (SoldierUpgrades.levelMG42 * SoldierUpgrades.upgradeCostMG42);
                SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostMG42 * SoldierUpgrades.levelMG42);
                SoldierUpgrades.weaponDamageMG42 += SoldierUpgrades.weaponDamageMG42 / 10;
                SoldierUpgrades.magazineSizeMG42 += SoldierUpgrades.magazineSizeMG42 / 5;
                SoldierUpgrades.totalAmmoMG42 += SoldierUpgrades.totalAmmoMG42 / 10;
                SoldierUpgrades.levelMG42++;
            }

        }
    }

    public void ThompsonCustomisationClick() {
        if (!SoldierUpgrades.unlockedThompson) {
            //Display the cost to unlock the gun
            if (SoldierUpgrades.money > SoldierUpgrades.costThompson) {
                SoldierUpgrades.unlockedThompson = true;
                customiseWeapons[3].isUnlocked = true;
                SoldierUpgrades.money -= SoldierUpgrades.costThompson;
            }
        }
        else {
            if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostThompson * SoldierUpgrades.levelThompson)) {
                thompsonText.text = "Upgrade Cost: " + (SoldierUpgrades.levelThompson * SoldierUpgrades.upgradeCostThompson);
                SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostThompson * SoldierUpgrades.levelThompson);
                SoldierUpgrades.weaponDamageThompson += SoldierUpgrades.weaponDamageThompson / 10;
                SoldierUpgrades.magazineSizeThompson += SoldierUpgrades.magazineSizeThompson / 5;
                SoldierUpgrades.totalAmmoThompson += SoldierUpgrades.totalAmmoThompson / 10;
                SoldierUpgrades.levelThompson++;
            }
        }
    }

    public void StengunCustomisationClick() {
        if (!SoldierUpgrades.unlockedStengun) {
            //Display the cost to unlock the gun
            if (SoldierUpgrades.money > SoldierUpgrades.costStengun) {
                SoldierUpgrades.unlockedStengun = true;
                customiseWeapons[4].isUnlocked = true;
                SoldierUpgrades.money -= SoldierUpgrades.costStengun;
            }
        }
        else {
            if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostStengun * SoldierUpgrades.levelStengun)) {
                stengunText.text = "Upgrade Cost: " + (SoldierUpgrades.levelStengun * SoldierUpgrades.upgradeCostStengun);
                SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostStengun * SoldierUpgrades.levelStengun);
                SoldierUpgrades.weaponDamageStengun += SoldierUpgrades.weaponDamageStengun / 10;
                SoldierUpgrades.magazineSizeStengun += SoldierUpgrades.magazineSizeStengun / 5;
                SoldierUpgrades.totalAmmoStengun += SoldierUpgrades.totalAmmoStengun / 10;
                SoldierUpgrades.levelStengun++;
            }
        }
    }

    public void BazookaCustomisationClick() {
        if (!SoldierUpgrades.unlockedBazooka) {
            //Display the cost to unlock the gun
            if (SoldierUpgrades.money > SoldierUpgrades.costBazooka) {
                SoldierUpgrades.unlockedBazooka = true;
                customiseWeapons[5].isUnlocked = true;
                SoldierUpgrades.money -= SoldierUpgrades.costBazooka;
            }
        }
        else {
            if (SoldierUpgrades.money > (SoldierUpgrades.upgradeCostBazooka * SoldierUpgrades.levelBazooka)) {
                bazookaText.text = "Upgrade Cost: " + (SoldierUpgrades.levelBazooka * SoldierUpgrades.upgradeCostBazooka);
                SoldierUpgrades.money -= (SoldierUpgrades.upgradeCostBazooka * SoldierUpgrades.levelBazooka);
                SoldierUpgrades.weaponDamageBazooka += SoldierUpgrades.weaponDamageBazooka;
                SoldierUpgrades.totalAmmoBazooka += SoldierUpgrades.levelBazooka * 2;
                SoldierUpgrades.levelBazooka++;
            }
        }
    }





    public void updateWeaponsUI() {
        if (!SoldierUpgrades.unlockedMP40) {
            mp40Text.text = "Unlock Cost: " + SoldierUpgrades.costMP40;
        }
        else {
            mp40Text.text = "Upgrade Cost: " + (SoldierUpgrades.levelMP40 * SoldierUpgrades.upgradeCostMP40);
        }

        if (!SoldierUpgrades.unlockedBrengun) {
            brengunText.text = "Unlock Cost: " + SoldierUpgrades.costBrengun;
        }
        else {
            brengunText.text = "Upgrade Cost: " + (SoldierUpgrades.levelBrengun * SoldierUpgrades.upgradeCostBrengun);
        }

        if (!SoldierUpgrades.unlockedMG42) {
            mg42Text.text = "Unlock Cost: " + SoldierUpgrades.costMG42;
        }
        else {
            mg42Text.text = "Upgrade Cost: " + (SoldierUpgrades.levelMG42 * SoldierUpgrades.upgradeCostMG42);
        }

        if (!SoldierUpgrades.unlockedThompson) {
            thompsonText.text = "Unlock Cost: " + SoldierUpgrades.costThompson;
        }
        else {
            thompsonText.text = "Upgrade Cost: " + (SoldierUpgrades.levelThompson * SoldierUpgrades.upgradeCostThompson);
        }

        if (!SoldierUpgrades.unlockedStengun) {
            stengunText.text = "Unlock Cost: " + SoldierUpgrades.costStengun;
        }
        else {
            stengunText.text = "Upgrade Cost: " + (SoldierUpgrades.levelStengun * SoldierUpgrades.upgradeCostStengun);
        }

        if (!SoldierUpgrades.unlockedBazooka) {
            bazookaText.text = "Unlock Cost: " + SoldierUpgrades.costBazooka;
        }
        else {
            bazookaText.text = "Upgrade Cost: " + (SoldierUpgrades.levelBazooka * SoldierUpgrades.upgradeCostBazooka);
        }
    }

    public void PlayGame() {
        equipWeapons();
        for (int i = 0; i < totalGunsInGame; i++) {
            Debug.Log(customiseWeapons[i].weaponName + ": " + customiseWeapons[i].inventorySlot);
        }
        SceneManager.LoadScene("Amiens");
    }

    public void equipWeapons() {
        if (customiseWeapons[0].isEquipped) {
            Debug.Log("MP40 is equipped");
            SoldierUpgrades.equipMP40 = true;
            SoldierUpgrades.slotMP40 = keyCodes[customiseWeapons[0].inventorySlot];
        }
        if (customiseWeapons[1].isEquipped) {
            //Debug.Log("Brengun is equipped");
            SoldierUpgrades.equipBrengun = true;
            SoldierUpgrades.slotBrengun = keyCodes[customiseWeapons[1].inventorySlot];
            
        }
        if (customiseWeapons[2].isEquipped) {
            //Debug.Log("MG42 is equipped");
            SoldierUpgrades.equipMG42 = true;
            SoldierUpgrades.slotMG42 = keyCodes[customiseWeapons[2].inventorySlot];
        }
        if (customiseWeapons[3].isEquipped) {
            SoldierUpgrades.equipThompson = true;
            SoldierUpgrades.slotThompson = keyCodes[customiseWeapons[3].inventorySlot];
        }
        if (customiseWeapons[4].isEquipped) {
            SoldierUpgrades.equipStengun = true;
            SoldierUpgrades.slotStengun = keyCodes[customiseWeapons[4].inventorySlot];
        }
        if (customiseWeapons[5].isEquipped) {
            SoldierUpgrades.equipBazooka = true;
            SoldierUpgrades.slotBazooka = keyCodes[customiseWeapons[5].inventorySlot];
        }
    }




    public void ExitMenu() {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Game has been backed");
    }

    
    public void DisplayPrimaryWeapons() {
        //displayPrimaryWeapons = new CustomiseWeapons[totalGunsInGame];
        for (int a = 0; a < totalGunsInGame; a++) {
            if (customiseWeapons[a].isUnlocked && !customiseWeapons[a].isEquipped) {
                currentPrimaryWeapon = a;
                customiseWeapons[a].isEquipped = true;
                customiseWeapons[a].inventorySlot = 1;
                currentPrimaryWeaponName.text = customiseWeapons[a].weaponName;
                return;

                // displayPrimaryWeapons[a] = customiseWeapons[a];
                // customiseWeapons[a].isEquipped = true;
                // customiseWeapons[a].inventorySlot = 1;
                // currentPrimaryWeaponName.text = customiseWeapons[a].weaponName;
                
            }
        }

      
        currentPrimaryWeaponName.text = "None available";
        
        
    }

    public void switchPrimaryWeaponRightButton() {
        int availableGuns = 0;

        for(int i = 0; i < totalGunsInGame; i++){
            if(customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped){
                availableGuns++;
            }
        }

        if(availableGuns == 0){
            return;
        }
        // if(currentPrimaryWeaponName.text == "None available"){
        //     return;
        // }

        // for(int i = 0; i < totalGunsInGame; i++){
        //     Debug.Log("i: " + i);
        //     Debug.Log("weapon: " + customiseWeapons[i].ToString());
        //     if (customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped 
        //          && (currentPrimaryWeaponName.text != customiseWeapons[i].weaponName)) {
        //              Debug.Log("available i: " + i);
        //         //Swapping out current weapon
        //         customiseWeapons[currentPrimaryWeapon].isEquipped = false;
        //         customiseWeapons[currentPrimaryWeapon].inventorySlot = 0;
        //         //Replacing with new weapon
        //         currentPrimaryWeapon = i;
        //         customiseWeapons[i].isEquipped = true;
        //         customiseWeapons[i].inventorySlot = 1;
        //         currentPrimaryWeaponName.text = customiseWeapons[i].weaponName;
                
        //         return;
        //     }

        // }
        bool nonAvailable = false;
        for (int b = currentPrimaryWeapon; b <= totalGunsInGame; b++) {
            if (b == totalGunsInGame) {
                b = 0;
                if (currentPrimaryWeapon == -1 && nonAvailable) {
                    currentPrimaryWeaponName.text = "None available";
                   return;
                }
                if (b == currentPrimaryWeapon && nonAvailable) {
                    return;
                }
                nonAvailable = true;
            }
            
            if (customiseWeapons[b].isUnlocked && !customiseWeapons[b].isEquipped) {
                //Swapping out current weapon
                customiseWeapons[currentPrimaryWeapon].isEquipped = false;
                customiseWeapons[currentPrimaryWeapon].inventorySlot = 0;
                //Replacing with new weapon
                currentPrimaryWeapon = b;
                customiseWeapons[b].isEquipped = true;
                customiseWeapons[b].inventorySlot = 1;
                currentPrimaryWeaponName.text = customiseWeapons[b].weaponName;
                
                return;
            }
        }

        // if(displayPrimaryWeapons == null){
        //     currentPrimaryWeaponName.text = "None available";
        //     return;
        // }

        // for(int i = 0; i <= totalGunsInGame; i++){
        //     if(displayPrimaryWeapons[i] != null){
                
        //     }
        // }
        
    }

    public void switchPrimaryWeaponLeftButton() {
        int availableGuns = 0;

        for(int i = 0; i < totalGunsInGame; i++){
            if(customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped){
                availableGuns++;
            }
        }

        if(availableGuns == 0){
            return;
        }
        bool nonAvailable = false;
        for (int j = currentPrimaryWeapon; j >= -1; j--) {
            if (j == - 1) {
                j = totalGunsInGame-1;
                if (currentPrimaryWeapon == -1 && nonAvailable) {
                    currentPrimaryWeaponName.text = "None available";
                    return;
                }
                if (j == currentPrimaryWeapon && nonAvailable) {
                    return;
                }
                nonAvailable = true;
            }
            
            if (customiseWeapons[j].isUnlocked && !customiseWeapons[j].isEquipped) {
                customiseWeapons[currentPrimaryWeapon].isEquipped = false;
                customiseWeapons[currentPrimaryWeapon].inventorySlot = 0;
                //Display text
                currentPrimaryWeapon = j;
                customiseWeapons[j].isEquipped = true;
                customiseWeapons[j].inventorySlot = 1;
                currentPrimaryWeaponName.text = customiseWeapons[j].weaponName;
                //Button text will show this
                return;
            }
        }
    }

    public void DisplaySecondaryWeapons() {
        for (int d = 0; d < totalGunsInGame; d++) {
            if (customiseWeapons[d].isUnlocked && !customiseWeapons[d].isEquipped) {
                currentSecondaryWeapon = d;
                customiseWeapons[d].isEquipped = true;
                customiseWeapons[d].inventorySlot = 2;
                currentSecondaryWeaponName.text = customiseWeapons[d].weaponName;
                return;

            }
        }
        currentSecondaryWeaponName.text = "None available";
    }

    public void switchSecondaryWeaponRightButton() {
        int availableGuns = 0;

        for(int i = 0; i < totalGunsInGame; i++){
            if(customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped){
                availableGuns++;
            }
        }

        if(availableGuns == 0){
            return;
        }

        bool nonAvailable = false;
        for (int e = currentSecondaryWeapon; e <= totalGunsInGame; e++) {
            if (e == (totalGunsInGame)) {
                e = 0;
                if (currentSecondaryWeapon == -1 && nonAvailable) {
                    currentSecondaryWeaponName.text = "None available";
                   return;
                }
                if (e == currentSecondaryWeapon && nonAvailable) {
                    return;
                }
                nonAvailable = true;
                //return;
            }
            Debug.Log("e: " + e);
            if (e != -1 && customiseWeapons[e].isUnlocked && !customiseWeapons[e].isEquipped) {
                //Swapping out current weapon
                if(currentSecondaryWeapon != -1){
                    customiseWeapons[currentSecondaryWeapon].isEquipped = false;
                    customiseWeapons[currentSecondaryWeapon].inventorySlot = 0;

                }
                
                //Replacing with new weapon
                currentSecondaryWeapon = e;
                customiseWeapons[e].isEquipped = true;
                customiseWeapons[e].inventorySlot = 2;
                currentSecondaryWeaponName.text = customiseWeapons[e].weaponName;
                //Button text will show this
                return;
            }
        }
    }


    public void switchSecondaryWeaponLeftButton() {
        int availableGuns = 0;

        for(int i = 0; i < totalGunsInGame; i++){
            if(customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped){
                availableGuns++;
            }
        }

        if(availableGuns == 0){
            return;
        }
        bool nonAvailable = false;
        for (int j = currentSecondaryWeapon; j >= -1; j--) {
            if (j == -1) {
                j = totalGunsInGame - 1;

                if (currentSecondaryWeapon == -1 && nonAvailable) {
                    currentSecondaryWeaponName.text = "None available";
                   return;
                }
                if (j == currentSecondaryWeapon && nonAvailable) {
                    return;
                }
                nonAvailable = true;
            }
            if (j != -1 && customiseWeapons[j].isUnlocked && !customiseWeapons[j].isEquipped) {
                if(currentSecondaryWeapon != -1){
                    customiseWeapons[currentSecondaryWeapon].isEquipped = false;
                    customiseWeapons[currentSecondaryWeapon].inventorySlot = 0;

                }
               
                //Display text
                currentSecondaryWeapon = j;
                customiseWeapons[j].isEquipped = true;
                customiseWeapons[j].inventorySlot = 1;
                currentSecondaryWeaponName.text = customiseWeapons[j].weaponName;
                //Button text will show this
                return;
            }
        }
    }


    public void DisplayTertiaryWeapons() {
        for (int g = 0; g < totalGunsInGame; g++) {
            if (customiseWeapons[g].isUnlocked && !customiseWeapons[g].isEquipped) {
                //Display text
                currentTertiaryWeapon = g;
                customiseWeapons[g].isEquipped = true;
                customiseWeapons[g].inventorySlot = 3;
                //Button text will show this
                currentTertiaryWeaponName.text = customiseWeapons[g].weaponName;
                return;
            }
        }
        currentTertiaryWeaponName.text = "None available";
    }

    public void switchTertiaryWeaponRightButton() {
        int availableGuns = 0;

        for(int i = 0; i < totalGunsInGame; i++){
            if(customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped){
                availableGuns++;
            }
        }

        if(availableGuns == 0){
            return;
        }
        bool nonAvailable = false;
        for (int h = currentTertiaryWeapon; h <= totalGunsInGame; h++) {
            if (h == totalGunsInGame) {
                if (h == currentTertiaryWeapon && nonAvailable) {
                    return;
                }
                h = 0;
                if (currentTertiaryWeapon == -1 && nonAvailable) {
                    currentTertiaryWeaponName.text = "None available";
                   return;
                }
                
                nonAvailable = true;
                // return;
            }
            if (h != -1 && customiseWeapons[h].isUnlocked && !customiseWeapons[h].isEquipped) {
                //Swapping out current weapon
                if(currentTertiaryWeapon != -1){
                    customiseWeapons[currentTertiaryWeapon].isEquipped = false;
                    customiseWeapons[currentTertiaryWeapon].inventorySlot = 0;

                }
                
                //Replacing with new weapon
                currentTertiaryWeapon = h;
                customiseWeapons[h].isEquipped = true;
                customiseWeapons[h].inventorySlot = 3;
                currentTertiaryWeaponName.text = customiseWeapons[h].weaponName;
                return;
            }
        }
    }

    public void switchTertiaryWeaponLeftButton() {
        int availableGuns = 0;

        for(int i = 0; i < totalGunsInGame; i++){
            if(customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped){
                availableGuns++;
            }
        }

        if(availableGuns == 0){
            return;
        }
        bool nonAvailable = false;
        for (int i = currentTertiaryWeapon; i >= -1; i--) {
            if (i == -1) {
                i = totalGunsInGame - 1;

                if (currentTertiaryWeapon == -1 && nonAvailable) {
                    currentTertiaryWeaponName.text = "None available";
                   return;
                }
                if (i== currentTertiaryWeapon && nonAvailable) {
                    return;
                }
                nonAvailable = true;
            }
            if (i != -1 && customiseWeapons[i].isUnlocked && !customiseWeapons[i].isEquipped) {
                if(currentTertiaryWeapon != -1){
                     customiseWeapons[currentTertiaryWeapon].isEquipped = false;
                    customiseWeapons[currentTertiaryWeapon].inventorySlot = 0;

                }
               
                //Display text
                currentTertiaryWeapon = i;
                customiseWeapons[i].isEquipped = true;
                customiseWeapons[i].inventorySlot = 1;
                currentTertiaryWeaponName.text = customiseWeapons[i].weaponName;
                //Button text will show this
                return;
            }
        }
    }


}
