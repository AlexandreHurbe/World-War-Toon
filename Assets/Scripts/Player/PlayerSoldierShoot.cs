using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSoldierShoot : MonoBehaviour {

    public Text ammoCount;
    public WeaponSetUp.Weapons currentWeapon;


    private PlayerSoldierMovement playerSoldierMovement;
    private AudioSource audioSource; 
    private Vector3 playerToMouse;
    private LineRenderer aimLine;
    private int floorMask;
    private float timer;
    private int currentMagAmmo;
    private int currentTotalAmmo;
    private bool reloadAnimFinish = false;
    private bool magFilled = true;
    private bool canShoot = true;
    private bool isReloading = false;
    private bool isAiming = false;
    private float currentWeight = 0;
    private bool waiting;
    private Animator anim;

    [System.Serializable]
    public class WeaponSetUp {

        [Tooltip("The weapon flash object that will play out the end of the gun")]
        //public GameObject weaponFlash;

        public Weapons[] weapons;


        

        [System.Serializable]
        public class Weapons {
            [Tooltip("Weapon Name Used for selecting wich animation to chose in animator")]
            public string weaponName;

            [Tooltip("Weapon object for switching out the weapon")]
            public GameObject WeaponGameobject;

            [Tooltip("Checks if weapon is equipped on soldier")]
            public bool isEquipped = false;

            [Tooltip("Bullet GameObject you want to use - leave clear if you do not want one")]
            public GameObject bulletGameobject;

            [Tooltip("Weapon object for switching out the weapon")]
            public float switchOutTime = 0.2f;

            [Tooltip("The empty transform placed at the end of the gun, used all sorts of gun logic")]
            public Transform weaponEnd;

            [Tooltip("Gun fire effect")]
            public ParticleSystem gunFireEffect;

            [Tooltip("Spark effect when bullets hit any none humen objects")]
            public ParticleSystem sparkEffect;

            [Tooltip("This is used by the left hand Ik for the character to have its left hand placed in the correct position on this gun")]
            public Transform leftHandPos;

            [Tooltip("How much health this will take away from enemys")]
            public int weaponDamage;

            [Tooltip("How far this weapon can hit the target")]
            public float weaponRange;

            [Tooltip("How fast you can shot the gun")]
            public float bulletSpeed;

            [Tooltip("Time interval between bullets")]
            public float fireRate;

            [Tooltip("Magazine/Clip Size")]
            public int magazineSize;

            [Tooltip("current Magazine Ammo")]
            public int currentMagAmmo;


            [Tooltip("Total Ammo")]
            public int totalAmmo;

            [Tooltip("current Total Ammo")]
            public int currentTotalAmmo;

            [Tooltip("The time it takes to reload")]
            public float reloadTime = 1f;

            [Tooltip("Bullet deviation")]
            public float strayFactor;

            [Tooltip("Which Key do you want to press to use the weapon")]
            public KeyCode switchWeaponKey;

            [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
            public float aimingZ = -80f;

            [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
            public float aimingY = -80f;

            [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
            public float aimingX = -9f;

            [Tooltip("Weapon Sound should be added to this slot")]
            public AudioClip shootAudio;

            [Tooltip("Weapon reload should be added to this slot")]
            public AudioClip reloadAudio;
        }
    }

    [System.Serializable]
    public class IkSetUp
    {
        public Transform spine;
    }

    [SerializeField]
    IkSetUp ikSetUp;

    [SerializeField]
    [Tooltip("How many weapons do you want to have")]
    WeaponSetUp weaponSetUp;
    

    


    private void Awake() {
        anim = GetComponent<Animator>();
        audioSource = transform.GetComponent<AudioSource>();
        aimLine = GetComponent<LineRenderer>();
        playerSoldierMovement = GetComponent<PlayerSoldierMovement>();
        floorMask = LayerMask.GetMask("Floor");
        SelectWeapons();
        SelectInitialWeapon();
        //AudioSource = transform.GetComponent<AudioSource>();

        //Setting up lineRenderer settings
        aimLine.startWidth = 0.04f;
        aimLine.endWidth = 0.04f;
        aimLine.startColor = Color.white;
        aimLine.endColor = Color.white;
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        aimLine.material = whiteDiffuseMat;
    }

    private void SelectInitialWeapon() {
        foreach (WeaponSetUp.Weapons item in weaponSetUp.weapons) {
            //Debug.Log(item.weaponName+": "+item.switchWeaponKey);
            if (item.switchWeaponKey == KeyCode.Alpha1) {
                weaponSetUp.weapons[0].WeaponGameobject.SetActive(false);
                anim.SetBool(weaponSetUp.weapons[0].weaponName, false);
                currentWeapon = item;
                currentWeapon.currentMagAmmo = currentWeapon.magazineSize;
                currentWeapon.currentTotalAmmo = currentWeapon.totalAmmo;
                item.WeaponGameobject.SetActive(true);
                anim.SetBool(item.weaponName, true);
            }

            item.currentMagAmmo = item.magazineSize;
            item.currentTotalAmmo = item.totalAmmo;
        }
    }

    private void SelectWeapons() {
        if (SoldierUpgrades.equipMP40) {
            //Debug.Log("MP40 is equipped");
            weaponSetUp.weapons[0].isEquipped = SoldierUpgrades.equipMP40;
            weaponSetUp.weapons[0].weaponDamage = SoldierUpgrades.weaponDamageMP40;
            weaponSetUp.weapons[0].magazineSize = SoldierUpgrades.magazineSizeMP40;
            weaponSetUp.weapons[0].totalAmmo = SoldierUpgrades.totalAmmoMP40;
            weaponSetUp.weapons[0].switchWeaponKey = SoldierUpgrades.slotMP40;
        }
        else {
            //Debug.Log("MP40 not equipped");
            weaponSetUp.weapons[0].switchWeaponKey = KeyCode.Keypad0;
        }

        if (SoldierUpgrades.equipBrengun) {
            //Debug.Log("Brengun is equipped");
            weaponSetUp.weapons[1].isEquipped = SoldierUpgrades.equipBrengun;
            weaponSetUp.weapons[1].weaponDamage = SoldierUpgrades.weaponDamageBrengun;
            weaponSetUp.weapons[1].magazineSize = SoldierUpgrades.magazineSizeBrengun;
            weaponSetUp.weapons[1].totalAmmo = SoldierUpgrades.totalAmmoBrengun;
            weaponSetUp.weapons[1].switchWeaponKey = SoldierUpgrades.slotBrengun;
        }
        else {
            //Debug.Log("Brengun not equipped");
            weaponSetUp.weapons[1].switchWeaponKey = KeyCode.Keypad1;
        }
        if (SoldierUpgrades.equipMG42) {
            //Debug.Log("MG42 is equipped");
            weaponSetUp.weapons[2].isEquipped = SoldierUpgrades.equipMG42;
            weaponSetUp.weapons[2].weaponDamage = SoldierUpgrades.weaponDamageMG42;
            weaponSetUp.weapons[2].magazineSize = SoldierUpgrades.magazineSizeMG42;
            weaponSetUp.weapons[2].totalAmmo = SoldierUpgrades.totalAmmoMG42;
            weaponSetUp.weapons[2].switchWeaponKey = SoldierUpgrades.slotMG42;
        }
        else {
            //Debug.Log("MG42 not equipped");
            weaponSetUp.weapons[2].switchWeaponKey = KeyCode.Keypad2;
        }
        if (SoldierUpgrades.equipThompson) {
            weaponSetUp.weapons[3].isEquipped = SoldierUpgrades.equipThompson;
            weaponSetUp.weapons[3].weaponDamage = SoldierUpgrades.weaponDamageThompson;
            weaponSetUp.weapons[3].magazineSize = SoldierUpgrades.magazineSizeThompson;
            weaponSetUp.weapons[3].totalAmmo = SoldierUpgrades.totalAmmoThompson;
            weaponSetUp.weapons[3].switchWeaponKey = SoldierUpgrades.slotThompson;
        }
        else {
            weaponSetUp.weapons[3].switchWeaponKey = KeyCode.Keypad3;
        }
        if (SoldierUpgrades.equipStengun) {
            weaponSetUp.weapons[4].isEquipped = SoldierUpgrades.equipStengun;
            weaponSetUp.weapons[4].weaponDamage = SoldierUpgrades.weaponDamageStengun;
            weaponSetUp.weapons[4].magazineSize = SoldierUpgrades.magazineSizeStengun;
            weaponSetUp.weapons[4].totalAmmo = SoldierUpgrades.totalAmmoStengun;
            weaponSetUp.weapons[4].switchWeaponKey = SoldierUpgrades.slotStengun;
        }
        else {
            weaponSetUp.weapons[4].switchWeaponKey = KeyCode.Keypad4;
        }
        if (SoldierUpgrades.equipBazooka){
            weaponSetUp.weapons[5].isEquipped = SoldierUpgrades.equipBazooka;
            weaponSetUp.weapons[5].weaponDamage = SoldierUpgrades.weaponDamageBazooka;
            weaponSetUp.weapons[5].magazineSize = SoldierUpgrades.magazineSizeBazooka;
            weaponSetUp.weapons[5].totalAmmo = SoldierUpgrades.totalAmmoBazooka;
            weaponSetUp.weapons[5].switchWeaponKey = SoldierUpgrades.slotBazooka;
        }
        else {
            weaponSetUp.weapons[5].switchWeaponKey = KeyCode.Keypad5;
        }
    }


    // Use this for initialization
    void Start () {
        ammoCount.text = currentWeapon.currentMagAmmo + "  /  " + currentWeapon.currentTotalAmmo;
    }

    private void Update() {
        
        ammoCount.text = currentWeapon.currentMagAmmo + "  /  " + currentWeapon.currentTotalAmmo;

        playerToMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        playerToMouse = Camera.main.ScreenToWorldPoint(playerToMouse);
        
        playerToMouse.y = 0f;
        


        WeaponSwitch();

        if (isReloading) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            CollectAmmo();
        }
    }

    private void LateUpdate() {
        pointAtCamera();
        if (Input.GetButton("Fire2")) {
            playerSoldierMovement.SetCrouching(false);
            isAiming = true;
            Aim(isAiming);
            if ((Input.GetButton("Fire1")) && (canShoot) && (currentWeapon.currentMagAmmo > 0) && (isReloading == false)) {
                StartCoroutine(shootSpeed());
                Fire();
            }
            else if (currentWeapon.currentMagAmmo <= 0) {
                magFilled = false;
                StartCoroutine(ReloadAnimTimer());
            }
        }
        else {
            isAiming = false;
            Aim(isAiming);
        }
        
    }

    IEnumerator shootSpeed() {
        canShoot = false;
        yield return new WaitForSeconds(currentWeapon.fireRate);
        canShoot = true;

    }

    private void CollectAmmo() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) {

            AmmoBox ammoBox = hit.collider.transform.root.GetComponent<AmmoBox>();
            if (ammoBox != null) {
                
                ammoBox.CollectAmmoSound();
                switch (currentWeapon.weaponName) {
                    case "MP40":
                        currentWeapon.currentTotalAmmo = SoldierUpgrades.totalAmmoMP40;
                        break;
                    case "Brengun":
                        currentWeapon.currentTotalAmmo = SoldierUpgrades.totalAmmoBrengun;
                        break;
                    case "MG42":
                        currentWeapon.currentTotalAmmo = SoldierUpgrades.totalAmmoMG42;
                        break;
                    case "Thompson":
                        currentWeapon.currentTotalAmmo = SoldierUpgrades.totalAmmoThompson;
                        break;
                    case "Stengun":
                        currentWeapon.currentTotalAmmo = SoldierUpgrades.totalAmmoStengun;
                        break;
                    case "Bazooka":
                        currentWeapon.currentTotalAmmo = SoldierUpgrades.totalAmmoBazooka;
                        break;
                }
            }
        }
        
    }
    

    private void Reload() {
        if (reloadAnimFinish && magFilled == false) {
            
            reloadAnimFinish = false;
            currentWeapon.currentTotalAmmo -= currentWeapon.magazineSize - currentWeapon.currentMagAmmo;
            currentWeapon.currentMagAmmo = currentWeapon.magazineSize;
            magFilled = true;
            isReloading = false;
        }
        else if(reloadAnimFinish && magFilled){
            isReloading = false;
        }
    }


    IEnumerator ReloadAnimTimer() {

        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(currentWeapon.reloadAudio);
        }
        reloadAnimFinish = false;
        anim.SetBool("isReloading", true);
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        reloadAnimFinish = true;
        anim.SetBool("isReloading", false);
        Reload();
        reloadAnimFinish = false;
    }

    void Aim(bool isAiming) {
        if (isAiming) {
            currentWeight = Mathf.Lerp(currentWeight, 1, Time.deltaTime * 10);
            anim.SetLayerWeight(1, currentWeight);
            anim.SetBool(currentWeapon.weaponName, true);
            //drawAimLine(true);
        }
        else {
            currentWeight = Mathf.Lerp(currentWeight, 0, Time.deltaTime * 10);
            anim.SetLayerWeight(1, currentWeight);
            anim.SetBool(currentWeapon.weaponName, false);
            //drawAimLine(false);
        }
    }

    private void drawAimLine(bool turnAimOn) {
        if (turnAimOn) {
            Ray aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit aimHit;
            if (Physics.Raycast(aimRay, out aimHit, 50f, floorMask)) {
                aimLine.SetPosition(0, currentWeapon.weaponEnd.position);
                Vector3 distanceGunToMouse = aimHit.point - this.transform.position;
                Vector3 offset = new Vector3(0, 0, 4);
                aimLine.SetPosition(1, (this.currentWeapon.weaponEnd.position+distanceGunToMouse-offset));
                aimLine.enabled = true;
            }
        }
        else {
            aimLine.enabled = false;
        }
        
    }


    private void pointAtCamera()
    {
        
        Vector3 eulerAngleOffset = Vector3.zero;
        eulerAngleOffset = new Vector3(currentWeapon.aimingX, currentWeapon.aimingY, currentWeapon.aimingZ);
        
        ikSetUp.spine.transform.Rotate(eulerAngleOffset, Space.Self);
    }

    void Fire() {
        audioSource.Stop();
        audioSource.PlayOneShot(currentWeapon.shootAudio, 0.5f);
        if (currentWeapon.weaponName != "Bazooka") {
            currentWeapon.currentMagAmmo--;
            ammoCount.text = currentWeapon.currentMagAmmo + "  /  " + currentWeapon.currentTotalAmmo;
            

            currentWeapon.gunFireEffect.transform.position = currentWeapon.weaponEnd.transform.position;
            currentWeapon.gunFireEffect.transform.rotation = currentWeapon.weaponEnd.transform.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);
            currentWeapon.gunFireEffect.Play();
            float randomNumberX = Random.Range(-currentWeapon.strayFactor, currentWeapon.strayFactor);
            float randomNumberZ = Random.Range(-currentWeapon.strayFactor, currentWeapon.strayFactor);

            GameObject bullet = Instantiate(currentWeapon.bulletGameobject, currentWeapon.weaponEnd.transform.position, currentWeapon.weaponEnd.transform.rotation);
            bullet.transform.Rotate(0, randomNumberZ, randomNumberX);
            BulletBehaviour bulletProperties = bullet.GetComponent<BulletBehaviour>();
            bulletProperties.setDmg(currentWeapon.weaponDamage);
            bulletProperties.setPlayerFriendly(true);
            bulletProperties.setSparkEffect(currentWeapon.sparkEffect);
        }
        else {
            currentWeapon.currentMagAmmo--;
            ammoCount.text = currentWeapon.currentMagAmmo + "  /  " + currentWeapon.currentTotalAmmo;
            
            GameObject bazookaRocket = Instantiate(currentWeapon.bulletGameobject, currentWeapon.weaponEnd.transform.position, currentWeapon.weaponEnd.transform.rotation);
            BazookaRocketBehaviour bazookaRocketProperties = bazookaRocket.GetComponent<BazookaRocketBehaviour>();
            bazookaRocketProperties.setDmg(currentWeapon.weaponDamage);
            bazookaRocketProperties.setSpeed(currentWeapon.bulletSpeed);
            bazookaRocketProperties.setPlayerFriendly(true);
        }
    }

    /*This function was taken from LowPolyWarPack by Polyperfect*/
    void WeaponSwitch() {
        foreach (WeaponSetUp.Weapons item in weaponSetUp.weapons) {
            if (Input.GetKeyDown(item.switchWeaponKey) && item.isEquipped) {
                currentWeapon = item;
                currentMagAmmo = currentWeapon.magazineSize;
                currentTotalAmmo = currentWeapon.totalAmmo;
                if (!waiting) {
                    StartCoroutine(WeaponSwitchTimer(currentWeapon.switchOutTime));
                }
            }

            if (item != currentWeapon) {
                item.WeaponGameobject.SetActive(false);
                anim.SetBool(item.weaponName, false);
            }
        }
    }

    IEnumerator WeaponSwitchTimer(float time) {
        waiting = true;
        yield return new WaitForSeconds(time);
        currentWeapon.WeaponGameobject.SetActive(true);
        anim.SetBool(currentWeapon.weaponName, true);
        waiting = false;
    }
    /* Up till this point */
}
