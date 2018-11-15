using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoldierShoot : MonoBehaviour {
    private int currentMagAmmo;
    private int currentTotalAmmo;
    private bool reloadAnimFinish = false;
    private bool magFilled = true;
    private bool canShoot = true;
    private bool isReloading = false;
    private bool isAiming = false;
    private float currentWeight = 0;
    private Animator anim;

    [System.Serializable]
    public class WeaponSetUp {

        [Tooltip("The weapon flash object that will play out the end of the gun")]

        public Weapons[] weapons;




        [System.Serializable]
        public class Weapons {
            [Tooltip("Weapon Name Used for selecting wich animation to chose in animator")]
            public string weaponName;

            [Tooltip("Weapon object for switching out the weapon")]
            public GameObject WeaponGameobject;

            [Tooltip("Bullet GameObject you want to use - leave clear if you do not want one")]
            public GameObject bulletGameobject;

            [Tooltip("Weapon object for switching out the weapon")]
            public float switchOutTime = 0.2f;

            [Tooltip("The empty transform placed at the end of the gun, used all sorts of gun logic")]
            public Transform weaponEnd;

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

            [Tooltip("Total Ammo")]
            public int totalAmmo;

            [Tooltip("The time it takes to reload")]
            public float reloadTime = 1f;

            [Tooltip("Which Key do you want to press to use the weapon")]
            public KeyCode switchWeaponKey;

            [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
            public float aimingZ = -10f;

            [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
            public float aimingY = -2f;

            [Tooltip("Used to make the Soldier aim properly when holding the weapon")]
            public float aimingX = 0f;

            [Tooltip("Weapon Sound should be added to this slot")]
            public AudioClip weaponAudio;
        }
    }

    [SerializeField]
    [Tooltip("How many weapons do you want to have")]
    WeaponSetUp weaponSetUp;
    public Transform spine;
    public WeaponSetUp.Weapons currentWeapon;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        currentWeapon = weaponSetUp.weapons[0];
        currentMagAmmo = currentWeapon.magazineSize;
        currentTotalAmmo = currentWeapon.totalAmmo;
    }
	


	// Update is called once per frame
	void Update () {
        adjustSpine();
        Aim(true);
		if (canShoot && currentMagAmmo > 0) {
            StartCoroutine(shootSpeed());
            
            Fire();
        }
        else if (currentMagAmmo <= 0){
            magFilled = false;
            StartCoroutine(ReloadAnimTimer());
        }
	}

    IEnumerator ReloadAnimTimer() {
        reloadAnimFinish = false;
        anim.SetBool("isReloading", true);
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        reloadAnimFinish = true;
        anim.SetBool("isReloading", false);
        Reload();
        reloadAnimFinish = false;
    }

    private void Reload() {
        if (reloadAnimFinish && magFilled == false) {
            reloadAnimFinish = false;
            currentTotalAmmo -= currentWeapon.magazineSize - currentMagAmmo;
            currentMagAmmo = currentWeapon.magazineSize;
            magFilled = true;
            isReloading = false;
        }
        else if (reloadAnimFinish && magFilled) {
            isReloading = false;
        }
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

    void Fire() {
        currentMagAmmo--;
        anim.SetTrigger("Fire");
        

        GameObject bullet = Instantiate(currentWeapon.bulletGameobject, currentWeapon.weaponEnd.transform.position, currentWeapon.weaponEnd.transform.rotation);
        BulletBehaviour bulletProperties = bullet.GetComponent<BulletBehaviour>();
        bulletProperties.setDmg(currentWeapon.weaponDamage);
        bulletProperties.setPlayerFriendly(true);
        bulletProperties.setSpeed(currentWeapon.bulletSpeed);
        bulletProperties.setDestroyTime(0.8f);
        bullet.transform.localScale = new Vector3(0.02f, 0.02f, 0.15f);
        
    }

    private void adjustSpine() {

        Vector3 eulerAngleOffset = Vector3.zero;
        eulerAngleOffset = new Vector3(currentWeapon.aimingX, currentWeapon.aimingY, currentWeapon.aimingZ);
        //ikSetUp.spine.transform.LookAt(playerToMouse);
        //ikSetUp.spine.transform.LookAt(Vector3.Lerp(ikSetUp.spine.transform.position, playerToMouse, 1.0f * Time.deltaTime));
        //ikSetUp.spine.transform.Rotate(playerToMouse);
        spine.transform.Rotate(eulerAngleOffset, Space.Self);
    }

    IEnumerator shootSpeed() {
        canShoot = false;
        yield return new WaitForSeconds(currentWeapon.fireRate);
        canShoot = true;

    }

}
